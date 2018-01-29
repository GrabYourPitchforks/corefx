// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Runtime.InteropServices
{
    internal sealed partial class NativeLibraryHandle : CriticalFinalizerObject
    {
        private const int OneReference = 1 << 2;
        private const int NeedsToCloseMask = 1 << 1;
        private const int HasClosedMask = 1 << 0;

        private readonly IntPtr _rawHandle;
        private int _refCount;

        public NativeLibraryHandle(string name, DllImportSearchPath paths)
        {
            Load(name, paths, out _rawHandle);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        ~NativeLibraryHandle()
        {
            Unload();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRef(ref bool refAdded)
        {
            AddRefInternal(ref refAdded);
            if (!refAdded)
            {
                // TODO: Replace with SR
                throw new Exception("Handle has been closed.");
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddRefInternal(ref bool refAdded)
        {
            if ((Interlocked.Add(ref _refCount, OneReference) & (NeedsToCloseMask | HasClosedMask)) != 0)
            {
                // We just added a reference, but the handle is in the process of being closed, so we need
                // to release the reference just in case we're blocking final disposal.
                Release();
            }
            else
            {
                refAdded = true;
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Close()
        {
            if ((InterlockedOr(ref _refCount, NeedsToCloseMask) & ~(NeedsToCloseMask | HasClosedMask)) == 0)
            {
                // There are no remaining references to this handle. Unload.
                Unload();
            }
        }

        public IntPtr GetProcAddress(string procName)
        {
            bool refAdded = false;
            try
            {
                AddRef(ref refAdded);
                return GetProcAddressInternal(procName);
            }
            finally
            {
                if (refAdded)
                {
                    Release();
                }
            }
        }

        public IntPtr GetRawHandle()
        {
            if ((Volatile.Read(ref _refCount) & (NeedsToCloseMask | HasClosedMask)) != 0)
            {
                // TODO: replace with SR
                throw new Exception("Handle has been closed.");
            }
            return _rawHandle;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InterlockedOr(ref int location, int value)
        {
            var originalValue = Volatile.Read(ref location);

            if ((originalValue & value) != value)
            {
                while (true)
                {
                    var actualValueBeforeCompXchg = Interlocked.CompareExchange(ref location, originalValue | value, originalValue);
                    if (actualValueBeforeCompXchg == originalValue)
                    {
                        break;
                    }

                    originalValue = actualValueBeforeCompXchg;
                }
            }

            return originalValue;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release()
        {
            // Call unload only if this is marked 'needs to close' and we're the last remaining
            // reference (all ref count bits are clear).
            if (Interlocked.Add(ref _refCount, -OneReference) == NeedsToCloseMask)
            {
                Unload();
            }
        }
    }
}
