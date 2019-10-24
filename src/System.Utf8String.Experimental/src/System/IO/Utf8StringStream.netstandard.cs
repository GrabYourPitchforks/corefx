// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

// TODO_UTF8STRING: Re-enable nullable
#nullable disable

namespace System.IO
{
    internal sealed partial class Utf8StringStream
    {
        private int Read(Span<byte> buffer)
        {
            ReadOnlySpan<byte> contentToWrite = _content.AsBytes(_position);
            if (buffer.Length < contentToWrite.Length)
            {
                contentToWrite = contentToWrite.Slice(buffer.Length);
            }

            contentToWrite.CopyTo(buffer);
            _position += contentToWrite.Length;

            return contentToWrite.Length;
        }

        private ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return new ValueTask<int>(Read(buffer.Span));
        }
    }
}
