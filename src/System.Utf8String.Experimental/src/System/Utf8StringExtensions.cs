// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public static class Utf8StringExtensions
    {
        /*
         * Extension methods for System.IO.TextReader & TextWriter
         */

        public static void Write(this TextWriter writer, Utf8String? value)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            writer.Write(value?.ToString());
        }

        public static void Write(this TextWriter writer, Utf8Span buffer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

             // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            writer.Write(buffer.ToString());
        }

        public static Task WriteAsync(this TextWriter writer, Utf8String? value, CancellationToken cancellationToken = default)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            return writer.WriteAsync(MemoryExtensions.AsMemory(value?.ToString()), cancellationToken);
        }

        public static void WriteLine(this TextWriter writer, Utf8String? value)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            writer.WriteLine(value?.ToString());
        }

         public static void WriteLine(this TextWriter writer, Utf8Span buffer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

             // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            writer.WriteLine(buffer.ToString());
        }

        public static Task WriteLineAsync(this TextWriter writer, Utf8String? value, CancellationToken cancellationToken = default)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // TODO_UTF8STRING: Call the proper non-allocating overload when it comes online.

            return writer.WriteLineAsync(MemoryExtensions.AsMemory(value?.ToString()), cancellationToken);
        }
    }
}
