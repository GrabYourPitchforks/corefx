// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit;

namespace System.Text.Unicode.Tests
{
    public partial class Utf8Tests
    {
        private static readonly UTF8Encoding _utf8EncodingWithoutReplacement = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        // All valid scalars [ U+0000 .. U+D7FF ] and [ U+E000 .. U+10FFFF ].
        private static readonly IEnumerable<int> _allValidScalars = Enumerable.Range(0x0000, 0xD800).Concat(Enumerable.Range(0xE000, 0x110000 - 0xE000));

        [Fact]
        public void IsWellFormedUtf8String_WithStringOfAllPossibleScalarValues_ReturnsTrue()
        {
            // Arrange

            byte[] allScalarsAsUtf8 = _utf8EncodingWithoutReplacement.GetBytes(_stringWithAllScalars.Value);
            using (var boundedMemory = BoundedMemory.AllocateFromExistingData(allScalarsAsUtf8))
            {
                boundedMemory.MakeReadonly();

                // Act & assert

                Assert.True(Utf8.IsWellFormed(boundedMemory.Span));
            }
        }

        [Fact]
        public void IsWellFormedUtf8String_WithCorruptedStringOfAllPossibleScalarValues_ReturnsFalse()
        {
            // Arrange

            byte[] allScalarsAsUtf8 = _utf8EncodingWithoutReplacement.GetBytes(_stringWithAllScalars.Value);
            allScalarsAsUtf8[0x1000] ^= 0x80; // modify the high bit of one of the characters, which will corrupt the header

            using (var boundedMemory = BoundedMemory.AllocateFromExistingData(allScalarsAsUtf8))
            {
                boundedMemory.MakeReadonly();

                // Act & assert

                Assert.False(Utf8.IsWellFormed(boundedMemory.Span));
            }
        }

        private static readonly Lazy<string> _stringWithAllScalars = new Lazy<string>(CreateStringWithAllScalars);

        private static string CreateStringWithAllScalars()
        {
            return string.Create(0xF800 /* number of BMP scalar values */ + 2 * 0x100000 /* number of astral scalar values */, (object)null, (buffer, _) =>
            {
                foreach (var scalarValue in _allValidScalars)
                {
                    var rune = new Rune(scalarValue);
                    Assert.True(rune.TryEncode(buffer, out int charsWritten));
                    buffer = buffer.Slice(charsWritten);
                }

                Assert.True(buffer.IsEmpty); // should've populated the entire buffer
            });
        }
    }
}
