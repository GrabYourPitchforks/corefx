// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

using static System.Tests.Utf8TestUtilities;

namespace System.Tests
{
    public unsafe partial class Utf8StringTests
    {
        [Fact]
        public static void Empty_HasLengthZero()
        {
            Assert.Equal(0, Utf8String.Empty.Length);
            SpanAssert.Equal(ReadOnlySpan<byte>.Empty, Utf8String.Empty.AsBytes());
        }

        [Fact]
        public static void Empty_ReturnsSingleton()
        {
            Assert.Same(Utf8String.Empty, Utf8String.Empty);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData("", null, false)]
        [InlineData(null, "", false)]
        [InlineData("hello", null, false)]
        [InlineData(null, "hello", false)]
        [InlineData("hello", "hello", true)]
        [InlineData("hello", "Hello", false)]
        [InlineData("hello there", "hello", false)]
        public static void Equality_Ordinal(string aString, string bString, bool expected)
        {
            Utf8String a = u8(aString);
            Utf8String b = u8(bString);

            // Operators

            Assert.Equal(expected, a == b);
            Assert.NotEqual(expected, a != b);

            // Static methods

            Assert.Equal(expected, Utf8String.Equals(a, b));
            Assert.Equal(expected, Utf8String.Equals(a, b, StringComparison.Ordinal));

            // Instance methods

            if (a != null)
            {
                Assert.Equal(expected, a.Equals((object)b));
                Assert.Equal(expected, a.Equals(b));
                Assert.Equal(expected, a.Equals(b, StringComparison.Ordinal));
            }
        }

        [Fact]
        public static void GetHashCode_ReturnsRandomized()
        {
            Utf8String a = u8("Hello");
            Utf8String b = new Utf8String(a.AsBytes());

            Assert.NotSame(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());

            Utf8String c = u8("Goodbye");
            Utf8String d = new Utf8String(c.AsBytes());

            Assert.NotSame(c, d);
            Assert.Equal(c.GetHashCode(), d.GetHashCode());

            Assert.NotEqual(a.GetHashCode(), c.GetHashCode());
        }

        [Fact]
        public static void GetPinnableReference_CalledMultipleTimes_ReturnsSameValue()
        {
            var utf8 = u8("Hello!");

            fixed (byte* pA = utf8)
            fixed (byte* pB = utf8)
            {
                Assert.True(pA == pB);
            }
        }

        [Fact]
        public static void GetPinnableReference_Empty()
        {
            fixed (byte* pStr = Utf8String.Empty)
            {
                Assert.True(pStr != null);
                Assert.Equal((byte)0, *pStr); // should point to null terminator
            }
        }

        [Fact]
        public static void GetPinnableReference_NotEmpty()
        {
            fixed (byte* pStr = u8("Hello!"))
            {
                Assert.True(pStr != null);

                Assert.Equal((byte)'H', pStr[0]);
                Assert.Equal((byte)'e', pStr[1]);
                Assert.Equal((byte)'l', pStr[2]);
                Assert.Equal((byte)'l', pStr[3]);
                Assert.Equal((byte)'o', pStr[4]);
                Assert.Equal((byte)'!', pStr[5]);
                Assert.Equal((byte)'\0', pStr[6]);
            }
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("not empty", false)]
        public static void IsNullOrEmpty(string value, bool expectedIsNullOrEmpty)
        {
            Assert.Equal(expectedIsNullOrEmpty, Utf8String.IsNullOrEmpty(new Utf8String(value)));
        }

        [Fact]
        public static void IsNullOrEmpty_Null_ReturnsTrue()
        {
            Assert.True(Utf8String.IsNullOrEmpty(null));
        }

        [Fact]
        public static void ToByteArray_Empty()
        {
            Assert.Same(Array.Empty<byte>(), Utf8String.Empty.ToByteArray());
        }

        [Fact]
        public static void ToByteArray_NotEmpty()
        {
            Assert.Equal(new byte[] { (byte)'H', (byte)'i' }, u8("Hi").ToByteArray());
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello!")]
        public static void ToString_ReturnsUtf16(string value)
        {
            Assert.Equal(value, u8(value).ToString());
        }
    }
}
