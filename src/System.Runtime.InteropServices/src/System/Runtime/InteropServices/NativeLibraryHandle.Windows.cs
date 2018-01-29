// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Runtime.InteropServices
{
    internal sealed partial class NativeLibraryHandle
    {
        private static void Load(string name, DllImportSearchPath paths, out IntPtr handle)
        {
            // TODO: Cleanup 'paths'

            LoadInternal(name, (uint)paths, out handle);

            if (handle == IntPtr.Zero)
            {
                // TODO: Should we fail on specific error codes?
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static void LoadInternal(string name, uint flags, out IntPtr handle)
        {
            handle = NativeMethods.LoadLibraryEx(name, IntPtr.Zero, flags);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Unload()
        {
            // No-op if another thread has closed or is in the process of closing this handle
            if ((InterlockedOr(ref _refCount, HasClosedMask) & HasClosedMask) == 0)
            {
                NativeMethods.FreeLibrary(_rawHandle);
                GC.SuppressFinalize(this);
            }
        }

        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(
                [In] IntPtr hModule);

            [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "LoadLibraryExW", SetLastError = true)]
            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            public static extern IntPtr LoadLibraryEx(
                [In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
                [In] IntPtr hFile,
                [In] uint dwFlags);
        }
    }
}
