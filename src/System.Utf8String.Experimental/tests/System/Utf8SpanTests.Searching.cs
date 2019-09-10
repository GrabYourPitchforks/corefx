// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        [Theory]
        [InlineData("", '\0', null)]
        [InlineData("Hello", 'l', "2..3")]
        [InlineData("x\ud83d\ude00y", '\ud83d', null)] // U+1F600 GRINNING FACE (surrogates should never match)
        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", '\0', null)]
        [InlineData("xyz\0xyz\0", '\0', "3..4")]
        public static void TryFind_Char(string sourceText, char searchChar, string expectedRange)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
            Utf8Span span = boundedSpan.Span;

            if (span.TryFind(searchChar, out Range matchedRange))
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Null(expectedRange);
            }
        }

        [Theory]
        [InlineData("", '\0', null)]
        [InlineData("Hello", 'l', "2..3")]
        [InlineData("x\U0001F600y", 0x1F600, "1..5")] // U+1F600 GRINNING FACE
        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", '\0', null)]
        [InlineData("xyz\0xyz\0", '\0', "3..4")]
        public static void TryFind_Rune(string sourceText, uint searchRune, string expectedRange)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
            Utf8Span span = boundedSpan.Span;

            if (span.TryFind(new Rune(searchRune), out Range matchedRange))
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Null(expectedRange);
            }
        }

        [Theory]
        [InlineData("", '\0', null)]
        [InlineData("Hello", 'l', "3..4")]
        [InlineData("x\ud83d\ude00y", '\ud83d', null)] // U+1F600 GRINNING FACE (surrogates should never match)
        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", '\0', null)]
        [InlineData("xyz\0xyz\0", '\0', "^1..")]
        public static void TryFindLast_Char(string sourceText, char searchChar, string expectedRange)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
            Utf8Span span = boundedSpan.Span;

            if (span.TryFindLast(searchChar, out Range matchedRange))
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Null(expectedRange);
            }
        }

        [Theory]
        [InlineData("", '\0', null)]
        [InlineData("Hello", 'l', "3..4")]
        [InlineData("x\U0001F600y", 0x1F600, "1..5")] // U+1F600 GRINNING FACE
        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", '\0', null)]
        [InlineData("xyz\0xyz\0", '\0', "^1..")]
        public static void TryFindLast_Rune(string sourceText, uint searchRune, string expectedRange)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
            Utf8Span span = boundedSpan.Span;

            if (span.TryFindLast(new Rune(searchRune), out Range matchedRange))
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Null(expectedRange);
            }
        }

        private static void AssertRangesEqual(int originalLength, Range range1, Range range2)
        {
            (int offset1, int length1) = range1.GetOffsetAndLength(originalLength);
            (int offset2, int length2) = range2.GetOffsetAndLength(originalLength);

            Assert.Equal(offset1, offset2);
            Assert.Equal(length1, length2);
        }
    }
}
