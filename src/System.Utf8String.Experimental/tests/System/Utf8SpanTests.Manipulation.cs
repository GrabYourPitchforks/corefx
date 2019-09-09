// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Tests;
using Xunit;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        [Theory]
        [MemberData(nameof(Trim_TestData))]
        public static void Trim(string input)
        {
            // Arrange

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);
            Utf8Span span = boundedSpan.Span;

            // Act

            Utf8Span trimmed = span.Trim();

            // Assert
            // Compute the trim manually and ensure it matches the trimmed span's characteristics.

            ReadOnlySpan<byte> utf8Bytes = span.Bytes;
            while (!utf8Bytes.IsEmpty)
            {
                OperationStatus status = Rune.DecodeFromUtf8(utf8Bytes, out Rune decodedRune, out int bytesConsumed);
                Assert.Equal(OperationStatus.Done, status);

                if (!Rune.IsWhiteSpace(decodedRune))
                {
                    break;
                }

                utf8Bytes = utf8Bytes.Slice(bytesConsumed);
            }
            while (!utf8Bytes.IsEmpty)
            {
                OperationStatus status = Rune.DecodeLastFromUtf8(utf8Bytes, out Rune decodedRune, out int bytesConsumed);
                Assert.Equal(OperationStatus.Done, status);

                if (!Rune.IsWhiteSpace(decodedRune))
                {
                    break;
                }

                utf8Bytes = utf8Bytes[..^bytesConsumed];
            }

            Assert.True(trimmed.Bytes == utf8Bytes); // must be an exact buffer match (address + length)
        }

        [Theory]
        [MemberData(nameof(Trim_TestData))]
        public static void TrimEnd(string input)
        {
            // Arrange

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);
            Utf8Span span = boundedSpan.Span;

            // Act

            Utf8Span trimmed = span.TrimEnd();

            // Assert
            // Compute the trim manually and ensure it matches the trimmed span's characteristics.

            ReadOnlySpan<byte> utf8Bytes = span.Bytes;
            while (!utf8Bytes.IsEmpty)
            {
                OperationStatus status = Rune.DecodeLastFromUtf8(utf8Bytes, out Rune decodedRune, out int bytesConsumed);
                Assert.Equal(OperationStatus.Done, status);

                if (!Rune.IsWhiteSpace(decodedRune))
                {
                    break;
                }

                utf8Bytes = utf8Bytes[..^bytesConsumed];
            }

            Assert.True(trimmed.Bytes == utf8Bytes); // must be an exact buffer match (address + length)
        }

        [Theory]
        [MemberData(nameof(Trim_TestData))]
        public static void TrimStart(string input)
        {
            // Arrange

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);
            Utf8Span span = boundedSpan.Span;

            // Act

            Utf8Span trimmed = span.TrimStart();

            // Assert
            // Compute the trim manually and ensure it matches the trimmed span's characteristics.

            ReadOnlySpan<byte> utf8Bytes = span.Bytes;
            while (!utf8Bytes.IsEmpty)
            {
                OperationStatus status = Rune.DecodeFromUtf8(utf8Bytes, out Rune decodedRune, out int bytesConsumed);
                Assert.Equal(OperationStatus.Done, status);

                if (!Rune.IsWhiteSpace(decodedRune))
                {
                    break;
                }

                utf8Bytes = utf8Bytes.Slice(bytesConsumed);
            }

            Assert.True(trimmed.Bytes == utf8Bytes); // must be an exact buffer match (address + length)
        }
    }
}
