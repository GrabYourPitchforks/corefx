// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;

namespace System
{
    /// <summary>
    /// Represents an immutable string of UTF-8 code units.
    /// </summary>
    public sealed partial class Utf8String : IComparable<Utf8String?>,
#nullable disable // see comment on String
        IEquatable<Utf8String>
#nullable restore
    {
        /*
         * STATIC FIELDS
         */

        public static readonly Utf8String Empty = FastAllocate(0);

        /*
         * INSTANCE FIELDS
         */

        private int _length = 0;
        private byte[] _nullTerminatedData = s_singleElementNullTerminatedArray;

        /*
         * OPERATORS
         */

        /// <summary>
        /// Compares two <see cref="Utf8String"/> instances for equality using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public static bool operator ==(Utf8String? left, Utf8String? right) => Equals(left, right);

        /// <summary>
        /// Compares two <see cref="Utf8String"/> instances for inequality using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public static bool operator !=(Utf8String? left, Utf8String? right) => !Equals(left, right);

        /// <summary>
        /// Projects a <see cref="Utf8String"/> instance as a <see cref="Utf8Span"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Utf8Span(Utf8String? value) => new Utf8Span(value);

        /*
         * INSTANCE PROPERTIES
         */

        /// <summary>
        /// Returns the length (in UTF-8 code units, or <see cref="byte"/>s) of this instance.
        /// </summary>
        public int Length => _length;

        /*
         * INDEXERS
         */

        public Utf8String this[Range range]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // The two lines immediately below provide no bounds checking.
                // The Substring method we call will both perform a bounds check
                // and check for an improper split across a multi-byte subsequence.

                int startIdx = range.Start.GetOffset(Length);
                int endIdx = range.End.GetOffset(Length);

                return Substring(startIdx, endIdx - startIdx);
            }
        }

        /*
         * METHODS
         */

        /// <summary>
        /// Similar to <see cref="Utf8Extensions.AsBytes(Utf8String)"/>, but skips the null check on the input.
        /// Throws a <see cref="NullReferenceException"/> if the input is null.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlySpan<byte> AsBytesSkipNullCheck()
        {
            // By dereferencing Length first, the JIT will skip the null check that normally precedes
            // most instance method calls, and it'll use the field dereference as the null check.

            _ = _nullTerminatedData.Length;
            return new ReadOnlySpan<byte>(_nullTerminatedData, 0, Length);
        }

        /// <summary>
        /// Similar to <see cref="Utf8Extensions.AsSpan(Utf8String)"/>, but skips the null check on the input.
        /// Throws a <see cref="NullReferenceException"/> if the input is null.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Utf8Span AsSpanSkipNullCheck()
        {
            return Utf8Span.UnsafeCreateWithoutValidation(this.AsBytesSkipNullCheck());
        }

        public int CompareTo(Utf8String? other)
        {
            // TODO_UTF8STRING: This is ordinal, but String.CompareTo uses CurrentCulture.
            // Is this acceptable? Should we perhaps just remove the interface?

            return Utf8StringComparer.Ordinal.Compare(this, other);
        }

        public int CompareTo(Utf8String? other, StringComparison comparison)
        {
            // TODO_UTF8STRING: We can avoid the virtual dispatch by moving the switch into this method.

            return Utf8StringComparer.FromComparison(comparison).Compare(this, other);
        }

        /// <summary>
        /// Returns a <em>mutable</em> <see cref="Span{Byte}"/> that can be used to populate this
        /// <see cref="Utf8String"/> instance. Only to be used during construction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<byte> DangerousGetMutableSpan()
        {
            // By dereferencing Length first, the JIT will skip the null check that normally precedes
            // most instance method calls, and it'll use the field dereference as the null check.

            _ = _nullTerminatedData.Length;
            return new Span<byte>(_nullTerminatedData, 0, Length);
        }

        /// <summary>
        /// Returns a <em>mutable</em> reference to the first byte of this <see cref="Utf8String"/>
        /// (or the null terminator if the string is empty).
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref byte DangerousGetMutableReference() => ref _nullTerminatedData[0];

        /// <summary>
        /// Returns a <em>mutable</em> reference to the element at index <paramref name="index"/>
        /// of this <see cref="Utf8String"/> instance. The index is not bounds-checked.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref byte DangerousGetMutableReference(int index)
        {
            Debug.Assert(index >= 0, "Caller should've performed bounds checking.");
            return ref DangerousGetMutableReference((uint)index);
        }

        /// <summary>
        /// Returns a <em>mutable</em> reference to the element at index <paramref name="index"/>
        /// of this <see cref="Utf8String"/> instance. The index is not bounds-checked.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref byte DangerousGetMutableReference(nuint index)
        {
            // Allow retrieving references to the null terminator.

            Debug.Assert(index <= (uint)Length, "Caller should've performed bounds checking.");
            return ref Unsafe.AddByteOffset(ref DangerousGetMutableReference(), index);
        }

        /// <summary>
        /// Performs an equality comparison using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return (obj is Utf8String other) && this.Equals(other);
        }

        /// <summary>
        /// Performs an equality comparison using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public bool Equals(Utf8String? value)
        {
            // First, a very quick check for referential equality.

            if (ReferenceEquals(this, value))
            {
                return true;
            }

            // Otherwise, perform a simple bitwise equality check.

            return !(value is null)
                && this.Length == value.Length
                && this.AsBytesSkipNullCheck().SequenceEqual(value.AsBytesSkipNullCheck());
        }

        /// <summary>
        /// Performs an equality comparison using the specified <see cref="StringComparison"/>.
        /// </summary>
        public bool Equals(Utf8String? value, StringComparison comparison) => Equals(this, value, comparison);

        /// <summary>
        /// Compares two <see cref="Utf8String"/> instances using a <see cref="StringComparison.Ordinal"/> comparer.
        /// </summary>
        public static bool Equals(Utf8String? left, Utf8String? right)
        {
            // First, a very quick check for referential equality.

            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // Otherwise, perform a simple bitwise equality check.

            return !(left is null)
                && !(right is null)
                && left.Length == right.Length
                && left.AsBytesSkipNullCheck().SequenceEqual(right.AsBytesSkipNullCheck());
        }

        /// <summary>
        /// Performs an equality comparison using the specified <see cref="StringComparison"/>.
        /// </summary>
        public static bool Equals(Utf8String? a, Utf8String? b, StringComparison comparison)
        {
            // TODO_UTF8STRING: This perf can be improved, including removing
            // the virtual dispatch by putting the switch directly in this method.

            return Utf8StringComparer.FromComparison(comparison).Equals(a, b);
        }

        /// <summary>
        /// Returns a hash code using a <see cref="StringComparison.Ordinal"/> comparison.
        /// </summary>
        public override int GetHashCode()
        {
            // TODO_UTF8STRING: Consider whether this should use a different seed than String.GetHashCode.

            return Marvin.ComputeHash32(this.AsBytes(), Marvin.DefaultSeed);
        }

        /// <summary>
        /// Returns a hash code using the specified <see cref="StringComparison"/>.
        /// </summary>
        public int GetHashCode(StringComparison comparison)
        {
            // TODO_UTF8STRING: This perf can be improved, including removing
            // the virtual dispatch by putting the switch directly in this method.

            return Utf8StringComparer.FromComparison(comparison).GetHashCode(this);
        }

        /// <summary>
        /// Like string.GetNonRandomizedHashCode, this should only be used when DoS
        /// isn't a concern. It's trivial for a malicious agent to create hash collisions using
        /// this algorithm.
        /// </summary>
        internal int GetNonRandomizedHashCode()
        {
            // This algorithm is essentially a carbon copy of string.GetNonRandomizedHashCode,
            // which itself is a modification of djb2.

            int length = Length;
            ref uint ptr = ref Unsafe.As<byte, uint>(ref DangerousGetMutableReference());

            unsafe
            {
                Debug.Assert(((uint)Unsafe.AsPointer(ref ptr) % sizeof(uint)) == 0, "Start of Utf8String data should be 32-bit aligned.");
                Debug.Assert(DangerousGetMutableReference(length) == 0, "Utf8String data should be null-terminated.");
            }

            uint hash1 = (5381 << 16) + 5381;
            uint hash2 = hash1;

            // We'll read 64 bits at a time. Endianness doesn't matter within the loop.
            // If the length is one byte short of a multiple of 8 (e.g., 7, 15, or 23 bytes),
            // this loop will additionally consume the null terminator.

            while (length >= 7)
            {
                length -= 8;
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr;
                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ Unsafe.Add(ref ptr, 1);
                ptr = ref Unsafe.Add(ref ptr, 2);
            }

            // n.b. at this point 'length' could be negative if we've already consumed the
            // null terminator in the main loop above.

            if (length >= 3)
            {
                // Read 32 bits and fold them into hash1. Endianness doesn't matter here.
                // If the remaining length is 3 bytes we'll also end up consuming the null
                // terminator here.

                length -= 4;
                hash1 = (BitOperations.RotateLeft(hash1, 5) + hash1) ^ ptr;
                ptr = ref Unsafe.Add(ref ptr, 1);
            }

            Debug.Assert(length >= -1, "Shouldn't have read beyond the null terminator.");
            Debug.Assert(length <= 2, "Should have no more than 2 bytes left to read.");

            if (length > 0)
            {
                // There's between 1 and 2 bytes (not including the null terminator)
                // remaining to be read. So we'll perform a single aligned 16-bit read
                // to drain the remaining data. If there's only 1 byte remaining then
                // this will also consume the null terminator. Endianness doesn't matter.

                hash2 = (BitOperations.RotateLeft(hash2, 5) + hash2) ^ Unsafe.As<uint, ushort>(ref ptr);
            }

            return (int)(hash1 + (hash2 * 1566083941u));
        }

        /// <summary>
        /// Gets an immutable reference that can be used in a <see langword="fixed"/> statement. The resulting
        /// reference can be pinned and used as a null-terminated <em>LPCUTF8STR</em>.
        /// </summary>
        /// <remarks>
        /// If this <see cref="Utf8String"/> instance is empty, returns a reference to the null terminator.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)] // for compiler use only
        public ref readonly byte GetPinnableReference() => ref _nullTerminatedData[0];

        /// <summary>
        /// Returns <see langword="true"/> if this UTF-8 text consists of all-ASCII data,
        /// <see langword="false"/> if there is any non-ASCII data within this UTF-8 text.
        /// </summary>
        /// <remarks>
        /// ASCII text is defined as text consisting only of scalar values in the range [ U+0000..U+007F ].
        /// Empty strings are considered to be all-ASCII. The runtime of this method is O(n).
        /// </remarks>
        public bool IsAscii()
        {
            return this.AsSpan().IsAscii();
        }

        /// <summary>
        /// Returns <see langword="true"/> if <paramref name="value"/> is <see langword="null"/> or zero length;
        /// <see langword="false"/> otherwise.
        /// </summary>
        public static bool IsNullOrEmpty([NotNullWhen(false)] Utf8String? value)
        {
            // Copied from String.IsNullOrEmpty. See that method for detailed comments on why this pattern is used.
            return (value is null || 0u >= (uint)value.Length) ? true : false;
        }

        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] Utf8String? value)
        {
            return (value is null) || value.AsSpan().IsEmptyOrWhiteSpace();
        }

        /// <summary>
        /// Returns the entire <see cref="Utf8String"/> as an array of UTF-8 bytes.
        /// </summary>
        public byte[] ToByteArray() => this.AsSpanSkipNullCheck().ToByteArray();

        /// <summary>
        /// Converts this <see cref="Utf8String"/> instance to a <see cref="string"/>.
        /// </summary>
        public override string ToString()
        {
            // TODO_UTF8STRING: Optimize the call below, potentially by avoiding the two-pass.

            return Encoding.UTF8.GetString(this.AsBytesSkipNullCheck());
        }

        /// <summary>
        /// Converts this <see cref="Utf8String"/> instance to a <see cref="string"/>.
        /// </summary>
        /// <remarks>
        /// This routine throws <see cref="InvalidOperationException"/> if the underlying instance
        /// contains invalid UTF-8 data.
        /// </remarks>
        internal unsafe string ToStringNoReplacement()
        {
            // TODO_UTF8STRING: Optimize the call below, potentially by avoiding the two-pass.

            int utf16CharCount;

            fixed (byte* pData = &GetPinnableReference())
            {
                byte* pFirstInvalidByte = Utf8Utility.GetPointerToFirstInvalidByte(pData, this.Length, out int utf16CodeUnitCountAdjustment, out _);
                if (pFirstInvalidByte != pData + (uint)this.Length)
                {
                    // Saw bad UTF-8 data.
                    // TODO_UTF8STRING: Throw a better exception below?

                    ThrowHelper.ThrowInvalidOperationException();
                }

                utf16CharCount = this.Length + utf16CodeUnitCountAdjustment;
                Debug.Assert(utf16CharCount <= this.Length && utf16CharCount >= 0);

                // TODO_UTF8STRING: Can we call string.FastAllocate directly?

                return Encoding.UTF8.GetString(pData, this.Length);
            }
        }
    }
}
