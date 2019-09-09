// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        [Fact]
        public static void BytesProperty_FromCustomBytes()
        {
            byte[] bytes = Encoding.UTF8.GetBytes("Hello!");
            Assert.True(bytes.AsSpan() == Utf8Span.UnsafeCreateWithoutValidation(bytes).Bytes);
        }

        [Fact]
        public static void BytesProperty_FromEmpty()
        {
            Assert.True(Utf8Span.Empty.Bytes == ReadOnlySpan<byte>.Empty);
        }

        [Fact]
        public static void BytesProperty_FromUtf8String()
        {
            ustring ustr = u8("Hello!");
            Utf8Span uspan = new Utf8Span(ustr);

            Assert.True(ustr.AsBytes() == uspan.Bytes);
        }

        [Fact]
        public static void EmptyProperty()
        {
            // Act

            Utf8Span span = Utf8Span.Empty;

            // Assert
            // GetPinnableReference should be 'null' to match behavior of empty ROS<byte>.GetPinnableReference();

            Assert.True(span.IsEmpty);
            Assert.Equal(IntPtr.Zero, (IntPtr)(void*)Unsafe.AsPointer(ref Unsafe.AsRef(in span.GetPinnableReference())));
            Assert.Equal(IntPtr.Zero, (IntPtr)(void*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span.Bytes)));
            Assert.Equal(0, span.Bytes.Length);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("Hello", true)]
        [InlineData("\u1234", false)]
        public static void IsAscii(string input, bool expected)
        {
            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);

            Assert.Equal(expected, boundedSpan.Span.IsAscii());
        }

        [Theory]
        [InlineData("", "..")]
        [InlineData("Hello", "1..")]
        [InlineData("Hello", "1..2")]
        [InlineData("Hello", "1..^2")]
        [InlineData("Hello", "^2..")]
        [InlineData("Hello", "^0..")]
        [InlineData("résumé", "1..^2")] // include first 'é', exclude last 'é'
        [InlineData("résumé", "^2..")] // include only last 'é'
        public static void Indexer_Success(string input, string rangeExpression)
        {
            Range range = ParseRangeExpr(rangeExpression);

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);
            Utf8Span originalSpan = boundedSpan.Span;
            Utf8Span slicedSpan = originalSpan[range]; // shouldn't throw

            ref byte startOfOriginalSpan = ref MemoryMarshal.GetReference(originalSpan.Bytes);
            ref byte startOfSlicedSpan = ref MemoryMarshal.GetReference(slicedSpan.Bytes);

            // Now ensure the slice was correctly produced by comparing the references directly.

            (int offset, int length) = range.GetOffsetAndLength(originalSpan.Bytes.Length);
            Assert.True(Unsafe.AreSame(ref startOfSlicedSpan, ref Unsafe.Add(ref startOfOriginalSpan, offset)));
            Assert.Equal(length, slicedSpan.Bytes.Length);
        }

        [Theory]
        [InlineData("résumé", "2..")] // try to split the first 'é'
        [InlineData("résumé", "..^1")] // try to split the last 'é'
        public static void Indexer_ThrowsIfTryToSplitMultiByteSubsequence(string input, string rangeExpression)
        {
            Range range = ParseRangeExpr(rangeExpression);

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(input);

            Assert.Throws<InvalidOperationException>(() => { var _ = boundedSpan.Span[range]; });
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello")]
        [InlineData("\U00000123\U00001234\U00101234")]
        public static void ToStringTest(string expected)
        {
            // Arrange

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(expected);
            Utf8Span span = boundedSpan.Span;

            // Act & assert

            Assert.Equal(expected, span.ToString());
        }

        [Fact]
        public static void ToStringTest_Null()
        {
            Assert.Same(string.Empty, Utf8Span.Empty.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello")]
        [InlineData("\U00000123\U00001234\U00101234")]
        public static void ToUtf8StringTest(string expected)
        {
            // Arrange

            ustring utf8 = u8(expected);

            using BoundedUtf8Span boundedSpan = new BoundedUtf8Span(expected);
            Utf8Span span = boundedSpan.Span;

            // Act & assert

            Assert.Equal(utf8, span.ToUtf8String());
        }

        [Fact]
        public static void ToUtf8StringTest_Null()
        {
            Assert.Same(ustring.Empty, Utf8Span.Empty.ToUtf8String());
        }
    }
}
