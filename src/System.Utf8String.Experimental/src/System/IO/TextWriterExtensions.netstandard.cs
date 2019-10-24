// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    internal static class TextWriterExtensions
    {
        public static async Task WriteAsync(this TextWriter writer, ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
        {
            char[] rented = ArrayPool<char>.Shared.Rent(buffer.Length);
            buffer.CopyTo(rented);
            await writer.WriteAsync(rented, 0, buffer.Length).ConfigureAwait(false);
            ArrayPool<char>.Shared.Return(rented);
        }

        public static async Task WriteLineAsync(this TextWriter writer, ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
        {
            char[] rented = ArrayPool<char>.Shared.Rent(buffer.Length);
            buffer.CopyTo(rented);
            await writer.WriteLineAsync(rented, 0, buffer.Length).ConfigureAwait(false);
            ArrayPool<char>.Shared.Return(rented);
        }
    }
}
