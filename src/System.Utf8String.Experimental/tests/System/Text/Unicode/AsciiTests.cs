// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using Xunit;

using static System.Tests.Utf8TestUtilities;

namespace System.Text.Tests
{
    public static class AsciiTests
    {
        private delegate void RunTestAction(ReadOnlySpan<byte> input);

        [Theory]
        [InlineData("résumé")]
        [InlineData("\u0080")]
        [InlineData("\u00ff")]
        public static void IsAllAscii_NegativeTests(string input)
        {
            RunTest(input, input =>
            {
                Assert.False(Ascii.IsAllAscii(input));
            });
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData("Hello there!")]
        [InlineData("\0")]
        [InlineData("\u007f")]
        public static void IsAllAscii_PositiveTests(string input)
        {
            RunTest(input, input =>
            {
                Assert.True(Ascii.IsAllAscii(input));
            });
        }

        [Theory]
        [InlineData((string)null, "..")]
        [InlineData("", "..")]
        [InlineData("\0", "..")]
        [InlineData("\0Hello\0", "..")]
        [InlineData("\rHello\n", "1..^1")]
        [InlineData("Hello\r\nthere!", "..")]
        [InlineData("\r\n", "^0..")]
        [InlineData("\t\n\v\f\rHello!\u007f \t\n\v\f\r", "5..^6")] // U+007F isn't ASCII whitespace
        [InlineData("\t\n\u00A0\v\f\r", "2..^3")] // U+00A0 isn't ASCII whitespace
        public static void Trim_Tests(string input, string expectedRange)
        {
            // First run the test for ROS<byte>.

            RunTest(input, input =>
            {
                // First check Trim.

                Range trimmedRange = ParseRangeExpr(expectedRange);
                Assert.True(Ascii.Trim(input) == input[trimmedRange]); // referential equality

                // Now also check TrimStart and TrimEnd.
                // Empty ranges need to be special-cased because TrimStart and TrimEnd
                // could result in different references.

                if (input[trimmedRange].IsEmpty)
                {
                    Assert.True(Ascii.TrimStart(input) == input[^0..]); // referential equality
                    Assert.True(Ascii.TrimEnd(input) == input[..0]); // referential equality
                }
                else
                {
                    Assert.True(Ascii.TrimStart(input) == input[trimmedRange.Start..]); // referential equality
                    Assert.True(Ascii.TrimEnd(input) == input[..trimmedRange.End]); // referential equality
                }
            });

            // Then run the test for Span<byte>.

            RunTest(input, readOnlyInput =>
            {
                unsafe
                {
                    fixed (byte* pBytes = &MemoryMarshal.GetReference(readOnlyInput))
                    {
                        // It's ok to create a Span<byte> from a ROS<byte> since we're not
                        // going to mutate it.

                        Span<byte> input = new Span<byte>(pBytes, readOnlyInput.Length);

                        // First check Trim.

                        Range trimmedRange = ParseRangeExpr(expectedRange);
                        Assert.True(Ascii.Trim(input) == input[trimmedRange]); // referential equality

                        // Now also check TrimStart and TrimEnd.
                        // Empty ranges need to be special-cased because TrimStart and TrimEnd
                        // could result in different references.

                        if (input[trimmedRange].IsEmpty)
                        {
                            Assert.True(Ascii.TrimStart(input) == input[^0..]); // referential equality
                            Assert.True(Ascii.TrimEnd(input) == input[..0]); // referential equality
                        }
                        else
                        {
                            Assert.True(Ascii.TrimStart(input) == input[trimmedRange.Start..]); // referential equality
                            Assert.True(Ascii.TrimEnd(input) == input[..trimmedRange.End]); // referential equality
                        }
                    }
                }
            });
        }

        private static void RunTest(string input, RunTestAction callback)
        {
            // Special-case null inputs.

            if (input is null)
            {
                callback(ReadOnlySpan<byte>.Empty);
                return;
            }

            // Convert input to Latin-1.

            byte[] inputAsBytes = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                uint thisChar = input[i];
                Assert.True(thisChar <= byte.MaxValue, "Input string wasn't Latin-1.");
                inputAsBytes[i] = (byte)thisChar;
            }

            const int POISON_SLIDING_BUFFER = 32; // number of offset positions to test

            // Place poisoned pages before and after the data, then run the test for varying offsets.
            // The callback will be called several times: once with the data at offset 0, once with the
            // data at offset 1, and so on. This helps check that any unsafe code in our text processing
            // methods don't overrun their buffers or mutate read-only buffers.

            using (BoundedMemory<byte> boundedMemory = BoundedMemory.Allocate<byte>(input.Length + POISON_SLIDING_BUFFER, PoisonPagePlacement.Before))
            {
                Span<byte> boundedSpan = boundedMemory.Span;

                for (int i = POISON_SLIDING_BUFFER; i >= 0; i--)
                {
                    Span<byte> thisSpan = boundedSpan.Slice(i, inputAsBytes.Length);

                    boundedMemory.MakeWriteable();
                    inputAsBytes.AsSpan().CopyTo(thisSpan);
                    boundedMemory.MakeReadonly();

                    callback(thisSpan);
                }
            }

            using (BoundedMemory<byte> boundedMemory = BoundedMemory.Allocate<byte>(input.Length + POISON_SLIDING_BUFFER, PoisonPagePlacement.After))
            {
                Span<byte> boundedSpan = boundedMemory.Span;

                for (int i = 0; i <= POISON_SLIDING_BUFFER; i++)
                {
                    Span<byte> thisSpan = boundedSpan.Slice(i, inputAsBytes.Length);

                    boundedMemory.MakeWriteable();
                    inputAsBytes.AsSpan().CopyTo(thisSpan);
                    boundedMemory.MakeReadonly();

                    callback(thisSpan);
                }
            }
        }
    }
}
