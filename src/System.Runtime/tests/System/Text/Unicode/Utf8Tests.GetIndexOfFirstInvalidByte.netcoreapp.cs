// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Text.Unicode.Tests
{
    public partial class Utf8Tests
    {
        private const string X = "58"; // U+0058 LATIN CAPITAL LETTER X, 1 byte
        private const string Y = "59"; // U+0058 LATIN CAPITAL LETTER Y, 1 byte
        private const string Z = "5A"; // U+0058 LATIN CAPITAL LETTER Z, 1 byte
        private const string E_ACUTE = "C3A9"; // U+00E9 LATIN SMALL LETTER E WITH ACUTE, 2 bytes
        private const string EURO_SYMBOL = "E282AC"; // U+20AC EURO SIGN, 3 bytes
        private const string GRINNING_FACE = "F09F9880"; // U+1F600 GRINNING FACE, 4 bytes

        [Theory]
        [InlineData("", 0, 0)] // empty string is OK
        [InlineData(X, 1, 1)]
        [InlineData(X + Y, 2, 2)]
        [InlineData(X + Y + Z, 3, 3)]
        [InlineData(E_ACUTE, 1, 1)]
        [InlineData(X + E_ACUTE, 2, 2)]
        [InlineData(E_ACUTE + X, 2, 2)]
        [InlineData(EURO_SYMBOL, 1, 1)]
        public void GetIndexOfFirstInvalidByte_WithSmallValidBuffers(string input, int expectedUtf16CharCount, int expectedScalarCount)
        {
            // These test cases are for the "slow processing" code path at the end of GetIndexOfFirstInvalidByte,
            // so inputs should be less than 4 bytes.

            Assert.InRange(input.Length, 0, 6);

            GetIndexOfFirstInvalidByte_Test_Core(input, -1 /* expectedRetVal */, expectedUtf16CharCount, expectedScalarCount);
        }

        [Theory]
        [InlineData("80", 0, 0, 0)] // sequence cannot begin with continuation character
        [InlineData("8182", 0, 0, 0)] // sequence cannot begin with continuation character
        [InlineData("838485", 0, 0, 0)] // sequence cannot begin with continuation character
        [InlineData(X + "80", 1, 1, 1)] // sequence cannot begin with continuation character
        [InlineData(X + "8182", 1, 1, 1)] // sequence cannot begin with continuation character
        [InlineData("C0", 0, 0, 0)] // [ C0 ] is always invalid
        [InlineData("C080", 0, 0, 0)] // [ C0 ] is always invalid
        [InlineData("C08081", 0, 0, 0)] // [ C0 ] is always invalid
        [InlineData(X + "C1", 1, 1, 1)] // [ C1 ] is always invalid
        [InlineData(X + "C180", 1, 1, 1)] // [ C1 ] is always invalid
        [InlineData("C2", 0, 0, 0)] // [ C2 ] is improperly terminated
        [InlineData(X + "C27F", 1, 1, 1)] // [ C2 ] is improperly terminated
        [InlineData(X + "E282", 1, 1, 1)] // [ E2 82 ] is improperly terminated
        [InlineData("E2827F", 0, 0, 0)] // [ E2 82 ] is improperly terminated
        [InlineData("E09F80", 0, 0, 0)] // [ E0 9F ... ] is overlong
        [InlineData("E0C080", 0, 0, 0)] // [ E0 ] is improperly terminated
        [InlineData("ED7F80", 0, 0, 0)] // [ ED ] is improperly terminated
        [InlineData("EDA080", 0, 0, 0)] // [ ED A0 ... ] is surrogate
        public void GetIndexOfFirstInvalidByte_WithSmallInvalidBuffers(string input, int expectedRetVal, int expectedUtf16CharCount, int expectedScalarCount)
        {
            // These test cases are for the "slow processing" code path at the end of GetIndexOfFirstInvalidByte,
            // so inputs should be less than 4 bytes.

            Assert.InRange(input.Length, 0, 6);

            GetIndexOfFirstInvalidByte_Test_Core(input, expectedRetVal, expectedUtf16CharCount, expectedScalarCount);
        }

        [Theory]
        [InlineData(E_ACUTE + "21222324" + "303132333435363738393A3B3C3D3E3F", 21, 21)] // Loop unrolling at end of buffer
        [InlineData(E_ACUTE + "21222324" + "303132333435363738393A3B3C3D3E3F" + "3031323334353637" + E_ACUTE + "38393A3B3C3D3E3F", 38, 38)] // Loop unrolling interrupted by non-ASCII
        [InlineData("212223" + E_ACUTE + "30313233", 8, 8)] // 3 ASCII bytes followed by non-ASCII
        [InlineData("2122" + E_ACUTE + "30313233", 7, 7)] // 2 ASCII bytes followed by non-ASCII
        [InlineData("21" + E_ACUTE + "30313233", 6, 6)] // 1 ASCII byte followed by non-ASCII
        [InlineData(E_ACUTE + E_ACUTE + E_ACUTE + E_ACUTE, 4, 4)] // 4x 2-byte sequences, exercises optimization code path in 2-byte sequence processing
        [InlineData(E_ACUTE + E_ACUTE + E_ACUTE + "5051", 5, 5)] // 3x 2-byte sequences + 2 ASCII bytes, exercises optimization code path in 2-byte sequence processing
        [InlineData(E_ACUTE + "5051", 3, 3)] // single 2-byte sequence + 2 trailing ASCII bytes, exercises draining logic in 2-byte sequence processing
        [InlineData(E_ACUTE + "50" + E_ACUTE + "304050", 6, 6)] // single 2-byte sequences + 1 trailing ASCII byte + 2-byte sequence, exercises draining logic in 2-byte sequence processing
        [InlineData(EURO_SYMBOL + "20", 2, 2)] // single 3-byte sequence + 1 trailing ASCII byte, exercises draining logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL + "203040", 4, 4)] // single 3-byte sequence + 3 trailing ASCII byte, exercises draining logic and "running out of data" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL + EURO_SYMBOL + EURO_SYMBOL, 3, 3)] // 3x 3-byte sequences, exercises "stay within 3-byte loop" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL + EURO_SYMBOL + EURO_SYMBOL + EURO_SYMBOL, 4, 4)] // 4x 3-byte sequences, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL + EURO_SYMBOL + EURO_SYMBOL + E_ACUTE, 4, 4)] // 3x 3-byte sequences + single 2-byte sequence, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL + EURO_SYMBOL + E_ACUTE + E_ACUTE + E_ACUTE + E_ACUTE, 6, 6)] // 2x 3-byte sequences + 4x 2-byte sequences, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(GRINNING_FACE + GRINNING_FACE, 4, 2)] // 2x 4-byte sequences, exercises 4-byte sequence processing
        [InlineData(GRINNING_FACE + "303132", 5, 4)] // single 4-byte sequence + 3 ASCII bytes, exercises 4-byte sequence processing and draining logic
        [InlineData("F09FA4B8" + "F09F8FBD" + "E2808D" + "E29980" + "EFB88F", 7, 5)] // U+1F938 U+1F3FD U+200D U+2640 U+FE0F WOMAN CARTWHEELING: MEDIUM SKIN TONE, exercising switching between multiple sequence lengths
        public void GetIndexOfFirstInvalidByte_WithLargeValidBuffers(string input, int expectedUtf16CharCount, int expectedScalarCount)
        {
            // These test cases are for the "fast processing" code which is the main loop of GetIndexOfFirstInvalidByte,
            // so inputs should be less >= 4 bytes.

            Assert.True(input.Length >= 8);

            GetIndexOfFirstInvalidByte_Test_Core(input, -1 /* expectedRetVal */, expectedUtf16CharCount, expectedScalarCount);
        }

        [Theory]
        [InlineData("3031" + "80" + "202122232425", 2, 2, 2)] // Continuation character at start of sequence should match no bitmask
        [InlineData("3031" + "C080" + "2021222324", 2, 2, 2)] // Overlong 2-byte sequence at start of DWORD
        [InlineData("3031" + "C180" + "2021222324", 2, 2, 2)] // Overlong 2-byte sequence at start of DWORD
        [InlineData("C280" + "C180", 2, 1, 1)] // Overlong 2-byte sequence at end of DWORD
        [InlineData("C27F" + "C280", 0, 0, 0)] // Improperly terminated 2-byte sequence at start of DWORD
        [InlineData("C2C0" + "C280", 0, 0, 0)] // Improperly terminated 2-byte sequence at start of DWORD
        [InlineData("C280" + "C27F", 2, 1, 1)] // Improperly terminated 2-byte sequence at end of DWORD
        [InlineData("C280" + "C2C0", 2, 1, 1)] // Improperly terminated 2-byte sequence at end of DWORD
        [InlineData("C280" + "C280" + "80203040", 4, 2, 2)] // Continuation character at start of sequence, within "stay in 2-byte processing" optimization
        [InlineData("C280" + "C280" + "C180" + "C280", 4, 2, 2)] // Overlong 2-byte sequence at start of DWORD, within "stay in 2-byte processing" optimization
        [InlineData("C280" + "C280" + "C280" + "C180", 6, 3, 3)] // Overlong 2-byte sequence at end of DWORD, within "stay in 2-byte processing" optimization
        [InlineData("3031" + "E09F80" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Overlong 3-byte sequence at start of DWORD
        [InlineData("3031" + "E07F80" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Improperly terminated 3-byte sequence at start of DWORD
        [InlineData("3031" + "E0C080" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Improperly terminated 3-byte sequence at start of DWORD
        [InlineData("3031" + "E17F80" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Improperly terminated 3-byte sequence at start of DWORD
        [InlineData("3031" + "E1C080" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Improperly terminated 3-byte sequence at start of DWORD
        [InlineData("3031" + "EDA080" + EURO_SYMBOL + EURO_SYMBOL, 2, 2, 2)] // Surrogate 3-byte sequence at start of DWORD
        [InlineData("3031" + "F5808080", 2, 2, 2)] // [ F5 ] is always invalid
        [InlineData("3031" + "F6808080", 2, 2, 2)] // [ F6 ] is always invalid
        [InlineData("3031" + "F7808080", 2, 2, 2)] // [ F7 ] is always invalid
        [InlineData("3031" + "F8808080", 2, 2, 2)] // [ F8 ] is always invalid
        [InlineData("3031" + "F9808080", 2, 2, 2)] // [ F9 ] is always invalid
        [InlineData("3031" + "FA808080", 2, 2, 2)] // [ FA ] is always invalid
        [InlineData("3031" + "FB808080", 2, 2, 2)] // [ FB ] is always invalid
        [InlineData("3031" + "FC808080", 2, 2, 2)] // [ FC ] is always invalid
        [InlineData("3031" + "FD808080", 2, 2, 2)] // [ FD ] is always invalid
        [InlineData("3031" + "FE808080", 2, 2, 2)] // [ FE ] is always invalid
        [InlineData("3031" + "FF808080", 2, 2, 2)] // [ FF ] is always invalid
        public void GetIndexOfFirstInvalidByte_WithLargeInvalidBuffers(string input, int expectedRetVal, int expectedUtf16CharCount, int expectedScalarCount)
        {
            // These test cases are for the "fast processing" code which is the main loop of GetIndexOfFirstInvalidByte,
            // so inputs should be less >= 4 bytes.

            Assert.True(input.Length >= 8);

            GetIndexOfFirstInvalidByte_Test_Core(input, expectedRetVal, expectedUtf16CharCount, expectedScalarCount);
        }

        private static byte[] DecodeHex(ReadOnlySpan<char> inputHex)
        {
            Assert.True(inputHex.Length % 2 == 0, "Input must be an even number of characters.");

            byte[] retVal = new byte[inputHex.Length / 2];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = byte.Parse(inputHex.Slice(i * 2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }
            return retVal;
        }

        private static void GetIndexOfFirstInvalidByte_Test_Core(string inputHex, int expectedRetVal, int expectedUtf16CharCount, int expectedScalarCount)
          => GetIndexOfFirstInvalidByte_Test_Core(DecodeHex(inputHex), expectedRetVal, expectedUtf16CharCount, expectedScalarCount);

        private static void GetIndexOfFirstInvalidByte_Test_Core(byte[] input, int expectedRetVal, int expectedUtf16CharCount, int expectedScalarCount)
        {
            // Arrange

            using (var boundedMemory = BoundedMemory.AllocateFromExistingData(input))
            {
                boundedMemory.MakeReadonly();

                // Act

                var indexOfFirstInvalidByte = Utf8.GetIndexOfFirstInvalidByte(boundedMemory.Span, out int actualUtf16Count, out int actualScalarCount);

                // Assert

                Assert.Equal(expectedRetVal, indexOfFirstInvalidByte);
                Assert.Equal(expectedUtf16CharCount, actualUtf16Count);
                Assert.Equal(expectedScalarCount, actualScalarCount);
            }
        }
    }
}
