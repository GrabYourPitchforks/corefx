// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

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

            bool wasFound = span.TryFind(searchChar, out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, span.Contains(searchChar));
            Assert.Equal(wasFound && span.Bytes[..matchedRange.Start].IsEmpty, span.StartsWith(searchChar));

            (var before, var after) = span.SplitOn(searchChar);
            if (wasFound)
            {
                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(span.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
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

            bool wasFound = span.TryFind(new Rune(searchRune), out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, span.Contains(new Rune(searchRune)));
            Assert.Equal(wasFound && span.Bytes[..matchedRange.Start].IsEmpty, span.StartsWith(new Rune(searchRune)));

            (var before, var after) = span.SplitOn(new Rune(searchRune));
            if (wasFound)
            {
                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(span.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }
        }

        [Theory]
        [InlineData("", "", "..0")] // empty search terms should always match at the front of the search space
        [InlineData("", "\0", null)]
        [InlineData("Hello", "", "..0")] // empty search terms should always match at the front of the search space
        [InlineData("Hello", "l", "2..3")]
        [InlineData("x\U0001F600y", "\U0001F600", "1..5")] // U+1F600 GRINNING FACE
        [InlineData("x\ud83d\ude00y", "\u00F0", null)] // make sure we don't confuse U+00F0 with the [ F0 ] byte
        [InlineData("x\ud83d\ude00y", "y", "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", "\u2660", "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", "\0", null)]
        [InlineData("xyz\0xyz\0", "\0", "3..4")]
        [InlineData("xyz\0xyz\0", "yz", "1..3")]
        public static void TryFind_Utf8Span(string sourceText, string searchTerm, string expectedRange)
        {
            using BoundedUtf8Span boundedSourceTextSpan = new BoundedUtf8Span(sourceText);
            using BoundedUtf8Span boundedSearchTermSpan = new BoundedUtf8Span(searchTerm);

            Utf8Span sourceSpan = boundedSourceTextSpan.Span;
            Utf8Span searchSpan = boundedSearchTermSpan.Span;

            bool wasFound = sourceSpan.TryFind(searchSpan, out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(sourceSpan.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, sourceSpan.Contains(searchSpan));
            Assert.Equal(wasFound && sourceSpan.Bytes[..matchedRange.Start].IsEmpty, sourceSpan.StartsWith(searchSpan));

            (var before, var after) = sourceSpan.SplitOn(searchSpan);
            if (wasFound)
            {
                Assert.True(sourceSpan.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(sourceSpan.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(sourceSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
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

            bool wasFound = span.TryFindLast(searchChar, out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, span.Contains(searchChar));
            Assert.Equal(wasFound && span.Bytes[matchedRange.End..].IsEmpty, span.EndsWith(searchChar));

            (var before, var after) = span.SplitOnLast(searchChar);
            if (wasFound)
            {
                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(span.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
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

            bool wasFound = span.TryFindLast(new Rune(searchRune), out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, span.Contains(new Rune(searchRune)));
            Assert.Equal(wasFound && span.Bytes[matchedRange.End..].IsEmpty, span.EndsWith(new Rune(searchRune)));

            (var before, var after) = span.SplitOnLast(new Rune(searchRune));
            if (wasFound)
            {
                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(span.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }
        }

        [Theory]
        [InlineData("", "", "^0..")] // empty search terms should always match at the end of the search space
        [InlineData("", "\0", null)]
        [InlineData("Hello", "", "^0..")] // empty search terms should always match at the end of the search space
        [InlineData("Hello", "l", "3..4")]
        [InlineData("x\U0001F600y", "\U0001F600", "1..5")] // U+1F600 GRINNING FACE
        [InlineData("x\ud83d\ude00y", "\u00F0", null)] // make sure we don't confuse U+00F0 with the [ F0 ] byte
        [InlineData("x\ud83d\ude00y", "y", "^1..")]
        [InlineData("x\u2660\u2661\u2660\u2661y", "\u2660", "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        [InlineData("xyz", "\0", null)]
        [InlineData("xyz\0xyz\0", "\0", "^1..")]
        [InlineData("xyz\0xyz\0", "yz", "^3..^1")]
        public static void TryFindLast_Utf8Span(string sourceText, string searchTerm, string expectedRange)
        {
            using BoundedUtf8Span boundedSourceTextSpan = new BoundedUtf8Span(sourceText);
            using BoundedUtf8Span boundedSearchTermSpan = new BoundedUtf8Span(searchTerm);

            Utf8Span sourceSpan = boundedSourceTextSpan.Span;
            Utf8Span searchSpan = boundedSearchTermSpan.Span;

            bool wasFound = sourceSpan.TryFindLast(searchSpan, out Range matchedRange);

            if (wasFound)
            {
                AssertRangesEqual(sourceSpan.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
            }
            else
            {
                Assert.Equal(default, matchedRange);
                Assert.Null(expectedRange);
            }

            // Also check Contains / StartsWith / EndsWith / SplitOn

            Assert.Equal(wasFound, sourceSpan.Contains(searchSpan));
            Assert.Equal(wasFound && sourceSpan.Bytes[matchedRange.End..].IsEmpty, sourceSpan.EndsWith(searchSpan));

            (var before, var after) = sourceSpan.SplitOnLast(searchSpan);
            if (wasFound)
            {
                Assert.True(sourceSpan.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
                Assert.True(sourceSpan.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(sourceSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
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
