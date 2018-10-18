// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#if MS_IO_REDIST
namespace Microsoft.IO
#else
namespace System.IO
#endif
{
    public static partial class File
    {
        private static void InternalWriteAllBytes(string path, ReadOnlySpan<byte> bytes)
        {
            Debug.Assert(path != null);
            Debug.Assert(path.Length != 0);

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                fs.Write(bytes);
            }
        }

        public static Utf8String[] ReadAllLinesUtf8(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (path.Length == 0)
                throw new ArgumentException(SR.Argument_EmptyPath, nameof(path));

            using (var stream = OpenRead(path))
            {
                return ReadAllLinesUtf8(stream).ToArray();
            }
        }

        private static IEnumerable<Utf8String> ReadAllLinesUtf8(FileStream stream)
        {
            const int RENTED_BUFFER_SIZE = 4096;

            List<ArraySegment<byte>> inUseBuffers = new List<ArraySegment<byte>>();
            Stack<byte[]> freeBuffers = new Stack<byte[]>();

            byte[] RentBuffer()
            {
                if (!freeBuffers.TryPop(out byte[] retVal))
                {
                    retVal = ArrayPool<byte>.Shared.Rent(RENTED_BUFFER_SIZE);
                }
                Debug.Assert(retVal != null);
                return retVal;
            }

            Utf8String GetUtf8String(ArraySegment<byte> currentArraySegment)
            {
                int totalLength = currentArraySegment.Count;
                foreach (var buffer in inUseBuffers)
                {
                    checked
                    {
                        totalLength += buffer.Count;
                    }
                }

                return Utf8String.Create(totalLength, (currentArraySegment, inUseBuffers, freeBuffers), (destSpan, state) =>
                {
                    state.currentArraySegment.AsSpan().CopyTo(destSpan);
                    destSpan = destSpan.Slice(state.currentArraySegment.Count);

                    // Don't push the current array segment into the free list - it might still have unparsed data we need to look at

                    foreach (var buffer in state.inUseBuffers)
                    {
                        buffer.AsSpan().CopyTo(destSpan);
                        state.freeBuffers.Push(buffer.Array);
                        destSpan = destSpan.Slice(buffer.Count);
                    }

                    if (state.inUseBuffers.Count > 0)
                    {
                        state.inUseBuffers.Clear();
                    }

                    Debug.Assert(destSpan.IsEmpty, "Destination span should've been fully consumed.");
                });
            }

            bool ignoreLineFeedAtStartOfBuffer = false;

            try
            {
                while (true)
                {
                    byte[] currentBuffer = RentBuffer();

                ContinueWithoutRentingNewBuffer:
                    int bufferLength = 0;

                    // Loop until EOF or filled buffer

                    int bytesRead;
                    do
                    {
                        int remainingBufferLength = currentBuffer.Length - bufferLength;
                        bytesRead = stream.Read(currentBuffer, bufferLength, remainingBufferLength);
                        bufferLength += bytesRead;
                    }
                    while (bytesRead != 0 && bufferLength < currentBuffer.Length);

                    // At this point, the buffer is full, or we reached EOF.
                    // Regardless, we'll try searching for CR or LF.

                    ArraySegment<byte> currentBufferSegment = new ArraySegment<byte>(currentBuffer, 0, bufferLength);
                    int indexOfCrLf;

                    if (ignoreLineFeedAtStartOfBuffer)
                    {
                        ignoreLineFeedAtStartOfBuffer = false;
                        if (currentBufferSegment.Count > 0 && currentBufferSegment[0] == (byte)'\n')
                        {
                            currentBufferSegment = currentBufferSegment.Slice(1);
                        }
                    }

                    while (currentBufferSegment.Count > 0)
                    {
                        // Extract as many as we can.

                        indexOfCrLf = currentBufferSegment.AsSpan().IndexOfAny((byte)'\r', (byte)'\n');
                        if (indexOfCrLf < 0)
                        {
                            // Didn't find EOL characters - process final block
                            break;
                        }
                        else
                        {
                            yield return GetUtf8String(currentBufferSegment.Slice(0, indexOfCrLf));

                            // If this is CRLF ("\r\n"), only count it as a single newline.

                            byte eolChar = currentBufferSegment[indexOfCrLf];
                            currentBufferSegment = currentBufferSegment.Slice(indexOfCrLf + 1);

                            if (eolChar == (byte)'\r')
                            {
                                if (currentBufferSegment.Count == 0)
                                {
                                    // The next byte might be LF, which we need to ignore
                                    ignoreLineFeedAtStartOfBuffer = true;
                                    continue;
                                }
                                else if (currentBufferSegment[0] == (byte)'\n')
                                {
                                    currentBufferSegment = currentBufferSegment.Slice(1);
                                }
                            }
                        }
                    }

                    // No CR or LF found - if we're at EOF then return this and we're finished - otherwise continue loop
                    if (bytesRead == 0)
                    {
                        // EOF

                        yield return GetUtf8String(currentBufferSegment);
                        ArrayPool<byte>.Shared.Return(currentBuffer);
                        yield break;
                    }
                    else
                    {
                        // Not EOF - append this segment to our buffer chain and keep going

                        if (currentBufferSegment.Count > 0)
                        {
                            inUseBuffers.Add(currentBufferSegment);
                            continue;
                        }
                        else
                        {
                            goto ContinueWithoutRentingNewBuffer;
                        }
                    }
                }
            }
            finally
            {
                // Return unused buffers to the pool
                foreach (var buffer in freeBuffers)
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }

        public static Utf8String ReadAllTextUtf8(string path)
        {
            // bufferSize == 1 used to avoid unnecessary buffer in FileStream
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1))
            {
                long fileLength = fs.Length;
                if (fileLength > int.MaxValue)
                {
                    throw new IOException(SR.IO_FileTooLong2GB);
                }
                else if (fileLength == 0)
                {
#if !MS_IO_REDIST
                    // Some file systems (e.g. procfs on Linux) return 0 for length even when there's content.
                    // Thus we need to assume 0 doesn't mean empty.
                    return ReadAllTextUtf8UnknownLength(fs);
#endif
                }

                return Utf8String.Create((int)fileLength, fs, (span, fileStream) =>
                {
                    while (!span.IsEmpty)
                    {
                        int n = fileStream.Read(span);
                        if (n == 0)
                            throw Error.GetEndOfFile();
                        span = span.Slice(n);
                    }
                });
            }
        }

#if !MS_IO_REDIST
        private static Utf8String ReadAllTextUtf8UnknownLength(FileStream fs)
        {
            byte[] rentedArray = null;
            Span<byte> buffer = stackalloc byte[512];
            try
            {
                int bytesRead = 0;
                while (true)
                {
                    if (bytesRead == buffer.Length)
                    {
                        uint newLength = (uint)buffer.Length * 2;
                        if (newLength > MaxByteArrayLength)
                        {
                            newLength = (uint)Math.Max(MaxByteArrayLength, buffer.Length + 1);
                        }

                        byte[] tmp = ArrayPool<byte>.Shared.Rent((int)newLength);
                        buffer.CopyTo(tmp);
                        if (rentedArray != null)
                        {
                            ArrayPool<byte>.Shared.Return(rentedArray);
                        }
                        buffer = rentedArray = tmp;
                    }

                    Debug.Assert(bytesRead < buffer.Length);
                    int n = fs.Read(buffer.Slice(bytesRead));
                    if (n == 0)
                    {
                        return new Utf8String(buffer.Slice(0, bytesRead));
                    }
                    bytesRead += n;
                }
            }
            finally
            {
                if (rentedArray != null)
                {
                    ArrayPool<byte>.Shared.Return(rentedArray);
                }
            }
        }
#endif

        public static void WriteAllTextUtf8(string path, Utf8String contents)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (path.Length == 0)
                throw new ArgumentException(SR.Argument_EmptyPath, nameof(path));

            // Utf8String.AsSpan() extension method below handles null inputs correctly.
            InternalWriteAllBytes(path, contents.AsBytes());
        }
    }
}
