// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public partial class Utf8SpanTests
    {
        [Theory]
        [MemberData(nameof(CaseConversionData))]
        public static void ToLower(string testData)
        {
            static void RunTest(string testData, string expected, CultureInfo culture)
            {
                using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(testData);
                Utf8Span inputSpan = boundedSpan.Span;

                // First try the allocating APIs

                ustring expectedUtf8 = u8(expected) ?? ustring.Empty;
                ustring actualUtf8;

                if (culture is null)
                {
                    actualUtf8 = inputSpan.ToLowerInvariant();
                }
                else
                {
                    actualUtf8 = inputSpan.ToLower(culture);
                }

                Assert.Equal(expectedUtf8, actualUtf8);

                // Next, try the non-allocating APIs with too small a buffer

                if (expectedUtf8.Length > 0)
                {
                    byte[] bufferTooSmall = new byte[expectedUtf8.Length - 1];

                    if (culture is null)
                    {
                        Assert.Equal(-1, inputSpan.ToLowerInvariant(bufferTooSmall));
                    }
                    else
                    {
                        Assert.Equal(-1, inputSpan.ToLower(bufferTooSmall, culture));
                    }
                }

                // Then the non-allocating APIs with a properly sized buffer

                foreach (int bufferSize in new[] { expectedUtf8.Length, expectedUtf8.Length + 1 })
                {
                    byte[] buffer = new byte[expectedUtf8.Length];

                    if (culture is null)
                    {
                        Assert.Equal(expectedUtf8.Length, inputSpan.ToLowerInvariant(buffer));
                    }
                    else
                    {
                        Assert.Equal(expectedUtf8.Length, inputSpan.ToLower(buffer, culture));
                    }

                    Assert.True(expectedUtf8.AsBytes().SequenceEqual(buffer));
                }
            }

            RunTest(testData, testData?.ToLowerInvariant(), null);
            RunTest(testData, testData?.ToLower(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            RunTest(testData, testData?.ToLower(CultureInfo.GetCultureInfo("en-US")), CultureInfo.GetCultureInfo("en-US"));
            RunTest(testData, testData?.ToLower(CultureInfo.GetCultureInfo("tr-TR")), CultureInfo.GetCultureInfo("tr-TR"));
        }

        [Theory]
        [MemberData(nameof(CaseConversionData))]
        public static void ToUpper(string testData)
        {
            static void RunTest(string testData, string expected, CultureInfo culture)
            {
                using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(testData);
                Utf8Span inputSpan = boundedSpan.Span;

                // First try the allocating APIs

                ustring expectedUtf8 = u8(expected) ?? ustring.Empty;
                ustring actualUtf8;

                if (culture is null)
                {
                    actualUtf8 = inputSpan.ToUpperInvariant();
                }
                else
                {
                    actualUtf8 = inputSpan.ToUpper(culture);
                }

                Assert.Equal(expectedUtf8, actualUtf8);

                // Next, try the non-allocating APIs with too small a buffer

                if (expectedUtf8.Length > 0)
                {
                    byte[] bufferTooSmall = new byte[expectedUtf8.Length - 1];

                    if (culture is null)
                    {
                        Assert.Equal(-1, inputSpan.ToUpperInvariant(bufferTooSmall));
                    }
                    else
                    {
                        Assert.Equal(-1, inputSpan.ToUpper(bufferTooSmall, culture));
                    }
                }

                // Then the non-allocating APIs with a properly sized buffer

                foreach (int bufferSize in new[] { expectedUtf8.Length, expectedUtf8.Length + 1 })
                {
                    byte[] buffer = new byte[expectedUtf8.Length];

                    if (culture is null)
                    {
                        Assert.Equal(expectedUtf8.Length, inputSpan.ToUpperInvariant(buffer));
                    }
                    else
                    {
                        Assert.Equal(expectedUtf8.Length, inputSpan.ToUpper(buffer, culture));
                    }

                    Assert.True(expectedUtf8.AsBytes().SequenceEqual(buffer));
                }
            }

            RunTest(testData, testData?.ToUpperInvariant(), null);
            RunTest(testData, testData?.ToUpper(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            RunTest(testData, testData?.ToUpper(CultureInfo.GetCultureInfo("en-US")), CultureInfo.GetCultureInfo("en-US"));
            RunTest(testData, testData?.ToUpper(CultureInfo.GetCultureInfo("tr-TR")), CultureInfo.GetCultureInfo("tr-TR"));
        }

        public static IEnumerable<object[]> CaseConversionData()
        {
            string[] testCases = new string[]
            {
                null,
                string.Empty,
                "Hello",
                "iı", // dotted and dotless I
                "İI", // dotted and dotless I
            };

            foreach (string testCase in testCases)
            {
                yield return new object[] { testCase };
            }
        }
    }
}
