﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Memory
{
    public static partial class BoundedMemory
    {
        private static Implementation<T> AllocateWithoutDataPopulation<T>(int elementCount, PoisonPagePlacement placement) where T : unmanaged
        {
            // On non-Windows platforms, we don't yet have support for changing the permissions of individual pages.

            return new Implementation<T>(elementCount);
        }

        private sealed class Implementation<T> : BoundedMemory<T> where T : unmanaged
        {
            private readonly T[] _buffer;

            public Implementation(int elementCount)
            {
                _buffer = new T[elementCount];
            }

            public override bool IsReadonly => false;

            public override Memory<T> Memory => _buffer;

            public override Span<T> Span => _buffer;

            public override void Dispose()
            {
                // no-op
            }

            public override void MakeReadonly()
            {
                // no-op
            }

            public override void MakeWriteable()
            {
                // no-op
            }
        }
    }
}
