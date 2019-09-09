// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        [Fact]
        public static void CharsProperty_FromData()
        {
            ustring ustr = u8("\U00000012\U00000123\U00001234\U00101234\U00000012\U00000123\U00001234\U00101234");
            var charsEnumerator = ustr.Chars.GetEnumerator();

            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00000012', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00000123', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00001234', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\uDBC4', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\uDE34', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00000012', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00000123', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\U00001234', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\uDBC4', charsEnumerator.Current);
            Assert.True(charsEnumerator.MoveNext());
            Assert.Equal('\uDE34', charsEnumerator.Current);
            Assert.False(charsEnumerator.MoveNext());
        }

        [Fact]
        public static void CharsProperty_FromEmpty()
        {
            Assert.False(Utf8Span.Empty.Chars.GetEnumerator().MoveNext());
        }

        [Fact]
        public static void RunesProperty_FromEmpty()
        {
            Assert.False(Utf8Span.Empty.Runes.GetEnumerator().MoveNext());
        }

        [Fact]
        public static void RunesProperty_FromData()
        {
            ustring ustr = u8("\U00000012\U00000123\U00001234\U00101234\U00000012\U00000123\U00001234\U00101234");
            var runesEnumerator = ustr.Runes.GetEnumerator();

            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x0012), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x0123), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x1234), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x101234), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x0012), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x0123), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x1234), runesEnumerator.Current);
            Assert.True(runesEnumerator.MoveNext());
            Assert.Equal(new Rune(0x101234), runesEnumerator.Current);
            Assert.False(runesEnumerator.MoveNext());
        }
    }
}
