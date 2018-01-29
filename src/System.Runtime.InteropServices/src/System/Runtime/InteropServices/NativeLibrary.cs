// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Runtime.InteropServices
{
    public sealed class NativeLibrary
    {
        private readonly NativeLibraryHandle _handle;

        private NativeLibrary(NativeLibraryHandle handle)
        {
            _handle = handle;
        }

        public static bool TryOpen(string name, DllImportSearchPath paths, out NativeLibrary result)
        {
            // TODO: This shouldn't throw on error.
            var handle = new NativeLibraryHandle(name, paths);
            result = new NativeLibrary(handle);
            return true;
        }

        public bool TryGetDelegateDangerous<TDelegate>(string name, out TDelegate result) where TDelegate : class
        {
            IntPtr farproc = _handle.GetProcAddress(name);
            if (farproc != IntPtr.Zero)
            {
                result = Marshal.GetDelegateForFunctionPointer<TDelegate>(farproc);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public bool TryGetDelegateSafe<TDelegate>(string name, out TDelegate result) where TDelegate : class
        {
            IntPtr farproc = _handle.GetProcAddress(name);
            if (farproc != IntPtr.Zero)
            {
                result = DelegateWrapperFactory<TDelegate>.WrapDelegate(_handle, Marshal.GetDelegateForFunctionPointer<TDelegate>(farproc));
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public void Unload()
        {
            _handle.Close();
        }

        private static class DelegateWrapperFactory<TDelegate> where TDelegate : class
        {
            private static readonly Func<ProcInfo, TDelegate> _factory = CreateFactory();

            private static Func<ProcInfo, TDelegate> CreateFactory()
            {
                // Given TDelegate = (T1, T2, ..., Tn) -> TRet, we're going to create a wrapper which when closed
                // over a 'this' object has a signature which matches TDelegate. This means the wrapper method
                // is really (ProcInfo, T1, T2, ..., Tn) -> TRet, and the implementation looks something
                // similar to below.
                //
                // static TRet Wrapper(ProcInfo @this, T1 arg1, T2 arg2, ..., Tn argn) {
                //   bool refAdded = false;
                //   TRet retVal;
                //   try {
                //     @this.NativeLibraryHandle.AddRef(ref refAdded);
                //     retVal = @this.DelegateToStub(arg1, arg2, ..., argn);
                //   } finally {
                //     if (refAdded) {
                //       @this.NativeLibraryHandle.Release();
                //     }
                //   }
                //   return retVal;
                // }

                var invokeMethod = typeof(TDelegate).GetMethod("Invoke");

                var returnType = invokeMethod.ReturnType;
                var parameters = invokeMethod.GetParameters();
                var parameterTypes = new Type[parameters.Length + 1];
                parameterTypes[0] = typeof(ProcInfo);
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterTypes[i + 1] = parameters[i].ParameterType;
                }

                var dynamicMethod = new DynamicMethod(
                    name: "Wrapper",
                    returnType: returnType,
                    parameterTypes: parameterTypes,
                    owner: typeof(ProcInfo),
                    skipVisibility: true)
                {
                    InitLocals = false // we'll handle this ourselves
                };

                var ilGen = dynamicMethod.GetILGenerator();
                var refAddedLocal = ilGen.DeclareLocal(typeof(bool));
                var retValLocal = (returnType != typeof(void)) ? ilGen.DeclareLocal(returnType) : null;
                var afterFinallyBlockLabel = ilGen.DefineLabel();
                var afterReleaseCalledLabel = ilGen.DefineLabel();

                // bool refAdded = false;

                ilGen.Emit(OpCodes.Ldc_I4_0);
                ilGen.Emit(OpCodes.Stloc_S, refAddedLocal);

                // try {

                ilGen.BeginExceptionBlock();

                // @this.NativeLibraryHandle.AddRef(ref refAdded);

                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, typeof(ProcInfo).GetField(nameof(ProcInfo.NativeLibraryHandle)));
                ilGen.Emit(OpCodes.Ldarga_S, refAddedLocal);
                ilGen.Emit(OpCodes.Call, typeof(NativeLibraryHandle).GetMethod("AddRef")); // call instead of callvirt since we know not null

                // retVal = @this.DelegateToStub(arg1, arg2, ..., argn);

                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, typeof(ProcInfo).GetField(nameof(ProcInfo.DelegateToStub)));
                for (int i = 1; i < parameterTypes.Length; i++)
                {
                    ilGen.Emit(OpCodes.Ldarg, i);
                }
                ilGen.Emit(OpCodes.Callvirt, invokeMethod);

                if (retValLocal != null)
                {
                    ilGen.Emit(OpCodes.Stloc_S, retValLocal);
                }

                ilGen.Emit(OpCodes.Jmp, afterFinallyBlockLabel);

                // } finally {

                ilGen.BeginFinallyBlock();

                // if (refAdded) {
                //   @this.NativeLibraryHandle.Release();
                // }

                ilGen.Emit(OpCodes.Ldloc_S, refAddedLocal);
                ilGen.Emit(OpCodes.Brfalse_S, afterReleaseCalledLabel);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, typeof(ProcInfo).GetField(nameof(ProcInfo.NativeLibraryHandle)));
                ilGen.Emit(OpCodes.Call, typeof(NativeLibraryHandle).GetMethod("Release")); // call instead of callvirt since we know not null
                ilGen.MarkLabel(afterReleaseCalledLabel);

                // } // finally

                ilGen.EndExceptionBlock();

                // return retVal;

                if (retValLocal != null)
                {
                    ilGen.Emit(OpCodes.Ldloc_S, retValLocal);
                }
                ilGen.Emit(OpCodes.Ret);

                // We actually created a dynamic method with a hidden 'this' parameter, and this parameter messed up
                // the delegate signature. We close over a null 'this' in order to restore the original delegate sig.

                Delegate canonDelegate = dynamicMethod.CreateDelegate(typeof(TDelegate), target: null);
                RuntimeHelpers.PrepareDelegate(canonDelegate); // ensure jitted

                // The delegate that we just created isn't usable on its own, but we can use it as a blueprint
                // to create other wrappers of this same type by substituting the 'this' parameter.

                var dm2 = new DynamicMethod(
                    "Factory",
                    returnType: typeof(TDelegate),
                    parameterTypes: new[] { typeof(object), typeof(IntPtr) },
                    owner: typeof(TDelegate),
                    skipVisibility: true);

                var ilg2 = dm2.GetILGenerator();

                ilg2.Emit(OpCodes.Ldarg_0);
                ilg2.Emit(OpCodes.Ldarg_1);
                ilg2.Emit(OpCodes.Newobj, typeof(TDelegate).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(object), typeof(IntPtr) }, null));
                ilg2.Emit(OpCodes.Ret);

                var delFac = (Func<object, IntPtr, TDelegate>)dm2.CreateDelegate(typeof(Func<object, IntPtr, TDelegate>));
                RuntimeHelpers.PrepareDelegate(delFac);

                GCHandle.Alloc(canonDelegate); // we don't ever want this to be GCed
                var canonMethodImpl = canonDelegate.Method.MethodHandle.Value;
                return (procInfo) =>
                {
                    return delFac(procInfo, canonMethodImpl);
                };
            }

            public static TDelegate WrapDelegate(NativeLibraryHandle handle, TDelegate delegateToStub)
            {
                return _factory(new ProcInfo
                {
                    NativeLibraryHandle = handle,
                    DelegateToStub = delegateToStub
                });
            }

            private sealed class ProcInfo
            {
                public TDelegate DelegateToStub;
                public NativeLibraryHandle NativeLibraryHandle;
            }
        }
    }
}
