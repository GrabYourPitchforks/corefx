// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;

namespace System
{
    public static class Utf8StringFactory
    {
        /*
         * Factories from ReadOnlySequence<byte>
         */

        public static Utf8String Create(ReadOnlySequence<byte> buffer)
        {
            return Utf8String.Create(SafeLength(buffer.Length), buffer, (span, buffer) =>
            {
                buffer.CopyTo(span);
            });
        }

        public static Utf8String CreateRelaxed(ReadOnlySequence<byte> buffer)
        {
            return Utf8String.CreateRelaxed(SafeLength(buffer.Length), buffer, (span, buffer) =>
            {
                buffer.CopyTo(span);
            });
        }

        public static Utf8String UnsafeCreateWithoutValidation(ReadOnlySequence<byte> buffer)
        {
            return Utf8String.UnsafeCreateWithoutValidation(SafeLength(buffer.Length), buffer, (span, buffer) =>
            {
                buffer.CopyTo(span);
            });
        }

        /*
         * Factories from files
         */

        public static Utf8String CreateFromFile(string path)
        {
            return CreateFromFileCommon(path, Utf8String.Create);
        }

        private static unsafe Utf8String CreateFromFileCommon(string path, Func<int, (FileStream, IntPtr), SpanAction<byte, (FileStream fs, IntPtr bom)>, Utf8String> factory)
        {
            if (string.IsNullOrEmpty(path))
            {
                // TODO_UTF8STRING: Use a different exception for null vs. empty.
                throw new ArgumentNullException(nameof(path));
            }

            // bufferSize == 1 used to avoid unnecessary buffer in FileStream
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1))
            {
                byte* bom = stackalloc byte[3]
                {
                    (byte)fs.ReadByte(),
                    (byte)fs.ReadByte(),
                    (byte)fs.ReadByte()
                };

                if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                {
                    // Strip the UTF-8 BOM and process only the remainder of the input.
                    return factory(SafeLength(fs.Length - 3), (fs, IntPtr.Zero), (span, state) =>
                    {
                        CopyStreamToSpan(state.fs, span);
                    });
                }

                // This wasn't a UTF-8 BOM. We need to make sure the Utf8String instance
                // contains this data. We'll smuggle the data across as a pointer.

                return factory(SafeLength(fs.Length), (fs, (IntPtr)bom), (span, state) =>
                {
                    Span<byte> earlyData = new Span<byte>((byte*)state.bom, (int)Math.Min(3, (ulong)state.fs.Length));
                    earlyData.CopyTo(span);
                    CopyStreamToSpan(state.fs, span.Slice(earlyData.Length));
                });
            }
        }

        public static unsafe Utf8String CreateFromFileRelaxed(string path)
        {
            return CreateFromFileCommon(path, Utf8String.CreateRelaxed);
        }

        public static Utf8String UnsafeCreateFromFileWithoutValidation(string path)
        {
            return CreateFromFileCommon(path, Utf8String.UnsafeCreateWithoutValidation);
        }

        /*
         * Helpers
         */

        private static void CopyStreamToSpan(Stream inputStream, Span<byte> destination)
        {
            while (!destination.IsEmpty)
            {
                int bytesRead = inputStream.Read(destination);

                if (bytesRead == 0)
                {
                    // TODO_UTF8STRING: Do we need an exception for end of stream reached unexpectedly?
                    throw new IOException();
                }

                destination = destination.Slice(bytesRead);
            }

            if (inputStream.ReadByte() >= 0)
            {
                // TODO_UTF8STRING: Do we need an exception for the FileStream overrunning the buffer?
                throw new IOException();
            }
        }

        /// <summary>
        /// Given an arbitrary length <paramref name="length"/>, returns that value cast to an
        /// <see cref="int"/> if the value is a positive integer; otherwise returns <see cref="int.MaxValue"/>.
        /// This is useful for normalizing OOMs when the input data is too large to be processed
        /// successfully.
        /// </summary>
        private static int SafeLength(long length)
        {
            return (int)Math.Min((ulong)int.MaxValue, (ulong)length);
        }
    }
}
