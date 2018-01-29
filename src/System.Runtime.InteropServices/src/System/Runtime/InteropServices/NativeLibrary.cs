// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        
        public bool TryGetDelegateDangerous<T>(string name, out T result)
        {
            IntPtr farproc = _handle.GetProcAddress(name);
            if (farproc != IntPtr.Zero)
            {
                result = Marshal.GetDelegateForFunctionPointer<T>(farproc);
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
    }
}
