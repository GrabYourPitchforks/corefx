// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Tests;
using Microsoft.DotNet.RemoteExecutor;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;
using System.Security.Policy;
using System.Buffers;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        [Theory]
        [MemberData(nameof(TryFindData_Char_Ordinal))]
        public static void TryFind_Char_Ordinal(ustring source, char searchTerm, Range? expectedForwardMatch, Range? expectedBackwardMatch)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(source.AsBytes());
            Utf8Span searchSpan = boundedSpan.Span;
            source = null; // to avoid accidentally using this for the remainder of the test

            // First, search forward

            bool wasFound = searchSpan.TryFind(searchTerm, out Range actualForwardMatch);
            Assert.Equal(expectedForwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedForwardMatch.Value, actualForwardMatch);
            }

            // Also check Contains / StartsWith / SplitOn

            Assert.Equal(wasFound, searchSpan.Contains(searchTerm));
            Assert.Equal(wasFound && searchSpan.Bytes[..actualForwardMatch.Start].IsEmpty, searchSpan.StartsWith(searchTerm));

            (var before, var after) = searchSpan.SplitOn(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualForwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualForwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }

            // Now search backward

            wasFound = searchSpan.TryFindLast(searchTerm, out Range actualBackwardMatch);
            Assert.Equal(expectedBackwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedBackwardMatch.Value, actualBackwardMatch);
            }

            // Also check EndsWith / SplitOnLast

            Assert.Equal(wasFound && searchSpan.Bytes[actualBackwardMatch.End..].IsEmpty, searchSpan.EndsWith(searchTerm));

            (before, after) = searchSpan.SplitOnLast(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualBackwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualBackwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }
        }

        [Theory]
        [MemberData(nameof(TryFindData_Rune_Ordinal))]
        public static void TryFind_Rune_Ordinal(ustring source, Rune searchTerm, Range? expectedForwardMatch, Range? expectedBackwardMatch)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(source.AsBytes());
            Utf8Span searchSpan = boundedSpan.Span;
            source = null; // to avoid accidentally using this for the remainder of the test

            // First, search forward

            bool wasFound = searchSpan.TryFind(searchTerm, out Range actualForwardMatch);
            Assert.Equal(expectedForwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedForwardMatch.Value, actualForwardMatch);
            }

            // Also check Contains / StartsWith / SplitOn

            Assert.Equal(wasFound, searchSpan.Contains(searchTerm));
            Assert.Equal(wasFound && searchSpan.Bytes[..actualForwardMatch.Start].IsEmpty, searchSpan.StartsWith(searchTerm));

            (var before, var after) = searchSpan.SplitOn(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualForwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualForwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }

            // Now search backward

            wasFound = searchSpan.TryFindLast(searchTerm, out Range actualBackwardMatch);
            Assert.Equal(expectedBackwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedBackwardMatch.Value, actualBackwardMatch);
            }

            // Also check EndsWith / SplitOnLast

            Assert.Equal(wasFound && searchSpan.Bytes[actualBackwardMatch.End..].IsEmpty, searchSpan.EndsWith(searchTerm));

            (before, after) = searchSpan.SplitOnLast(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualBackwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualBackwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }
        }

        [Theory]
        [MemberData(nameof(TryFindData_Utf8Span_Ordinal))]
        public static void TryFind_Utf8Span_Ordinal(ustring source, ustring searchTerm, Range? expectedForwardMatch, Range? expectedBackwardMatch)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(source.AsBytes());
            Utf8Span searchSpan = boundedSpan.Span;
            source = null; // to avoid accidentally using this for the remainder of the test

            // First, search forward

            bool wasFound = searchSpan.TryFind(searchTerm, out Range actualForwardMatch);
            Assert.Equal(expectedForwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedForwardMatch.Value, actualForwardMatch);
            }

            // Also check Contains / StartsWith / SplitOn

            Assert.Equal(wasFound, searchSpan.Contains(searchTerm));
            Assert.Equal(wasFound && searchSpan.Bytes[..actualForwardMatch.Start].IsEmpty, searchSpan.StartsWith(searchTerm));

            (var before, var after) = searchSpan.SplitOn(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualForwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualForwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }

            // Now search backward

            wasFound = searchSpan.TryFindLast(searchTerm, out Range actualBackwardMatch);
            Assert.Equal(expectedBackwardMatch.HasValue, wasFound);

            if (wasFound)
            {
                AssertRangesEqual(searchSpan.Bytes.Length, expectedBackwardMatch.Value, actualBackwardMatch);
            }

            // Also check EndsWith / SplitOnLast

            Assert.Equal(wasFound && searchSpan.Bytes[actualBackwardMatch.End..].IsEmpty, searchSpan.EndsWith(searchTerm));

            (before, after) = searchSpan.SplitOnLast(searchTerm);
            if (wasFound)
            {
                Assert.True(searchSpan.Bytes[..actualBackwardMatch.Start] == before.Bytes); // check for referential equality
                Assert.True(searchSpan.Bytes[actualBackwardMatch.End..] == after.Bytes); // check for referential equality
            }
            else
            {
                Assert.True(searchSpan.Bytes == before.Bytes); // check for reference equality
                Assert.True(after.IsNull());
            }
        }

        //        [Theory]
        //        [InlineData("", '\0', null)]
        //        [InlineData("Hello", 'l', "2..3")]
        //        [InlineData("x\ud83d\ude00y", '\ud83d', null)] // U+1F600 GRINNING FACE (surrogates should never match)
        //        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        //        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", '\0', null)]
        //        [InlineData("xyz\0xyz\0", '\0', "3..4")]
        //        public static void TryFind_Char(string sourceText, char searchChar, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
        //            Utf8Span span = boundedSpan.Span;

        //            bool wasFound = span.TryFind(searchChar, out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, span.Contains(searchChar));
        //            Assert.Equal(wasFound && span.Bytes[..matchedRange.Start].IsEmpty, span.StartsWith(searchChar));

        //            (var before, var after) = span.SplitOn(searchChar);
        //            if (wasFound)
        //            {
        //                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(span.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        //        [Theory]
        //        [InlineData("", '\0', null)]
        //        [InlineData("Hello", 'l', "2..3")]
        //        [InlineData("x\U0001F600y", 0x1F600, "1..5")] // U+1F600 GRINNING FACE
        //        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        //        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", '\0', null)]
        //        [InlineData("xyz\0xyz\0", '\0', "3..4")]
        //        public static void TryFind_Rune(string sourceText, uint searchRune, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
        //            Utf8Span span = boundedSpan.Span;

        //            bool wasFound = span.TryFind(new Rune(searchRune), out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, span.Contains(new Rune(searchRune)));
        //            Assert.Equal(wasFound && span.Bytes[..matchedRange.Start].IsEmpty, span.StartsWith(new Rune(searchRune)));

        //            (var before, var after) = span.SplitOn(new Rune(searchRune));
        //            if (wasFound)
        //            {
        //                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(span.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        //        [Theory]
        //        [InlineData("", "", "..0")] // empty search terms should always match at the front of the search space
        //        [InlineData("", "\0", null)]
        //        [InlineData("Hello", "", "..0")] // empty search terms should always match at the front of the search space
        //        [InlineData("Hello", "l", "2..3")]
        //        [InlineData("x\U0001F600y", "\U0001F600", "1..5")] // U+1F600 GRINNING FACE
        //        [InlineData("x\ud83d\ude00y", "\u00F0", null)] // make sure we don't confuse U+00F0 with the [ F0 ] byte
        //        [InlineData("x\ud83d\ude00y", "y", "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", "\u2660", "1..4")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", "\0", null)]
        //        [InlineData("xyz\0xyz\0", "\0", "3..4")]
        //        [InlineData("xyz\0xyz\0", "yz", "1..3")]
        //        public static void TryFind_Utf8Span(string sourceText, string searchTerm, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSourceTextSpan = new BoundedUtf8Span(sourceText);
        //            using BoundedUtf8Span boundedSearchTermSpan = new BoundedUtf8Span(searchTerm);

        //            Utf8Span sourceSpan = boundedSourceTextSpan.Span;
        //            Utf8Span searchSpan = boundedSearchTermSpan.Span;

        //            bool wasFound = sourceSpan.TryFind(searchSpan, out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(sourceSpan.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, sourceSpan.Contains(searchSpan));
        //            Assert.Equal(wasFound && sourceSpan.Bytes[..matchedRange.Start].IsEmpty, sourceSpan.StartsWith(searchSpan));

        //            (var before, var after) = sourceSpan.SplitOn(searchSpan);
        //            if (wasFound)
        //            {
        //                Assert.True(sourceSpan.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(sourceSpan.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(sourceSpan.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        //        [Theory]
        //        [InlineData("", '\0', null)]
        //        [InlineData("Hello", 'l', "3..4")]
        //        [InlineData("x\ud83d\ude00y", '\ud83d', null)] // U+1F600 GRINNING FACE (surrogates should never match)
        //        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        //        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", '\0', null)]
        //        [InlineData("xyz\0xyz\0", '\0', "^1..")]
        //        public static void TryFindLast_Char(string sourceText, char searchChar, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
        //            Utf8Span span = boundedSpan.Span;

        //            bool wasFound = span.TryFindLast(searchChar, out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, span.Contains(searchChar));
        //            Assert.Equal(wasFound && span.Bytes[matchedRange.End..].IsEmpty, span.EndsWith(searchChar));

        //            (var before, var after) = span.SplitOnLast(searchChar);
        //            if (wasFound)
        //            {
        //                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(span.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        //        [Theory]
        //        [InlineData("", '\0', null)]
        //        [InlineData("Hello", 'l', "3..4")]
        //        [InlineData("x\U0001F600y", 0x1F600, "1..5")] // U+1F600 GRINNING FACE
        //        [InlineData("x\ud83d\ude00y", 0xF0, null)] // don't match [ F0 ] UTF-8 byte, match only the char
        //        [InlineData("x\ud83d\ude00y", 'y', "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", '\u2660', "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", '\0', null)]
        //        [InlineData("xyz\0xyz\0", '\0', "^1..")]
        //        public static void TryFindLast_Rune(string sourceText, uint searchRune, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(sourceText);
        //            Utf8Span span = boundedSpan.Span;

        //            bool wasFound = span.TryFindLast(new Rune(searchRune), out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(span.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, span.Contains(new Rune(searchRune)));
        //            Assert.Equal(wasFound && span.Bytes[matchedRange.End..].IsEmpty, span.EndsWith(new Rune(searchRune)));

        //            (var before, var after) = span.SplitOnLast(new Rune(searchRune));
        //            if (wasFound)
        //            {
        //                Assert.True(span.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(span.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(span.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        //        [Theory]
        //        [InlineData("", "", "^0..")] // empty search terms should always match at the end of the search space
        //        [InlineData("", "\0", null)]
        //        [InlineData("Hello", "", "^0..")] // empty search terms should always match at the end of the search space
        //        [InlineData("Hello", "l", "3..4")]
        //        [InlineData("x\U0001F600y", "\U0001F600", "1..5")] // U+1F600 GRINNING FACE
        //        [InlineData("x\ud83d\ude00y", "\u00F0", null)] // make sure we don't confuse U+00F0 with the [ F0 ] byte
        //        [InlineData("x\ud83d\ude00y", "y", "^1..")]
        //        [InlineData("x\u2660\u2661\u2660\u2661y", "\u2660", "7..10")] // U+2660 BLACK SPADE SUIT & U+2661 WHITE HEART SUIT
        //        [InlineData("xyz", "\0", null)]
        //        [InlineData("xyz\0xyz\0", "\0", "^1..")]
        //        [InlineData("xyz\0xyz\0", "yz", "^3..^1")]
        //        public static void TryFindLast_Utf8Span(string sourceText, string searchTerm, string expectedRange)
        //        {
        //            using BoundedUtf8Span boundedSourceTextSpan = new BoundedUtf8Span(sourceText);
        //            using BoundedUtf8Span boundedSearchTermSpan = new BoundedUtf8Span(searchTerm);

        //            Utf8Span sourceSpan = boundedSourceTextSpan.Span;
        //            Utf8Span searchSpan = boundedSearchTermSpan.Span;

        //            bool wasFound = sourceSpan.TryFindLast(searchSpan, out Range matchedRange);

        //            if (wasFound)
        //            {
        //                AssertRangesEqual(sourceSpan.Bytes.Length, ParseRangeExpr(expectedRange), matchedRange);
        //            }
        //            else
        //            {
        //                Assert.Equal(default, matchedRange);
        //                Assert.Null(expectedRange);
        //            }

        //            // Also check Contains / StartsWith / EndsWith / SplitOn

        //            Assert.Equal(wasFound, sourceSpan.Contains(searchSpan));
        //            Assert.Equal(wasFound && sourceSpan.Bytes[matchedRange.End..].IsEmpty, sourceSpan.EndsWith(searchSpan));

        //            (var before, var after) = sourceSpan.SplitOnLast(searchSpan);
        //            if (wasFound)
        //            {
        //                Assert.True(sourceSpan.Bytes[..matchedRange.Start] == before.Bytes); // check for referential equality
        //                Assert.True(sourceSpan.Bytes[matchedRange.End..] == after.Bytes); // check for referential equality
        //            }
        //            else
        //            {
        //                Assert.True(sourceSpan.Bytes == before.Bytes); // check for reference equality
        //                Assert.True(after.IsNull());
        //            }
        //        }

        private static void AssertRangesEqual(int originalLength, Range expected, Range actual)
        {
            (int expectedOffset, int expectedLength) = expected.GetOffsetAndLength(originalLength);
            (int actualOffset, int actualLength) = actual.GetOffsetAndLength(originalLength);

            Assert.Equal(expectedOffset, actualOffset);
            Assert.Equal(expectedLength, actualLength);
        }

        //        [Theory]
        //        [MemberData(nameof(TryFindData_All))]
        //        public static void TryFind_Battery(TryFindTestData testData)
        //        {
        //            RemoteExecutor.Invoke((source, searchTerm, options, additionalCultures, expectedMatches) =>
        //            {
        //                // First, create the source as UTF-8.

        //                using BoundedUtf8Span boundedSource = new BoundedUtf8Span(source);
        //                Utf8Span utf8Source = boundedSource.Span;

        //                // The search term _might_ be well-formed UTF-16 data. We don't know that for
        //                // a fact, so we smuggle it across the remote executor boundary as hex-encoded chars
        //                // "AAAA;BBBB;CCCC;..." so that it doesn't get corrupted on the way back in.
        //                // We'll re-create it now.

        //                string searchString = new string(searchTerm.Split(',').Select(str => (char)int.Parse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToArray());
        //                ustring.TryCreateFrom(searchString, out ustring searchStringUtf8);

        //                if (searchStringUtf8 is object)
        //                {

        //                }




        //#error not implemented


        //                return RemoteExecutor.SuccessExitCode;
        //            },
        //            testData.Source?.ToString(),
        //            (testData.SearchTerm != null) ? string.Join(',', testData.SearchTerm.ToString().Select(ch => ((int)ch).ToString("X4", CultureInfo.InvariantCulture))) : string.Empty,
        //            ((int)testData.Options).ToString(CultureInfo.InvariantCulture),
        //            String.Join(',', testData.AdditionalCultures ?? Array.Empty<string>()),
        //            testData.ExpectedFirstMatch?.ToString() + ";" + testData.ExpectedLastMatch?.ToString()
        //            ).Dispose();
        //        }

        public static IEnumerable<object[]> TryFindData_Char_Ordinal()
        {
            foreach (TryFindTestData entry in TryFindData_All())
            {
                if (!entry.Options.HasFlag(TryFindTestDataOptions.TestOrdinal) || entry.Options.HasFlag(TryFindTestDataOptions.TestIgnoreCaseOnly))
                {
                    continue;
                }

                char searchChar = default;

                if (entry.SearchTerm is char ch)
                {
                    searchChar = ch;
                }
                else if (entry.SearchTerm is Rune r)
                {
                    if (!r.IsBmp) { continue; }
                    searchChar = (char)r.Value;
                }
                else if (entry.SearchTerm is string str)
                {
                    if (str.Length != 1) { continue; }
                    searchChar = str[0];
                }
                else if (entry.SearchTerm is ustring ustr)
                {
                    var enumerator = ustr.Chars.GetEnumerator();
                    if (!enumerator.MoveNext()) { continue; }
                    searchChar = enumerator.Current;
                    if (enumerator.MoveNext()) { continue; }
                }
                else
                {
                    continue;
                }

                yield return new object[]
                {
                    entry.Source,
                    searchChar,
                    entry.ExpectedFirstMatch,
                    entry.ExpectedLastMatch,
                };
            }
        }

        public static IEnumerable<object[]> TryFindData_Rune_Ordinal()
        {
            foreach (TryFindTestData entry in TryFindData_All())
            {
                if (!entry.Options.HasFlag(TryFindTestDataOptions.TestOrdinal) || entry.Options.HasFlag(TryFindTestDataOptions.TestIgnoreCaseOnly))
                {
                    continue;
                }

                Rune searchRune = default;

                if (entry.SearchTerm is char ch)
                {
                    if (!Rune.TryCreate(ch, out searchRune)) { continue; }
                }
                else if (entry.SearchTerm is Rune r)
                {
                    searchRune = r;
                }
                else if (entry.SearchTerm is string str)
                {
                    if (Rune.DecodeFromUtf16(str, out searchRune, out int charsConsumed) != OperationStatus.Done
                        || charsConsumed != str.Length)
                    {
                        continue;
                    }
                }
                else if (entry.SearchTerm is ustring ustr)
                {
                    if (Rune.DecodeFromUtf8(ustr.AsBytes(), out searchRune, out int bytesConsumed) != OperationStatus.Done
                        || bytesConsumed != ustr.GetByteLength())
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                yield return new object[]
                {
                    entry.Source,
                    searchRune,
                    entry.ExpectedFirstMatch,
                    entry.ExpectedLastMatch,
                };
            }
        }

        public static IEnumerable<object[]> TryFindData_Utf8Span_Ordinal()
        {
            foreach (TryFindTestData entry in TryFindData_All())
            {
                if (!entry.Options.HasFlag(TryFindTestDataOptions.TestOrdinal) || entry.Options.HasFlag(TryFindTestDataOptions.TestIgnoreCaseOnly))
                {
                    continue;
                }

                ustring searchTerm = default;

                if (entry.SearchTerm is char ch)
                {
                    if (!Rune.TryCreate(ch, out Rune rune)) { continue; }
                    searchTerm = rune.ToUtf8String();
                }
                else if (entry.SearchTerm is Rune r)
                {
                    searchTerm = r.ToUtf8String();
                }
                else if (entry.SearchTerm is string str)
                {
                    if (!ustring.TryCreateFrom(str, out searchTerm)) { continue; }
                }
                else if (entry.SearchTerm is ustring ustr)
                {
                    searchTerm = ustr;
                }
                else if (entry.SearchTerm is null)
                {
                    searchTerm = null;
                }
                else
                {
                    continue;
                }

                yield return new object[]
                {
                    entry.Source,
                    searchTerm,
                    entry.ExpectedFirstMatch,
                    entry.ExpectedLastMatch,
                };
            }
        }

        public static IEnumerable<TryFindTestData> TryFindData_All()
        {
            const string inv = "inv";
            const string en_US = "en-US";
            const string tr_TR = "tr-TR";
            const string hu_HU = "hu-HU";

            TryFindTestData[] testDataEntries = new TryFindTestData[]
            {
                new TryFindTestData
                {
                    // Searching for the empty string within the empty string should result in 0..0 / ^0..^0 across all comparers and all cultures
                    Source = null,
                    SearchTerm = null,
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv, en_US, tr_TR, hu_HU },
                    ExpectedFirstMatch = 0..0,
                    ExpectedLastMatch = ^0..^0,
                },
                new TryFindTestData
                {
                    // Searching for the empty string within a non-empty string should result in 0..0 / ^0..^0 across all comparers and all cultures
                    Source = u8("Hello"),
                    SearchTerm = null,
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv, en_US, tr_TR, hu_HU },
                    ExpectedFirstMatch = 0..0,
                    ExpectedLastMatch = ^0..^0,
                },
                new TryFindTestData
                {
                    // Searching for a non-empty string within an empty string should fail across all comparers and all cultures
                    Source = null,
                    SearchTerm = u8("Hello"),
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv, en_US, tr_TR, hu_HU },
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // Searching for the null terminator shouldn't match unless the input contains a null terminator
                    Source = u8("Hello"),
                    SearchTerm = '\0',
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = null,
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // Searching for the null terminator shouldn't match unless the input contains a null terminator
                    Source = u8("H\0ell\0o"),
                    SearchTerm = '\0',
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = null,
                    ExpectedFirstMatch = 1..2,
                    ExpectedLastMatch = ^2..^1,
                },
                new TryFindTestData
                {
                    // Simple ASCII search with success (case-sensitive)
                    Source = u8("Hello"),
                    SearchTerm = 'l',
                    Options = TryFindTestDataOptions.TestOrdinal | TryFindTestDataOptions.TestCaseSensitiveOnly,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = 2..3,
                    ExpectedLastMatch = 3..4,
                },
                new TryFindTestData
                {
                    // Simple ASCII search with failure (case-sensitive)
                    Source = u8("Hello"),
                    SearchTerm = 'L',
                    Options = TryFindTestDataOptions.TestOrdinal | TryFindTestDataOptions.TestCaseSensitiveOnly,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // Simple ASCII search with success (case-insensitive)
                    Source = u8("Hello"),
                    SearchTerm = 'L',
                    Options = TryFindTestDataOptions.TestOrdinal | TryFindTestDataOptions.TestIgnoreCaseOnly,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = 2..3,
                    ExpectedLastMatch = 3..4,
                },
                new TryFindTestData
                {
                    // U+1F600 GRINNING FACE, should match an exact Rune search
                    Source = u8("x\U0001F600y"),
                    SearchTerm = new Rune(0x1F600),
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = 1..5,
                    ExpectedLastMatch = 1..5,
                },
                new TryFindTestData
                {
                    // U+1F600 GRINNING FACE, shouldn't match looking for individual UTF-16 surrogate chars
                    Source = u8("x\ud83d\ude00y"),
                    SearchTerm = '\ud83d',
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // U+1F600 GRINNING FACE, shouldn't match on the standalone [ F0 ] byte that begins the multi-byte sequence
                    Source = u8("x\ud83d\ude00y"),
                    SearchTerm = '\u00f0',
                    Options = TryFindTestDataOptions.TestOrdinal,
                    AdditionalCultures = new string[] { inv },
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // hu_HU shouldn't match "d" within "dz"
                    Source = u8("ab_dz_ba"),
                    SearchTerm = 'd',
                    Options = TryFindTestDataOptions.None,
                    AdditionalCultures = new string[] { hu_HU },
                    ExpectedFirstMatch = null,
                    ExpectedLastMatch = null,
                },
                new TryFindTestData
                {
                    // Turkish I, case-sensitive
                    Source = u8("\u0069\u0130\u0131\u0049"), // iİıI
                    SearchTerm = 'i',
                    Options = TryFindTestDataOptions.TestCaseSensitiveOnly,
                    AdditionalCultures = new string[] { tr_TR },
                    ExpectedFirstMatch = 0..1,
                    ExpectedLastMatch = 0..1,
                },
                new TryFindTestData
                {
                    // Turkish I, case-insensitive
                    Source = u8("\u0069\u0130\u0131\u0049"), // iİıI
                    SearchTerm = 'i',
                    Options = TryFindTestDataOptions.TestIgnoreCaseOnly,
                    AdditionalCultures = new string[] { tr_TR },
                    ExpectedFirstMatch = 0..1,
                    ExpectedLastMatch = 1..3,
                },
            };

            foreach (TryFindTestData entry in testDataEntries)
            {
                // yield return new object[] { entry };
                yield return entry;
            }
        }

        public class TryFindTestData
        {
            public ustring Source;
            public object SearchTerm;
            public TryFindTestDataOptions Options;
            public string[] AdditionalCultures;
            public Range? ExpectedFirstMatch;
            public Range? ExpectedLastMatch;
        }

        [Flags]
        public enum TryFindTestDataOptions
        {
            None = 0,
            TestOrdinal = 1 << 0,
            TestCaseSensitiveOnly = 1 << 1,
            TestIgnoreCaseOnly = 2 << 1,
        }
    }
}
