// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// TODO_UTF8STRING: Re-enable nullable
#nullable disable

namespace System.Net.Http
{
    public sealed partial class Utf8StringContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return stream.WriteAsync(_content.AsMemoryBytes()).AsTask();
        }
    }
}
