// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        [InlineData("")]
        [InlineData("Hello")]
        [InlineData("\U00000123\U00001234\U00101234")]
        public static void ToStringTest(string expected)
        {
            // Arrange

            Utf8Span span = Utf8Span.UnsafeCreateWithoutValidation(Encoding.UTF8.GetBytes(expected));

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
            Utf8Span span = Utf8Span.UnsafeCreateWithoutValidation(utf8.AsBytes());

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
