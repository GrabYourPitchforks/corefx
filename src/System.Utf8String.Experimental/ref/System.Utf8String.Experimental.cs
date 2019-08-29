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
        public Utf8String(string? value) { }
        public ByteEnumerable Bytes { get { throw null; } }
        public CharEnumerable Chars { get { throw null; } }
        public int Length { get { throw null; } }
        public RuneEnumerable Runes { get { throw null; } }
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
        public int IndexOf(char value) { throw null; }
        public int IndexOf(System.Text.Rune value) { throw null; }
        public static bool IsNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(false)] System.Utf8String? value) { throw null; }
        public static bool operator !=(System.Utf8String? left, System.Utf8String? right) { throw null; }
        public static bool operator ==(System.Utf8String? left, System.Utf8String? right) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public System.Utf8String Slice(int startIndex, int length) { throw null; }
        public bool StartsWith(char value) { throw null; }
        public bool StartsWith(System.Text.Rune value) { throw null; }
        public System.Utf8String Substring(System.Index startIndex) { throw null; }
        public System.Utf8String Substring(int startIndex) { throw null; }
        public System.Utf8String Substring(int startIndex, int length) { throw null; }
        public byte[] ToByteArray() { throw null; }
        public byte[] ToByteArray(int startIndex, int length) { throw null; }
        public override string ToString() { throw null; }
        public static bool TryCreateFrom(System.ReadOnlySpan<byte> buffer, [System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] out System.Utf8String? value) { throw null; }
        public static bool TryCreateFrom(System.ReadOnlySpan<char> buffer, [System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] out System.Utf8String? value) { throw null; }
        public static System.Utf8String UnsafeCreateWithoutValidation(System.ReadOnlySpan<byte> utf8Contents) { throw null; }
        public static System.Utf8String UnsafeCreateWithoutValidation<TState>(int length, TState state, System.Buffers.SpanAction<byte, TState> action) { throw null; }
        public readonly partial struct ByteEnumerable : System.Collections.Generic.IEnumerable<byte>
        {
            private readonly object _dummy;
            public Enumerator GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            System.Collections.Generic.IEnumerator<byte> System.Collections.Generic.IEnumerable<byte>.GetEnumerator() { throw null; }
            public struct Enumerator : System.Collections.Generic.IEnumerable<byte>
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
            public struct Enumerator : System.Collections.Generic.IEnumerable<char>
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
            public struct Enumerator : System.Collections.Generic.IEnumerable<System.Text.Rune>
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
        public bool IsEmpty { get { throw null; } }
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
        public bool StartsWith(char value) { throw null; }
        public bool StartsWith(char value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Text.Rune value) { throw null; }
        public bool StartsWith(System.Text.Rune value, System.StringComparison comparison) { throw null; }
        public bool StartsWith(System.Text.Utf8Span value) { throw null; }
        public bool StartsWith(System.Text.Utf8Span value, System.StringComparison comparison) { throw null; }
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
        [System.Runtime.CompilerServices.UnsafeMemberAttribute]
        public static System.Text.Utf8Span UnsafeCreateWithoutValidation(System.ReadOnlySpan<byte> buffer) { throw null; }
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
        public abstract int GetHashCode(System.Utf8String obj);
        public abstract int GetHashCode(System.Text.Utf8Span obj);
    }
}
