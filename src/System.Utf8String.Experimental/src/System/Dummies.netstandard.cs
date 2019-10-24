// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA1823, CS0169 // ignore warnings about unused fields

namespace System
{
    public readonly partial struct Char8 : System.IComparable<System.Char8>, System.IEquatable<System.Char8>
    {
        private readonly int _dummyPrimitive;
        public int CompareTo(System.Char8 other) { throw null; }
        public bool Equals(System.Char8 other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Char8 left, System.Char8 right) { throw null; }
        public static explicit operator System.Char8(char value) { throw null; }
        public static explicit operator char(System.Char8 value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator sbyte(System.Char8 value) { throw null; }
        public static explicit operator System.Char8(short value) { throw null; }
        public static explicit operator System.Char8(int value) { throw null; }
        public static explicit operator System.Char8(long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Char8(sbyte value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Char8(ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Char8(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Char8(ulong value) { throw null; }
        public static bool operator >(System.Char8 left, System.Char8 right) { throw null; }
        public static bool operator >=(System.Char8 left, System.Char8 right) { throw null; }
        public static implicit operator System.Char8(byte value) { throw null; }
        public static implicit operator byte(System.Char8 value) { throw null; }
        public static implicit operator short(System.Char8 value) { throw null; }
        public static implicit operator int(System.Char8 value) { throw null; }
        public static implicit operator long(System.Char8 value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator ushort(System.Char8 value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator uint(System.Char8 value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator ulong(System.Char8 value) { throw null; }
        public static bool operator !=(System.Char8 left, System.Char8 right) { throw null; }
        public static bool operator <(System.Char8 left, System.Char8 right) { throw null; }
        public static bool operator <=(System.Char8 left, System.Char8 right) { throw null; }
        public override string ToString() { throw null; }
    }
    public static partial class Utf8Extensions
    {
        public static System.ReadOnlySpan<byte> AsBytes(this System.ReadOnlySpan<System.Char8> text) { throw null; }
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text) { throw null; }
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text, int start) { throw null; }
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text, int start, int length) { throw null; }
        public static System.ReadOnlyMemory<System.Char8> AsMemory(this System.Utf8String? text) { throw null; }
        public static System.ReadOnlyMemory<System.Char8> AsMemory(this System.Utf8String? text, System.Index startIndex) { throw null; }
        public static System.ReadOnlyMemory<System.Char8> AsMemory(this System.Utf8String? text, int start) { throw null; }
        public static System.ReadOnlyMemory<System.Char8> AsMemory(this System.Utf8String? text, int start, int length) { throw null; }
        public static System.ReadOnlyMemory<System.Char8> AsMemory(this System.Utf8String? text, System.Range range) { throw null; }
        public static System.ReadOnlyMemory<byte> AsMemoryBytes(this System.Utf8String? text) { throw null; }
        public static System.ReadOnlyMemory<byte> AsMemoryBytes(this System.Utf8String? text, System.Index startIndex) { throw null; }
        public static System.ReadOnlyMemory<byte> AsMemoryBytes(this System.Utf8String? text, int start) { throw null; }
        public static System.ReadOnlyMemory<byte> AsMemoryBytes(this System.Utf8String? text, int start, int length) { throw null; }
        public static System.ReadOnlyMemory<byte> AsMemoryBytes(this System.Utf8String? text, System.Range range) { throw null; }
        public static System.Text.Utf8Segment AsSegment(this System.Utf8String? text) { throw null; }
        public static System.Text.Utf8Segment AsSegment(this System.Utf8String? text, System.Index startIndex) { throw null; }
        public static System.Text.Utf8Segment AsSegment(this System.Utf8String? text, int start) { throw null; }
        public static System.Text.Utf8Segment AsSegment(this System.Utf8String? text, int start, int length) { throw null; }
        public static System.Text.Utf8Segment AsSegment(this System.Utf8String? text, System.Range range) { throw null; }
        public static System.Text.Utf8Span AsSpan(this System.Utf8String? text) { throw null; }
        public static System.Text.Utf8Span AsSpan(this System.Utf8String? text, int start) { throw null; }
        public static System.Text.Utf8Span AsSpan(this System.Utf8String? text, int start, int length) { throw null; }
        public static System.Utf8String ToUtf8String(this System.Text.Rune rune) { throw null; }
    }
}
namespace System.Text.Unicode
{
    public static partial class TR29Utility
    {
        public static int GetLengthOfFirstUtf16ExtendedGraphemeCluster(System.ReadOnlySpan<char> input) { throw null; }
        public static int GetLengthOfFirstUtf8ExtendedGraphemeCluster(System.ReadOnlySpan<byte> input) { throw null; }
    }
}
