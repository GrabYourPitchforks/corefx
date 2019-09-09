// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.Unicode;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public static partial class TR29UtilityTests
    {
        [Fact]
        public static void GraphemeClusterBreaks_NullAndEmptyInput()
        {
            Assert.Equal(0, TR29Utility.GetLengthOfFirstUtf16ExtendedGraphemeCluster(ReadOnlySpan<char>.Empty));
            Assert.Equal(0, TR29Utility.GetLengthOfFirstUtf16ExtendedGraphemeCluster(string.Empty));

            Assert.Equal(0, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(Utf8Span.Empty.Bytes));
            Assert.Equal(0, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(ustring.Empty.AsBytes()));
        }

        [Fact]
        public static void GraphemeClusterBreaks_SingleScalar()
        {
            Assert.Equal(1, TR29Utility.GetLengthOfFirstUtf16ExtendedGraphemeCluster("\u1234"));
            Assert.Equal(2, TR29Utility.GetLengthOfFirstUtf16ExtendedGraphemeCluster("\U00101234"));

            Assert.Equal(1, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(u8("\u0012").AsBytes()));
            Assert.Equal(2, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(u8("\u0123").AsBytes()));
            Assert.Equal(3, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(u8("\u1234").AsBytes()));
            Assert.Equal(4, TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(u8("\U00101234").AsBytes()));
        }

        [Theory]
        [MemberData(nameof(GraphemeClusterBreakTestData))]
        public static void GraphemeClusterBreaks_FromUnicodeTestData(string testData)
        {
            // breaks are denoted in the test data by U+00F7 DIVISION SIGN
            // non-breaking separations between scalars denoted in the test data by U+00D7 MULTIPLICATION SIGN

            IEnumerable<IEnumerable<Rune>> clusters = testData.Split('\u00F7', StringSplitOptions.RemoveEmptyEntries)
                .Select(cluster => cluster.Split('\u00D7', StringSplitOptions.RemoveEmptyEntries)
                    .Select(value => new Rune(uint.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture))));

            string combinedInput = string.Concat(clusters.Select(string.Concat<Rune>));

            // Test UTF-16

            {
                ReadOnlySpan<char> remainingInput = combinedInput;
                foreach (var cluster in clusters)
                {
                    int expectedCharsInThisCluster = cluster.Sum(rune => rune.Utf16SequenceLength);
                    int lengthToNextCluster = TR29Utility.GetLengthOfFirstUtf16ExtendedGraphemeCluster(remainingInput);
                    Assert.Equal(expectedCharsInThisCluster, lengthToNextCluster);
                    remainingInput = remainingInput.Slice(expectedCharsInThisCluster);
                }

                Assert.True(remainingInput.IsEmpty, "We didn't consume the entire test data.");
            }

            // Test UTF-8

            {
                ReadOnlySpan<byte> remainingInput = u8(combinedInput).AsBytes();

                foreach (var cluster in clusters)
                {
                    int expectedCharsInThisCluster = cluster.Sum(rune => rune.Utf8SequenceLength);
                    int lengthToNextCluster = TR29Utility.GetLengthOfFirstUtf8ExtendedGraphemeCluster(remainingInput);
                    Assert.Equal(expectedCharsInThisCluster, lengthToNextCluster);
                    remainingInput = remainingInput.Slice(expectedCharsInThisCluster);
                }

                Assert.True(remainingInput.IsEmpty, "We didn't consume the entire test data.");
            }
        }
    }
}
