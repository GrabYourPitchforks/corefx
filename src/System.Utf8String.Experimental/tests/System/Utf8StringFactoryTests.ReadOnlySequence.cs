// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Linq;
using Xunit;

using ustring = System.Utf8String;

namespace System.Tests
{
    public partial class Utf8StringFactoryTests
    {
        [Fact]
        public static void CreateFromReadOnlySequence_LargeInput_ThrowsOutOfMemoryException()
        {
            RunTest(Utf8StringFactory.Create);
            RunTest(Utf8StringFactory.CreateRelaxed);
            RunTest(Utf8StringFactory.UnsafeCreateWithoutValidation);

            static void RunTest(Func<ReadOnlySequence<byte>, ustring> factory)
            {
                // Generate a huge ReadOnlySequence<T> consisting of 4 GB of data.
                // We'll only allocate a single 1 MB buffer for this.

                byte[] bigBuffer = new byte[1024 * 1024];
                MockReadOnlySequenceSegment<byte>[] segments = new MockReadOnlySequenceSegment<byte>[4096];

                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i] = new MockReadOnlySequenceSegment<byte>
                    {
                        Memory = bigBuffer,
                        RunningIndex = (long)i * bigBuffer.Length
                    };
                }

                for (int i = segments.Length - 2; i >= 0; i--)
                {
                    segments[i].Next = segments[i + 1];
                }

                ReadOnlySequence<byte> bigSequence = new ReadOnlySequence<byte>(segments.First(), 0, segments.Last(), bigBuffer.Length);
                Assert.Equal((long)4096 * 1024 * 1024, bigSequence.Length);

                Assert.Throws<OutOfMemoryException>(() => factory(bigSequence));
            }
        }

        [Theory]
        [InlineData(new byte[0], "")]
        [InlineData(new byte[] { 0x61 }, "a")]
        [InlineData(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 }, "\ufeffa")] // don't strip the BOM
        [InlineData(new byte[] { 0xEF, 0xBB, 0x61 }, null)] // ill-formed data throws
        public static void CreateFromReadOnlySequence_Tests(byte[] actualUtf8, string expectedAsUtf16)
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(actualUtf8);

            if (expectedAsUtf16 is null)
            {
                // Expect this to throw due to bad input.
                Assert.Throws<ArgumentException>(() => Utf8StringFactory.Create(sequence));
            }
            else
            {
                Assert.True(ustring.AreEquivalent(Utf8StringFactory.Create(sequence), expectedAsUtf16));
            }
        }

        [Theory]
        [InlineData(new byte[0], "")]
        [InlineData(new byte[] { 0x61 }, "a")]
        [InlineData(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 }, "\ufeffa")] // don't strip the BOM
        [InlineData(new byte[] { 0xEF, 0xBB, 0x61 }, "\ufffda")] // ill-formed data gets replaced
        public static void CreateRelaxedFromReadOnlySequence_Tests(byte[] actualUtf8, string expectedAsUtf16)
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(actualUtf8);
            Assert.True(ustring.AreEquivalent(Utf8StringFactory.CreateRelaxed(sequence), expectedAsUtf16));
        }

        [Theory]
        [InlineData(new byte[0], "")]
        [InlineData(new byte[] { 0x61 }, "a")]
        [InlineData(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 }, "\ufeffa")] // don't strip the BOM
        public static void UnsafeCreateWithoutValidationFromReadOnlySequence_Tests(byte[] actualUtf8, string expectedAsUtf16)
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(actualUtf8);
            Assert.True(ustring.AreEquivalent(Utf8StringFactory.UnsafeCreateWithoutValidation(sequence), expectedAsUtf16));
        }

        // A ReadOnlySequenceSegment<T> where all members are public read+write.
        private sealed class MockReadOnlySequenceSegment<T> : ReadOnlySequenceSegment<T>
        {
            public new ReadOnlyMemory<T> Memory { get => base.Memory; set => base.Memory = value; }
            public new ReadOnlySequenceSegment<T> Next { get => base.Next; set => base.Next = value; }
            public new long RunningIndex { get => base.RunningIndex; set => base.RunningIndex = value; }
        }
    }
}
