// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// TODO_UTF8STRING: Re-enable nullable
#nullable disable

namespace System.Net.Http
{
    public sealed partial class Utf8StringContent
    {
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            byte[] rented = ArrayPool<byte>.Shared.Rent(_content.Length);
            _content.AsBytes().CopyTo(rented);
            await stream.WriteAsync(rented, 0, _content.Length).ConfigureAwait(false);
            ArrayPool<byte>.Shared.Return(rented);
        }
    }
}
