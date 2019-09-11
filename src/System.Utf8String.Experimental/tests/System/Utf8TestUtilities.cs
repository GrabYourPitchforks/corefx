﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace System.Tests
{
    public static class Utf8TestUtilities
    {
        private static readonly Lazy<Func<int, Utf8String>> _utf8StringFactory = CreateUtf8StringFactory();

        private static Lazy<Func<int, Utf8String>> CreateUtf8StringFactory()
        {
            return new Lazy<Func<int, Utf8String>>(() =>
            {
                MethodInfo fastAllocateMethod = typeof(Utf8String).GetMethod("FastAllocate", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int) }, null);
                Assert.NotNull(fastAllocateMethod);
                return (Func<int, Utf8String>)fastAllocateMethod.CreateDelegate(typeof(Func<int, Utf8String>));
            });
        }

        public static int GetByteLength(this Utf8String value)
        {
            return value.AsBytes().Length;
        }

        public unsafe static bool IsNull(this Utf8Span span)
        {
            return Unsafe.AreSame(ref Unsafe.AsRef<byte>(null), ref MemoryMarshal.GetReference(span.Bytes));
        }

        /// <summary>
        /// Parses an expression of the form "a..b" and returns a <see cref="Range"/>.
        /// </summary>
        public static Range ParseRangeExpr(ReadOnlySpan<char> expression)
        {
            int idxOfDots = expression.IndexOf("..", StringComparison.Ordinal);
            if (idxOfDots < 0)
            {
                goto Error;
            }

            ReadOnlySpan<char> firstPart = expression[..idxOfDots].Trim();
            Index firstIndex = Index.Start;

            if (!firstPart.IsWhiteSpace())
            {
                bool fromEnd = false;

                if (!firstPart.IsEmpty && firstPart[0] == '^')
                {
                    fromEnd = true;
                    firstPart = firstPart[1..];
                }

                if (!int.TryParse(firstPart, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out int startIndex))
                {
                    goto Error;
                }

                firstIndex = new Index(startIndex, fromEnd);
            }

            ReadOnlySpan<char> secondPart = expression[(idxOfDots + 2)..].Trim();
            Index secondIndex = Index.End;

            if (!secondPart.IsWhiteSpace())
            {
                bool fromEnd = false;

                if (!secondPart.IsEmpty && secondPart[0] == '^')
                {
                    fromEnd = true;
                    secondPart = secondPart[1..];
                }

                if (!int.TryParse(secondPart, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out int endIndex))
                {
                    goto Error;
                }

                secondIndex = new Index(endIndex, fromEnd);
            }

            return new Range(firstIndex, secondIndex);

        Error:
            throw new ArgumentException($"Range expression '{expression.ToString()}' is invalid.");
        }

        /// <summary>
        /// Mimics returning a literal <see cref="Utf8String"/> instance.
        /// </summary>
        public static unsafe Utf8String u8(string str)
        {
            if (str is null)
            {
                return null;
            }
            else if (str.Length == 0)
            {
                return Utf8String.Empty;
            }

            // First, transcode UTF-16 to UTF-8. We use direct by-scalar transcoding here
            // because we have good reference implementation tests for this and it'll help
            // catch any errors we introduce to our bulk transcoding implementations.

            MemoryStream memStream = new MemoryStream();

            Span<byte> utf8Bytes = stackalloc byte[4]; // 4 UTF-8 code units is the largest any scalar value can be encoded as

            int index = 0;
            while (index < str.Length)
            {
                if (Rune.TryGetRuneAt(str, index, out Rune value) && value.TryEncodeToUtf8(utf8Bytes, out int bytesWritten))
                {
                    memStream.Write(utf8Bytes.Slice(0, bytesWritten));
                    index += value.Utf16SequenceLength;
                }
                else
                {
                    throw new ArgumentException($"String '{str}' is not a well-formed UTF-16 string.");
                }
            }

            Assert.True(memStream.TryGetBuffer(out ArraySegment<byte> buffer));

            // Now allocate a UTF-8 string instance and set this as the contents.
            // We do it this way rather than go through a public ctor because we don't
            // want the "control" part of our unit tests to depend on the code under test.

            Utf8String newUtf8String = _utf8StringFactory.Value(buffer.Count);
            fixed (byte* pNewUtf8String = newUtf8String)
            {
                buffer.AsSpan().CopyTo(new Span<byte>(pNewUtf8String, newUtf8String.GetByteLength()));
            }

            return newUtf8String;
        }
    }
}
