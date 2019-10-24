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
    public readonly partial struct Index : System.IEquatable<System.Index>
    {
        private readonly int _dummyPrimitive;
        public Index(int value, bool fromEnd = false) { throw null; }
        public static System.Index End { get { throw null; } }
        public bool IsFromEnd { get { throw null; } }
        public static System.Index Start { get { throw null; } }
        public int Value { get { throw null; } }
        public bool Equals(System.Index other) { throw null; }
        public override bool Equals(object? value) { throw null; }
        public static System.Index FromEnd(int value) { throw null; }
        public static System.Index FromStart(int value) { throw null; }
        public override int GetHashCode() { throw null; }
        public int GetOffset(int length) { throw null; }
        public static implicit operator System.Index(int value) { throw null; }
        public override string ToString() { throw null; }
    }
    public readonly partial struct Range : System.IEquatable<System.Range>
    {
        private readonly int _dummyPrimitive;
        public Range(System.Index start, System.Index end) { throw null; }
        public static System.Range All { get { throw null; } }
        public System.Index End { get { throw null; } }
        public System.Index Start { get { throw null; } }
        public static System.Range EndAt(System.Index end) { throw null; }
        public override bool Equals(object? value) { throw null; }
        public bool Equals(System.Range other) { throw null; }
        public override int GetHashCode() { throw null; }
        public (int Offset, int Length) GetOffsetAndLength(int length) { throw null; }
        public static System.Range StartAt(System.Index start) { throw null; }
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
    public sealed partial class Utf8String : System.IComparable<System.Utf8String?>,
#nullable disable
        System.IEquatable<System.Utf8String>
#nullable restore
    {
        public static readonly System.Utf8String Empty;
        [System.CLSCompliantAttribute(false)]
        public unsafe Utf8String(byte* value) { }
        public Utf8String(byte[] value, int startIndex, int length) { }
        [System.CLSCompliantAttribute(false)]
        public unsafe Utf8String(char* value) { }
        public Utf8String(char[] value, int startIndex, int length) { }
        public Utf8String(System.ReadOnlySpan<byte> value) { }
        public Utf8String(System.ReadOnlySpan<char> value) { }
        public Utf8String(string value) { }
        public ByteEnumerable Bytes { get { throw null; } }
        public CharEnumerable Chars { get { throw null; } }
        public int Length { get { throw null; } }
        public RuneEnumerable Runes { get { throw null; } }
        public static bool AreEquivalent(System.Utf8String? utf8Text, string? utf16Text) { throw null; }
        public static bool AreEquivalent(System.Text.Utf8Span utf8Text, System.ReadOnlySpan<char> utf16Text) { throw null; }
        public static bool AreEquivalent(System.ReadOnlySpan<byte> utf8Text, System.ReadOnlySpan<char> utf16Text) { throw null; }
        public int CompareTo(System.Utf8String? other) { throw null; }
        public int CompareTo(System.Utf8String? other, System.StringComparison comparison) { throw null; }
        public bool Contains(char value) { throw null; }
        public bool Contains(char value, System.StringComparison comparison) { throw null; }
        public bool Contains(System.Text.Rune value) { throw null; }
        public bool Contains(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool Contains(System.Utf8String value) { throw null; }
        public bool Contains(System.Utf8String value, System.StringComparison comparison) { throw null; }
        public static System.Utf8String Create<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public static System.Utf8String CreateFromRelaxed(System.ReadOnlySpan<byte> buffer) { throw null; }
        public static System.Utf8String CreateFromRelaxed(System.ReadOnlySpan<char> buffer) { throw null; }
        public static System.Utf8String CreateRelaxed<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public bool EndsWith(char value) { throw null; }
        public bool EndsWith(char value, System.StringComparison comparison) { throw null; }
        public bool EndsWith(System.Text.Rune value) { throw null; }
        public bool EndsWith(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool EndsWith(System.Utf8String value) { throw null; }
        public bool EndsWith(System.Utf8String value, System.StringComparison comparison) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public static bool Equals(System.Utf8String? a, System.Utf8String? b, System.StringComparison comparison) { throw null; }
        public static bool Equals(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public bool Equals(System.Utf8String? value) { throw null; }
        public bool Equals(System.Utf8String? value, System.StringComparison comparison) { throw null; }
        public override int GetHashCode() { throw null; }
        public int GetHashCode(System.StringComparison comparison) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref readonly byte GetPinnableReference() { throw null; }
        public static implicit operator System.Text.Utf8Span(System.Utf8String? value) { throw null; }
        public bool IsAscii() { throw null; }
        public bool IsNormalized(System.Text.NormalizationForm normalizationForm = System.Text.NormalizationForm.FormC) { throw null; }
        public static bool IsNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(false)] System.Utf8String? value) { throw null; }
        public static bool IsNullOrWhiteSpace([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(false)] System.Utf8String? value) { throw null; }
        public System.Utf8String Normalize(System.Text.NormalizationForm normalizationForm = System.Text.NormalizationForm.FormC) { throw null; }
        public static bool operator !=(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public static bool operator ==(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public SplitResult Split(char separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitResult Split(System.Text.Rune separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitResult Split(System.Utf8String separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitOnResult SplitOn(char separator) { throw null; }
        public SplitOnResult SplitOn(char separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOn(System.Text.Rune separator) { throw null; }
        public SplitOnResult SplitOn(System.Text.Rune separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOn(System.Utf8String separator) { throw null; }
        public SplitOnResult SplitOn(System.Utf8String separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(char separator) { throw null; }
        public SplitOnResult SplitOnLast(char separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Rune separator) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Rune separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(System.Utf8String separator) { throw null; }
        public SplitOnResult SplitOnLast(System.Utf8String separator, System.StringComparison comparisonType) { throw null; }
        public bool StartsWith(char value) { throw null; }
        public bool StartsWith(char value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Text.Rune value) { throw null; }
        public bool StartsWith(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Utf8String value) { throw null; }
        public bool StartsWith(System.Utf8String value, System.StringComparison comparison) { throw null; }
        public System.Utf8String this[System.Range range] { get { throw null; } }
        public byte[] ToByteArray() { throw null; }
        public char[] ToCharArray() { throw null; }
        public System.Utf8String ToLower(System.Globalization.CultureInfo culture) { throw null; }
        public System.Utf8String ToLowerInvariant() { throw null; }
        public override string ToString() { throw null; }
        public System.Utf8String ToUpper(System.Globalization.CultureInfo culture) { throw null; }
        public System.Utf8String ToUpperInvariant() { throw null; }
        public System.Utf8String Trim() { throw null; }
        public System.Utf8String TrimEnd() { throw null; }
        public System.Utf8String TrimStart() { throw null; }
        public static bool TryCreateFrom(System.ReadOnlySpan<byte> buffer, [System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] out System.Utf8String? value) { throw null; }
        public static bool TryCreateFrom(System.ReadOnlySpan<char> buffer, [System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] out System.Utf8String? value) { throw null; }
        public bool TryFind(char value, out System.Range range) { throw null; }
        public bool TryFind(char value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Rune value, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Rune value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFind(System.Utf8String value, out System.Range range) { throw null; }
        public bool TryFind(System.Utf8String value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(char value, out System.Range range) { throw null; }
        public bool TryFindLast(char value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Rune value, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Rune value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(System.Utf8String value, out System.Range range) { throw null; }
        public bool TryFindLast(System.Utf8String value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public static System.Utf8String UnsafeCreateWithoutValidation(System.ReadOnlySpan<byte> utf8Contents) { throw null; }
        public static System.Utf8String UnsafeCreateWithoutValidation<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public readonly partial struct ByteEnumerable : System.Collections.Generic.IEnumerable<byte>
        {
            private readonly object _dummy;
            public Enumerator GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            System.Collections.Generic.IEnumerator<byte> System.Collections.Generic.IEnumerable<byte>.GetEnumerator() { throw null; }
            public struct Enumerator : System.Collections.Generic.IEnumerator<byte>
            {
                private readonly object _dummy;
                private readonly int _dummyPrimitive;
                public byte Current { get { throw null; } }
                public bool MoveNext() { throw null; }
                void System.IDisposable.Dispose() { }
                object System.Collections.IEnumerator.Current { get { throw null; } }
                void System.Collections.IEnumerator.Reset() { }
            }
        }
        public readonly partial struct CharEnumerable : System.Collections.Generic.IEnumerable<char>
        {
            private readonly object _dummy;
            public Enumerator GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            System.Collections.Generic.IEnumerator<char> System.Collections.Generic.IEnumerable<char>.GetEnumerator() { throw null; }
            public struct Enumerator : System.Collections.Generic.IEnumerator<char>
            {
                private readonly object _dummy;
                private readonly int _dummyPrimitive;
                public char Current { get { throw null; } }
                public bool MoveNext() { throw null; }
                void System.IDisposable.Dispose() { }
                object System.Collections.IEnumerator.Current { get { throw null; } }
                void System.Collections.IEnumerator.Reset() { }
            }
        }
        public readonly partial struct RuneEnumerable : System.Collections.Generic.IEnumerable<System.Text.Rune>
        {
            private readonly object _dummy;
            public Enumerator GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            System.Collections.Generic.IEnumerator<System.Text.Rune> System.Collections.Generic.IEnumerable<System.Text.Rune>.GetEnumerator() { throw null; }
            public struct Enumerator : System.Collections.Generic.IEnumerator<System.Text.Rune>
            {
                private readonly object _dummy;
                private readonly int _dummyPrimitive;
                public System.Text.Rune Current { get { throw null; } }
                public bool MoveNext() { throw null; }
                void System.IDisposable.Dispose() { }
                object System.Collections.IEnumerator.Current { get { throw null; } }
                void System.Collections.IEnumerator.Reset() { }
            }
        }
        public readonly struct SplitResult : System.Collections.Generic.IEnumerable<Utf8String?>
        {
            private readonly object _dummy;
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3, out System.Utf8String? item4) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3, out System.Utf8String? item4, out System.Utf8String? item5) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3, out System.Utf8String? item4, out System.Utf8String? item5, out System.Utf8String? item6) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3, out System.Utf8String? item4, out System.Utf8String? item5, out System.Utf8String? item6, out System.Utf8String? item7) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String? item1, out System.Utf8String? item2, out System.Utf8String? item3, out System.Utf8String? item4, out System.Utf8String? item5, out System.Utf8String? item6, out System.Utf8String? item7, out System.Utf8String? item8) { throw null; }
            public Enumerator GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            System.Collections.Generic.IEnumerator<System.Utf8String?> System.Collections.Generic.IEnumerable<System.Utf8String?>.GetEnumerator() { throw null; }
            public struct Enumerator : System.Collections.Generic.IEnumerator<System.Utf8String?>
            {
                private readonly object _dummy;
                public System.Utf8String? Current { get { throw null; } }
                public bool MoveNext() { throw null; }
                void System.IDisposable.Dispose() { }
                object? System.Collections.IEnumerator.Current { get { throw null; } }
                void System.Collections.IEnumerator.Reset() { throw null; }
            }
        }
        public readonly struct SplitOnResult
        {
            private readonly object _dummy;
            public System.Utf8String? After { get { throw null; } }
            public System.Utf8String Before { get { throw null; } }
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Utf8String before, out System.Utf8String? after) { throw null; }
        }
    }
    [System.FlagsAttribute]
    public enum Utf8StringSplitOptions
    {
        None = 0,
        RemoveEmptyEntries = 1,
        TrimEntries = 2
    }
}
namespace System.Text
{
    public readonly partial struct Rune : System.IComparable<System.Text.Rune>, System.IEquatable<System.Text.Rune>
    {
        private readonly int _dummyPrimitive;
        public Rune(char ch) { throw null; }
        public Rune(char highSurrogate, char lowSurrogate) { throw null; }
        public Rune(int value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public Rune(uint value) { throw null; }
        public bool IsAscii { get { throw null; } }
        public bool IsBmp { get { throw null; } }
        public int Plane { get { throw null; } }
        public static System.Text.Rune ReplacementChar { get { throw null; } }
        public int Utf16SequenceLength { get { throw null; } }
        public int Utf8SequenceLength { get { throw null; } }
        public int Value { get { throw null; } }
        public int CompareTo(System.Text.Rune other) { throw null; }
        public static System.Buffers.OperationStatus DecodeFromUtf16(System.ReadOnlySpan<char> source, out System.Text.Rune result, out int charsConsumed) { throw null; }
        public static System.Buffers.OperationStatus DecodeFromUtf8(System.ReadOnlySpan<byte> source, out System.Text.Rune result, out int bytesConsumed) { throw null; }
        public static System.Buffers.OperationStatus DecodeLastFromUtf16(System.ReadOnlySpan<char> source, out System.Text.Rune result, out int charsConsumed) { throw null; }
        public static System.Buffers.OperationStatus DecodeLastFromUtf8(System.ReadOnlySpan<byte> source, out System.Text.Rune value, out int bytesConsumed) { throw null; }
        public int EncodeToUtf16(System.Span<char> destination) { throw null; }
        public int EncodeToUtf8(System.Span<byte> destination) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(System.Text.Rune other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static double GetNumericValue(System.Text.Rune value) { throw null; }
        public static System.Text.Rune GetRuneAt(string input, int index) { throw null; }
        public static System.Globalization.UnicodeCategory GetUnicodeCategory(System.Text.Rune value) { throw null; }
        public static bool IsControl(System.Text.Rune value) { throw null; }
        public static bool IsDigit(System.Text.Rune value) { throw null; }
        public static bool IsLetter(System.Text.Rune value) { throw null; }
        public static bool IsLetterOrDigit(System.Text.Rune value) { throw null; }
        public static bool IsLower(System.Text.Rune value) { throw null; }
        public static bool IsNumber(System.Text.Rune value) { throw null; }
        public static bool IsPunctuation(System.Text.Rune value) { throw null; }
        public static bool IsSeparator(System.Text.Rune value) { throw null; }
        public static bool IsSymbol(System.Text.Rune value) { throw null; }
        public static bool IsUpper(System.Text.Rune value) { throw null; }
        public static bool IsValid(int value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool IsValid(uint value) { throw null; }
        public static bool IsWhiteSpace(System.Text.Rune value) { throw null; }
        public static bool operator ==(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static explicit operator System.Text.Rune(char ch) { throw null; }
        public static explicit operator System.Text.Rune(int value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Text.Rune(uint value) { throw null; }
        public static bool operator >(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static bool operator >=(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static bool operator !=(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static bool operator <(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static bool operator <=(System.Text.Rune left, System.Text.Rune right) { throw null; }
        public static System.Text.Rune ToLower(System.Text.Rune value, System.Globalization.CultureInfo culture) { throw null; }
        public static System.Text.Rune ToLowerInvariant(System.Text.Rune value) { throw null; }
        public override string ToString() { throw null; }
        public static System.Text.Rune ToUpper(System.Text.Rune value, System.Globalization.CultureInfo culture) { throw null; }
        public static System.Text.Rune ToUpperInvariant(System.Text.Rune value) { throw null; }
        public static bool TryCreate(char highSurrogate, char lowSurrogate, out System.Text.Rune result) { throw null; }
        public static bool TryCreate(char ch, out System.Text.Rune result) { throw null; }
        public static bool TryCreate(int value, out System.Text.Rune result) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryCreate(uint value, out System.Text.Rune result) { throw null; }
        public bool TryEncodeToUtf16(System.Span<char> destination, out int charsWritten) { throw null; }
        public bool TryEncodeToUtf8(System.Span<byte> destination, out int bytesWritten) { throw null; }
        public static bool TryGetRuneAt(string input, int index, out System.Text.Rune value) { throw null; }
    }
    public readonly partial struct Utf8Segment : System.IComparable<System.Text.Utf8Segment>, System.IEquatable<System.Text.Utf8Segment>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Utf8Segment(System.Utf8String? value) { throw null; }
        public System.ReadOnlyMemory<byte> Bytes { get { throw null; } }
        public System.Text.Utf8Span Span { get { throw null; } }
        public int CompareTo(Utf8Segment other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(System.Text.Utf8Segment other) { throw null; }
        public bool Equals(System.Text.Utf8Segment other, System.StringComparison comparison) { throw null; }
        public static bool Equals(System.Text.Utf8Segment left, System.Text.Utf8Segment right) { throw null; }
        public static bool Equals(System.Text.Utf8Segment left, System.Text.Utf8Segment right, System.StringComparison comparison) { throw null; }
        public override int GetHashCode() { throw null; }
        public int GetHashCode(System.StringComparison comparison) { throw null; }
        public static bool operator !=(System.Text.Utf8Segment left, System.Text.Utf8Segment right) { throw null; }
        public static bool operator ==(System.Text.Utf8Segment left, System.Text.Utf8Segment right) { throw null; }
        public override string ToString() { throw null; }
        public System.Utf8String ToUtf8String() { throw null; }
        public static System.Text.Utf8Segment UnsafeCreateWithoutValidation(System.ReadOnlyMemory<byte> buffer) { throw null; }
    }
    public readonly ref partial struct Utf8Span
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Utf8Span(System.Utf8String? value) { throw null; }
        public System.ReadOnlySpan<byte> Bytes { get { throw null; } }
        public CharEnumerable Chars { get { throw null; } }
        public static System.Text.Utf8Span Empty { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public int Length { get { throw null; } }
        public RuneEnumerable Runes { get { throw null; } }
        public int CompareTo(System.Text.Utf8Span other) { throw null; }
        public int CompareTo(System.Text.Utf8Span other, System.StringComparison comparison) { throw null; }
        public bool Contains(char value) { throw null; }
        public bool Contains(char value, System.StringComparison comparison) { throw null; }
        public bool Contains(System.Text.Rune value) { throw null; }
        public bool Contains(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool Contains(System.Text.Utf8Span value) { throw null; }
        public bool Contains(System.Text.Utf8Span value, System.StringComparison comparison) { throw null; }
        public bool EndsWith(char value) { throw null; }
        public bool EndsWith(char value, System.StringComparison comparison) { throw null; }
        public bool EndsWith(System.Text.Rune value) { throw null; }
        public bool EndsWith(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool EndsWith(System.Text.Utf8Span value) { throw null; }
        public bool EndsWith(System.Text.Utf8Span value, System.StringComparison comparison) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ObsoleteAttribute("Equals(object) on Utf8Span will always throw an exception. Use Equals(Utf8Span) or == instead.")]
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(System.Text.Utf8Span other) { throw null; }
        public bool Equals(System.Text.Utf8Span other, System.StringComparison comparison) { throw null; }
        public static bool Equals(System.Text.Utf8Span left, System.Text.Utf8Span right) { throw null; }
        public static bool Equals(System.Text.Utf8Span left, System.Text.Utf8Span right, System.StringComparison comparison) { throw null; }
        public override int GetHashCode() { throw null; }
        public int GetHashCode(System.StringComparison comparison) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref readonly byte GetPinnableReference() { throw null; }
        public bool IsAscii() { throw null; }
        public bool IsEmptyOrWhiteSpace() { throw null; }
        public bool IsNormalized(System.Text.NormalizationForm normalizationForm = System.Text.NormalizationForm.FormC) { throw null; }
        public System.Utf8String Normalize(System.Text.NormalizationForm normalizationForm = System.Text.NormalizationForm.FormC) { throw null; }
        public int Normalize(System.Span<byte> destination, System.Text.NormalizationForm normalizationForm = System.Text.NormalizationForm.FormC) { throw null; }
        public static bool operator !=(System.Text.Utf8Span left, System.Text.Utf8Span right) { throw null; }
        public static bool operator ==(System.Text.Utf8Span left, System.Text.Utf8Span right) { throw null; }
        public System.Text.Utf8Span this[System.Range range] { get { throw null; } }
        public SplitResult Split(char separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitResult Split(System.Text.Rune separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitResult Split(System.Text.Utf8Span separator, System.Utf8StringSplitOptions options = System.Utf8StringSplitOptions.None) { throw null; }
        public SplitOnResult SplitOn(char separator) { throw null; }
        public SplitOnResult SplitOn(char separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOn(System.Text.Rune separator) { throw null; }
        public SplitOnResult SplitOn(System.Text.Rune separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOn(System.Text.Utf8Span separator) { throw null; }
        public SplitOnResult SplitOn(System.Text.Utf8Span separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(char separator) { throw null; }
        public SplitOnResult SplitOnLast(char separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Rune separator) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Rune separator, System.StringComparison comparisonType) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Utf8Span separator) { throw null; }
        public SplitOnResult SplitOnLast(System.Text.Utf8Span separator, System.StringComparison comparisonType) { throw null; }
        public bool StartsWith(char value) { throw null; }
        public bool StartsWith(char value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Text.Rune value) { throw null; }
        public bool StartsWith(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Text.Utf8Span value) { throw null; }
        public bool StartsWith(System.Text.Utf8Span value, System.StringComparison comparison) { throw null; }
        public System.Text.Utf8Span Trim() { throw null; }
        public System.Text.Utf8Span TrimEnd() { throw null; }
        public System.Text.Utf8Span TrimStart() { throw null; }
        public byte[] ToByteArray() { throw null; }
        public char[] ToCharArray() { throw null; }
        public int ToChars(System.Span<char> destination) { throw null; }
        public System.Utf8String ToLower(System.Globalization.CultureInfo culture) { throw null; }
        public int ToLower(System.Span<byte> destination, System.Globalization.CultureInfo culture) { throw null; }
        public System.Utf8String ToLowerInvariant() { throw null; }
        public int ToLowerInvariant(System.Span<byte> destination) { throw null; }
        public override string ToString() { throw null; }
        public System.Utf8String ToUpper(System.Globalization.CultureInfo culture) { throw null; }
        public int ToUpper(System.Span<byte> destination, System.Globalization.CultureInfo culture) { throw null; }
        public System.Utf8String ToUpperInvariant() { throw null; }
        public int ToUpperInvariant(System.Span<byte> destination) { throw null; }
        public System.Utf8String ToUtf8String() { throw null; }
        public bool TryFind(char value, out System.Range range) { throw null; }
        public bool TryFind(char value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Rune value, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Rune value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Utf8Span value, out System.Range range) { throw null; }
        public bool TryFind(System.Text.Utf8Span value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(char value, out System.Range range) { throw null; }
        public bool TryFindLast(char value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Rune value, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Rune value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Utf8Span value, out System.Range range) { throw null; }
        public bool TryFindLast(System.Text.Utf8Span value, System.StringComparison comparisonType, out System.Range range) { throw null; }
        public static System.Text.Utf8Span UnsafeCreateWithoutValidation(System.ReadOnlySpan<byte> buffer) { throw null; }
        public readonly ref struct CharEnumerable
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public Enumerator GetEnumerator() { throw null; }
            public ref struct Enumerator
            {
                private object _dummy;
                private int _dummyPrimitive;
                public char Current { get { throw null; } }
                public bool MoveNext() { throw null; }
            }
        }
        public readonly ref struct RuneEnumerable
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public Enumerator GetEnumerator() { throw null; }
            public ref struct Enumerator
            {
                private object _dummy;
                private int _dummyPrimitive;
                public System.Text.Rune Current { get { throw null; } }
                public bool MoveNext() { throw null; }
            }
        }
        public readonly ref struct SplitResult
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3, out System.Text.Utf8Span item4) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3, out System.Text.Utf8Span item4, out System.Text.Utf8Span item5) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3, out System.Text.Utf8Span item4, out System.Text.Utf8Span item5, out System.Text.Utf8Span item6) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3, out System.Text.Utf8Span item4, out System.Text.Utf8Span item5, out System.Text.Utf8Span item6, out System.Text.Utf8Span item7) { throw null; }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span item1, out System.Text.Utf8Span item2, out System.Text.Utf8Span item3, out System.Text.Utf8Span item4, out System.Text.Utf8Span item5, out System.Text.Utf8Span item6, out System.Text.Utf8Span item7, out System.Text.Utf8Span item8) { throw null; }
            public Enumerator GetEnumerator() { throw null; }
            public ref struct Enumerator
            {
                private readonly object _dummy;
                private readonly int _dummyPrimitive;
                public System.Text.Utf8Span Current { get { throw null; } }
                public bool MoveNext() { throw null; }
            }
        }
        public readonly ref struct SplitOnResult
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public Utf8Span After { get { throw null; } }
            public Utf8Span Before { get { throw null; } }
            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
            public void Deconstruct(out System.Text.Utf8Span before, out System.Text.Utf8Span after) { throw null; }
        }
    }
    public abstract class Utf8StringComparer : System.Collections.Generic.IComparer<System.Text.Utf8Segment>, System.Collections.Generic.IComparer<System.Utf8String?>, System.Collections.Generic.IEqualityComparer<System.Text.Utf8Segment>, System.Collections.Generic.IEqualityComparer<System.Utf8String?>
    {
        private Utf8StringComparer() { }
        public static System.Text.Utf8StringComparer CurrentCulture { get { throw null; } }
        public static System.Text.Utf8StringComparer CurrentCultureIgnoreCase { get { throw null; } }
        public static System.Text.Utf8StringComparer InvariantCulture { get { throw null; } }
        public static System.Text.Utf8StringComparer InvariantCultureIgnoreCase { get { throw null; } }
        public static System.Text.Utf8StringComparer Ordinal { get { throw null; } }
        public static System.Text.Utf8StringComparer OrdinalIgnoreCase { get { throw null; } }
        public static System.Text.Utf8StringComparer Create(System.Globalization.CultureInfo culture, bool ignoreCase) { throw null; }
        public static System.Text.Utf8StringComparer Create(System.Globalization.CultureInfo culture, System.Globalization.CompareOptions options) { throw null; }
        public static System.Text.Utf8StringComparer FromComparison(System.StringComparison comparisonType) { throw null; }
        public abstract int Compare(System.Text.Utf8Segment x, System.Text.Utf8Segment y);
        public abstract int Compare(System.Utf8String? x, System.Utf8String? y);
        public abstract int Compare(System.Text.Utf8Span x, System.Text.Utf8Span y);
        public abstract bool Equals(System.Text.Utf8Segment x, System.Text.Utf8Segment y);
        public abstract bool Equals(System.Utf8String? x, System.Utf8String? y);
        public abstract bool Equals(System.Text.Utf8Span x, System.Text.Utf8Span y);
        public abstract int GetHashCode(System.Text.Utf8Segment obj);
#pragma warning disable CS8614 // Remove warning disable when nullable attributes are respected
        public abstract int GetHashCode(System.Utf8String obj);
#pragma warning restore CS8614
        public abstract int GetHashCode(System.Text.Utf8Span obj);
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
