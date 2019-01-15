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
