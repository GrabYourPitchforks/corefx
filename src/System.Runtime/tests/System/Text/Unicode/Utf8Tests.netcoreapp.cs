// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Text.Unicode.Tests
{
    public partial class Utf8Tests
    {
        private const string X_UTF8 = "58"; // U+0058 LATIN CAPITAL LETTER X, 1 byte
        private const string X_UTF16 = "X";

        private const string Y_UTF8 = "59"; // U+0058 LATIN CAPITAL LETTER Y, 1 byte
        private const string Y_UTF16 = "Y";

        private const string Z_UTF8 = "5A"; // U+0058 LATIN CAPITAL LETTER Z, 1 byte
        private const string Z_UTF16 = "Z";

        private const string E_ACUTE_UTF8 = "C3A9"; // U+00E9 LATIN SMALL LETTER E WITH ACUTE, 2 bytes
        private const string E_ACUTE_UTF16 = "\u00E9";

        private const string EURO_SYMBOL_UTF8 = "E282AC"; // U+20AC EURO SIGN, 3 bytes
        private const string EURO_SYMBOL_UTF16 = "\u20AC";

        private const string GRINNING_FACE_UTF8 = "F09F9880"; // U+1F600 GRINNING FACE, 4 bytes
        private const string GRINNING_FACE_UTF16 = "\U0001F600";

        private static readonly UTF8Encoding _utf8EncodingWithoutReplacement = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        // All valid scalars [ U+0000 .. U+D7FF ] and [ U+E000 .. U+10FFFF ].
        private static readonly IEnumerable<Rune> _allValidRunes = Enumerable.Range(0x0000, 0xD800).Concat(Enumerable.Range(0xE000, 0x110000 - 0xE000)).Select(value => new Rune(value));

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
            return string.Create(_allValidRunes.Sum(rune => rune.Utf16SequenceLength), (object)null, (buffer, _) =>
            {
                foreach (var rune in _allValidRunes)
                {
                    Assert.True(rune.TryEncode(buffer, out int charsWritten));
                    buffer = buffer.Slice(charsWritten);
                }

                Assert.True(buffer.IsEmpty); // should've populated the entire buffer
            });
        }
    }
}
