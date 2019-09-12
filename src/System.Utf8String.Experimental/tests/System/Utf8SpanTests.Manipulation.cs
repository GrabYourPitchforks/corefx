// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Buffers;
using System.Collections.Generic;
using System.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public partial class Utf8SpanTests
    {
        private delegate Utf8Span.SplitResult Utf8SpanSplitDelegate(Utf8Span span, Utf8StringSplitOptions splitOptions);

        [Theory]
        [MemberData(nameof(SplitData_CharSeparator))]
        public static void Split_Char(ustring source, char separator, Range[] expectedRanges)
        {
            SplitTest_Common(source, (span, splitOptions) => span.Split(separator, splitOptions), expectedRanges);
        }

        [Theory]
        [MemberData(nameof(SplitData_RuneSeparator))]
        public static void Split_Rune(ustring source, Rune separator, Range[] expectedRanges)
        {
            SplitTest_Common(source, (span, splitOptions) => span.Split(separator, splitOptions), expectedRanges);
        }

        [Theory]
        [MemberData(nameof(SplitData_Utf8SpanSeparator))]
        public static void Split_Utf8Span(ustring source, ustring separator, Range[] expectedRanges)
        {
            SplitTest_Common(source, (span, splitOptions) => span.Split(separator.AsSpan(), splitOptions), expectedRanges);
        }

        private static void SplitTest_Common(ustring source, Utf8SpanSplitDelegate splitAction, Range[] expectedRanges)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(source.AsBytes());
            Utf8Span span = boundedSpan.Span;
            int totalSpanLengthInBytes = span.Bytes.Length;
            source = null; // to avoid inadvertently using this for the remainder of the method

            // First, run the split with default options and make sure the ranges are equivalent

            List<Range> actualRanges = new List<Range>();
            foreach (Utf8Span slice in splitAction(span, Utf8StringSplitOptions.None))
            {
                actualRanges.Add(GetRangeOfSubspan(span, slice));
            }

            Assert.Equal(expectedRanges, actualRanges, new RangeEqualityComparer(totalSpanLengthInBytes));

            // Next, run the split with empty entries removed

            actualRanges = new List<Range>();
            foreach (Utf8Span slice in splitAction(span, Utf8StringSplitOptions.RemoveEmptyEntries))
            {
                actualRanges.Add(GetRangeOfSubspan(span, slice));
            }

            Assert.Equal(expectedRanges.Where(range => !range.IsEmpty(totalSpanLengthInBytes)), actualRanges, new RangeEqualityComparer(totalSpanLengthInBytes));

            // Next, run the split with results trimmed (but allowing empty results)

            expectedRanges = (Range[])expectedRanges.Clone(); // clone the array since we're about to mutate it
            for (int i = 0; i < expectedRanges.Length; i++)
            {
                expectedRanges[i] = GetRangeOfSubspan(span, span[expectedRanges[i]].Trim());
            }

            actualRanges = new List<Range>();
            foreach (Utf8Span slice in splitAction(span, Utf8StringSplitOptions.TrimEntries))
            {
                actualRanges.Add(GetRangeOfSubspan(span, slice));
            }

            Assert.Equal(expectedRanges, actualRanges, new RangeEqualityComparer(totalSpanLengthInBytes));

            // Finally, run the split both trimmed and with empty entries removed

            actualRanges = new List<Range>();
            foreach (Utf8Span slice in splitAction(span, Utf8StringSplitOptions.TrimEntries | Utf8StringSplitOptions.RemoveEmptyEntries))
            {
                actualRanges.Add(GetRangeOfSubspan(span, slice));
            }

            Assert.Equal(expectedRanges.Where(range => !range.IsEmpty(totalSpanLengthInBytes)), actualRanges, new RangeEqualityComparer(totalSpanLengthInBytes));
        }

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
