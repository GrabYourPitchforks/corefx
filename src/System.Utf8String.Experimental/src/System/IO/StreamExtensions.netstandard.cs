// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.IO
{
    internal static class StreamExtensions
    {
        public static int Read(this Stream stream, Span<byte> buffer)
        {
            byte[] rented = ArrayPool<byte>.Shared.Rent(buffer.Length);
            int read = stream.Read(rented, 0, buffer.Length);
            rented.AsSpan(0, read).CopyTo(buffer);
            ArrayPool<byte>.Shared.Return(rented);
            return read;
        }
    }
}
