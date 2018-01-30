// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

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
            if (TryGetDelegateDangerous<TDelegate>(name, out var stub))
            {
                result = DelegateWrapperFactory<TDelegate>.WrapDelegate(_handle, stub);
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
                ilGen.Emit(OpCodes.Ldloca_S, refAddedLocal);
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

                ilGen.Emit(OpCodes.Leave_S, afterFinallyBlockLabel);

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
                ilGen.MarkLabel(afterFinallyBlockLabel);

                // return retVal;

                if (retValLocal != null)
                {
                    ilGen.Emit(OpCodes.Ldloc_S, retValLocal);
                }
                ilGen.Emit(OpCodes.Ret);

                // We actually created a dynamic method with a hidden 'this' parameter, and this parameter messed up
                // the delegate signature. Passing a real 'this' parameter in calls to CreateDelegate will allow us
                // to close over the first parameter and restore the proper TDelegate signature. Once we compile the
                // method, future calls to CreateDelegate will be fast.

                typeof(DynamicMethod).InvokeMember("GetMethodDescriptor", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, dynamicMethod, null);
                var methodHandle = typeof(DynamicMethod).GetField("m_methodHandle", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(dynamicMethod);
                typeof(RuntimeHelpers).InvokeMember("_CompileMethod", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, null, new[] { methodHandle });

                return (procInfo) => (TDelegate)(object)dynamicMethod.CreateDelegate(typeof(TDelegate), target: procInfo);
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
