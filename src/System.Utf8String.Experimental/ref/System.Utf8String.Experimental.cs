// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

namespace System
{
    public static partial class Utf8Extensions
    {
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text) { throw null; }
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text, int start) { throw null; }
        public static System.ReadOnlySpan<byte> AsBytes(this System.Utf8String? text, int start, int length) { throw null; }
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
        public Utf8String(byte[]? value, int startIndex, int length) { }
        [System.CLSCompliantAttribute(false)]
        public unsafe Utf8String(char* value) { }
        public Utf8String(char[]? value, int startIndex, int length) { }
        public Utf8String(System.ReadOnlySpan<byte> value) { }
        public Utf8String(System.ReadOnlySpan<char> value) { }
        public Utf8String(string value) { }
        public ByteEnumerable Bytes { get { throw null; } }
        public CharEnumerable Chars { get { throw null; } }
        public RuneEnumerable Runes { get { throw null; } }
        public static bool AreEquivalent(System.Utf8String? utf8Text, string? utf16Text) { throw null; }
        public static bool AreEquivalent(System.Text.Utf8Span utf8Text, System.ReadOnlySpan<char> utf16Text) { throw null; }
        public static bool AreEquivalent(System.ReadOnlySpan<byte> utf8Text, System.ReadOnlySpan<char> utf16Text) { throw null; }
        public int CompareTo(System.Utf8String? other) { throw null; }
        public bool Contains(char value) { throw null; }
        public bool Contains(System.Text.Rune value) { throw null; }
        public static System.Utf8String Create<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public static System.Utf8String CreateFromLoose(System.ReadOnlySpan<byte> buffer) { throw null; }
        public static System.Utf8String CreateFromLoose(System.ReadOnlySpan<char> buffer) { throw null; }
        public static System.Utf8String CreateLoose<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public bool EndsWith(char value) { throw null; }
        public bool EndsWith(System.Text.Rune value) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public static bool Equals(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public bool Equals(System.Utf8String? value) { throw null; }
        public static explicit operator System.ReadOnlySpan<byte>(System.Utf8String? value) { throw null; }
        public static implicit operator System.Text.Utf8Span(System.Utf8String? value) { throw null; }
        public override int GetHashCode() { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref readonly byte GetPinnableReference() { throw null; }
        public bool IsAscii() { throw null; }
        public static bool IsNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(false)] System.Utf8String? value) { throw null; }
        public static bool operator !=(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public static bool operator ==(System.Utf8String? left, System.Utf8String? right) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Utf8String Slice(int startIndex, int length) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(char separator) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(char separator, System.StringComparison comparisonType) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(System.Text.Rune separator) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(System.Text.Rune separator, System.StringComparison comparisonType) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(System.Utf8String separator) { throw null; }
        public (System.Utf8String Before, System.Utf8String? After) SplitOn(System.Utf8String separator, System.StringComparison comparisonType) { throw null; }
        public System.Utf8String this[System.Range range] { get { throw null; } }
        public bool StartsWith(char value) { throw null; }
        public bool StartsWith(System.Text.Rune value) { throw null; }
        public byte[] ToByteArray() { throw null; }
        public byte[] ToByteArray(int startIndex, int length) { throw null; }
        public override string ToString() { throw null; }
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
    }
    [System.FlagsAttribute]
    public enum Utf8StringSplitOptions
    {
        None = 0,
        RemoveEmptyEntries = 1,
        TrimEntries = 2
    }
}
namespace System.Net.Http
{
    public sealed partial class Utf8StringContent : System.Net.Http.HttpContent
    {
        public Utf8StringContent(System.Utf8String content) { }
        public Utf8StringContent(System.Utf8String content, string? mediaType) { }
        protected override System.Threading.Tasks.Task<System.IO.Stream> CreateContentReadStreamAsync() { throw null; }
        protected override System.Threading.Tasks.Task SerializeToStreamAsync(System.IO.Stream stream, System.Net.TransportContext? context) { throw null; }
        protected override bool TryComputeLength(out long length) { throw null; }
    }
}
namespace System.Runtime.CompilerServices
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All)]
    public sealed partial class UnsafeMemberAttribute : Attribute
    {
    }
}
namespace System.Text
{
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
        [System.Runtime.CompilerServices.UnsafeMemberAttribute]
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
        [System.Runtime.CompilerServices.UnsafeMemberAttribute]
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
