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
        [Theory]
        [InlineData("", "")] // empty string is OK
        [InlineData(X_UTF8, X_UTF16)]
        [InlineData(X_UTF8 + Y_UTF8, X_UTF16 + Y_UTF16)]
        [InlineData(X_UTF8 + Y_UTF8 + Z_UTF8, X_UTF16 + Y_UTF16 + Z_UTF16)]
        [InlineData(E_ACUTE_UTF8, E_ACUTE_UTF16)]
        [InlineData(X_UTF8 + E_ACUTE_UTF8, X_UTF16 + E_ACUTE_UTF16)]
        [InlineData(E_ACUTE_UTF8 + X_UTF8, E_ACUTE_UTF16 + X_UTF16)]
        [InlineData(EURO_SYMBOL_UTF8, EURO_SYMBOL_UTF16)]
        public void ToChars_WithSmallValidBuffers(string utf8HexInput, string expectedUtf16Transcoding)
        {
            // These test cases are for the "slow processing" code path at the end of TranscodeToUtf16,
            // so inputs should be less than 4 bytes.

            Assert.InRange(utf8HexInput.Length, 0, 6);

            ToChars_Test_Core(
                utf8Input: DecodeHex(utf8HexInput),
                destinationSize: expectedUtf16Transcoding.Length,
                replaceInvalidSequences: false,
                isFinalChunk: false,
                expectedOperationStatus: OperationStatus.Done,
                expectedNumBytesRead: utf8HexInput.Length / 2,
                expectedUtf16Transcoding: expectedUtf16Transcoding);
        }

        [Theory]
        [InlineData("80", 0, "")] // sequence cannot begin with continuation character
        [InlineData("8182", 0, "")] // sequence cannot begin with continuation character
        [InlineData("838485", 0, "")] // sequence cannot begin with continuation character
        [InlineData(X_UTF8 + "80", 1, X_UTF16)] // sequence cannot begin with continuation character
        [InlineData(X_UTF8 + "8182", 1, X_UTF16)] // sequence cannot begin with continuation character
        [InlineData("C0", 0, "")] // [ C0 ] is always invalid
        [InlineData("C080", 0, "")] // [ C0 ] is always invalid
        [InlineData("C08081", 0, "")] // [ C0 ] is always invalid
        [InlineData(X_UTF8 + "C1", 1, X_UTF16)] // [ C1 ] is always invalid
        [InlineData(X_UTF8 + "C180", 1, X_UTF16)] // [ C1 ] is always invalid
        [InlineData("C2", 0, "")] // [ C2 ] is improperly terminated
        [InlineData(X_UTF8 + "C27F", 1, X_UTF16)] // [ C2 ] is improperly terminated
        [InlineData(X_UTF8 + "E282", 1, X_UTF16)] // [ E2 82 ] is improperly terminated
        [InlineData("E2827F", 0, "")] // [ E2 82 ] is improperly terminated
        [InlineData("E09F80", 0, "")] // [ E0 9F ... ] is overlong
        [InlineData("E0C080", 0, "")] // [ E0 ] is improperly terminated
        [InlineData("ED7F80", 0, "")] // [ ED ] is improperly terminated
        [InlineData("EDA080", 0, "")] // [ ED A0 ... ] is surrogate
        public void ToChars_WithSmallInvalidBuffers(string utf8HexInput, int expectedNumBytesConsumed, string expectedUtf16Transcoding)
        {
            // These test cases are for the "slow processing" code path at the end of TranscodeToUtf16,
            // so inputs should be less than 4 bytes.

            Assert.InRange(utf8HexInput.Length, 0, 6);

            ToChars_Test_Core(
              utf8Input: DecodeHex(utf8HexInput),
              destinationSize: expectedUtf16Transcoding.Length,
              replaceInvalidSequences: false,
              isFinalChunk: false,
              expectedOperationStatus: OperationStatus.InvalidData,
              expectedNumBytesRead: expectedNumBytesConsumed,
              expectedUtf16Transcoding: expectedUtf16Transcoding);
        }

        [Theory]
        [InlineData("C2", 0, "")] // [ C2 ] is an incomplete sequence
        [InlineData(X_UTF8 + "C2", 1, X_UTF16)] // [ C2 ] is an incomplete sequence
        [InlineData(X_UTF8 + "E0", 1, X_UTF16)] // [ E0 ] is an incomplete sequence
        [InlineData(X_UTF8 + "E0BF", 1, X_UTF16)] // [ E0 BF ] is an incomplete sequence
        [InlineData(X_UTF8 + "F0", 1, X_UTF16)] // [ F0 ] is an incomplete sequence
        [InlineData(X_UTF8 + "F0BF", 1, X_UTF16)] // [ F0 BF ] is an incomplete sequence
        [InlineData(X_UTF8 + "F0BFA0", 1, X_UTF16)] // [ F0 BF A0 ] is an incomplete sequence
        [InlineData(E_ACUTE_UTF8 + "C2", 2, E_ACUTE_UTF16)] // [ C2 ] is an incomplete sequence
        [InlineData(E_ACUTE_UTF8 + "E0", 2, E_ACUTE_UTF16)] // [ E0 ] is an incomplete sequence
        [InlineData(E_ACUTE_UTF8 + "E0BF", 2, E_ACUTE_UTF16)] // [ E0 BF ] is an incomplete sequence
        [InlineData(E_ACUTE_UTF8 + "F0", 2, E_ACUTE_UTF16)] // [ F0 ] is an incomplete sequence
        [InlineData(E_ACUTE_UTF8 + "F0BF", 2, E_ACUTE_UTF16)] // [ F0 BF ] is an incomplete sequence
        [InlineData(EURO_SYMBOL_UTF8 + "C2", 3, EURO_SYMBOL_UTF16)] // [ C2 ] is an incomplete sequence
        [InlineData(EURO_SYMBOL_UTF8 + "E0", 3, EURO_SYMBOL_UTF16)] // [ E0 ] is an incomplete sequence
        [InlineData(EURO_SYMBOL_UTF8 + "F0", 3, EURO_SYMBOL_UTF16)] // [ F0 ] is an incomplete sequence
        public void ToChars_WithSmallIncompleteBuffers(string utf8HexInput, int expectedNumBytesConsumed, string expectedUtf16Transcoding)
        {
            // These test cases are for the "slow processing" code path at the end of TranscodeToUtf16,
            // so inputs should be less than 4 bytes.

            Assert.InRange(utf8HexInput.Length, 0, 6);

            ToChars_Test_Core(
              utf8Input: DecodeHex(utf8HexInput),
              destinationSize: expectedUtf16Transcoding.Length,
              replaceInvalidSequences: false,
              isFinalChunk: false,
              expectedOperationStatus: OperationStatus.NeedMoreData,
              expectedNumBytesRead: expectedNumBytesConsumed,
              expectedUtf16Transcoding: expectedUtf16Transcoding);
        }

        [Theory]
        [InlineData(E_ACUTE_UTF8 + "41424344" + "303132333435363738393A3B3C3D3E3F", E_ACUTE_UTF16 + "ABCD" + "0123456789:;<=>?")] // Loop unrolling at end of buffer
        [InlineData(E_ACUTE_UTF8 + "41424344" + "303132333435363738393A3B3C3D3E3F" + "3031323334353637" + E_ACUTE_UTF8 + "38393A3B3C3D3E3F", E_ACUTE_UTF16 + "ABCD" + "0123456789:;<=>?" + "01234567" + E_ACUTE_UTF16 + "89:;<=>?")] // Loop unrolling interrupted by non-ASCII
        [InlineData("414243" + E_ACUTE_UTF8 + "30313233", "ABC" + E_ACUTE_UTF16 + "0123")] // 3 ASCII bytes followed by non-ASCII
        [InlineData("4142" + E_ACUTE_UTF8 + "30313233", "AB" + E_ACUTE_UTF16 + "0123")] // 2 ASCII bytes followed by non-ASCII
        [InlineData("41" + E_ACUTE_UTF8 + "30313233", "A" + E_ACUTE_UTF16 + "0123")] // 1 ASCII byte followed by non-ASCII
        [InlineData(E_ACUTE_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8, E_ACUTE_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16)] // 4x 2-byte sequences, exercises optimization code path in 2-byte sequence processing
        [InlineData(E_ACUTE_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8 + "5051", E_ACUTE_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16 + "PQ")] // 3x 2-byte sequences + 2 ASCII bytes, exercises optimization code path in 2-byte sequence processing
        [InlineData(E_ACUTE_UTF8 + "5051", E_ACUTE_UTF16 + "PQ")] // single 2-byte sequence + 2 trailing ASCII bytes, exercises draining logic in 2-byte sequence processing
        [InlineData(E_ACUTE_UTF8 + "50" + E_ACUTE_UTF8 + "304050", E_ACUTE_UTF16 + "P" + E_ACUTE_UTF16 + "0@P")] // single 2-byte sequences + 1 trailing ASCII byte + 2-byte sequence, exercises draining logic in 2-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + "40", EURO_SYMBOL_UTF16 + "@")] // single 3-byte sequence + 1 trailing ASCII byte, exercises draining logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + "405060", EURO_SYMBOL_UTF16 + "@P`")] // single 3-byte sequence + 3 trailing ASCII byte, exercises draining logic and "running out of data" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8, EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16)] // 3x 3-byte sequences, exercises "stay within 3-byte loop" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8, EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16)] // 4x 3-byte sequences, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + E_ACUTE_UTF8, EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + E_ACUTE_UTF16)] // 3x 3-byte sequences + single 2-byte sequence, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(EURO_SYMBOL_UTF8 + EURO_SYMBOL_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8 + E_ACUTE_UTF8, EURO_SYMBOL_UTF16 + EURO_SYMBOL_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16 + E_ACUTE_UTF16)] // 2x 3-byte sequences + 4x 2-byte sequences, exercises "consume multiple bytes at a time" logic in 3-byte sequence processing
        [InlineData(GRINNING_FACE_UTF8 + GRINNING_FACE_UTF8, GRINNING_FACE_UTF16 + GRINNING_FACE_UTF16)] // 2x 4-byte sequences, exercises 4-byte sequence processing
        [InlineData(GRINNING_FACE_UTF8 + "404142", GRINNING_FACE_UTF16 + "@AB")] // single 4-byte sequence + 3 ASCII bytes, exercises 4-byte sequence processing and draining logic
        [InlineData("F09FA4B8" + "F09F8FBD" + "E2808D" + "E29980" + "EFB88F", "\U0001F938\U0001F3FD\u200D\u2640\uFE0F")] // U+1F938 U+1F3FD U+200D U+2640 U+FE0F WOMAN CARTWHEELING: MEDIUM SKIN TONE, exercising switching between multiple sequence lengths
        public void ToChars_WithLargeValidBuffers(string utf8HexInput, string expectedUtf16Transcoding)
        {
            // These test cases are for the "fast processing" code which is the main loop of TranscodeToUtf16,
            // so inputs should be less >= 4 bytes.

            Assert.True(utf8HexInput.Length >= 8);

            ToChars_Test_Core(
                utf8Input: DecodeHex(utf8HexInput),
                destinationSize: expectedUtf16Transcoding.Length,
                replaceInvalidSequences: false,
                isFinalChunk: false,
                expectedOperationStatus: OperationStatus.Done,
                expectedNumBytesRead: utf8HexInput.Length / 2,
                expectedUtf16Transcoding: expectedUtf16Transcoding);
        }

        private static void ToChars_Test_Core(byte[] utf8Input, int destinationSize, bool replaceInvalidSequences, bool isFinalChunk, OperationStatus expectedOperationStatus, int expectedNumBytesRead, string expectedUtf16Transcoding)
        {
            // Arrange

            using (var boundedSource = BoundedMemory.AllocateFromExistingData(utf8Input))
            using (var boundedDestination = BoundedMemory.Allocate<char>(destinationSize))
            {
                boundedSource.MakeReadonly();

                // Act

                var actualOperationStatus = Utf8.ToChars(boundedSource.Span, boundedDestination.Span, replaceInvalidSequences, isFinalChunk, out int actualNumBytesRead, out int actualNumCharsWritten);

                // Assert

                Assert.Equal(expectedOperationStatus, actualOperationStatus);
                Assert.Equal(expectedNumBytesRead, actualNumBytesRead);
                Assert.Equal(expectedUtf16Transcoding.Length, actualNumCharsWritten);
                Assert.Equal(boundedDestination.Span.Slice(0, actualNumCharsWritten).ToString(), expectedUtf16Transcoding);
            }
        }
    }
}
