// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Text
{
    internal static class EncodingExtensions
    {
        public static unsafe string GetString(this Encoding encoding, ReadOnlySpan<byte> buffer)
        {
            fixed (byte* pBuffer = &MemoryMarshal.GetReference(buffer))
            {
                byte dummy = default;
                return encoding.GetString((pBuffer != null) ? pBuffer : &dummy, buffer.Length);
            }
        }
    }
}
