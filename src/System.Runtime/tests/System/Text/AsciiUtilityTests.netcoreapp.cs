// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Xunit;

namespace System.Text.Tests
{
    // Since many of the methods we'll be testing are internal, we'll need to invoke
    // them via reflection.
    public unsafe static partial class AsciiUtilityTests
    {
        private static bool Is64Bit => IntPtr.Size >= 8;

        private const int SizeOfVector128 = 128 / 8;

        // The delegate definitions and members below provide us access to CoreLib's internals.

        private delegate TNuint FnGetIndexOfFirstNonAsciiElement<TElement, TNuint>(TElement* pBuffer, TNuint bufferLength) where TElement : unmanaged;
        private static Lazy<FnGetIndexOfFirstNonAsciiElement<byte, UIntPtr>> _fnGetIndexOfFirstNonAsciiByte
            = new Lazy<FnGetIndexOfFirstNonAsciiElement<byte, UIntPtr>>(() => CreateNuintDelegate<FnGetIndexOfFirstNonAsciiElement<byte, UIntPtr>>("GetIndexOfFirstNonAsciiByte"));
        private static Lazy<FnGetIndexOfFirstNonAsciiElement<char, UIntPtr>> _fnGetIndexOfFirstNonAsciiChar
            = new Lazy<FnGetIndexOfFirstNonAsciiElement<char, UIntPtr>>(() => CreateNuintDelegate<FnGetIndexOfFirstNonAsciiElement<char, UIntPtr>>("GetIndexOfFirstNonAsciiChar"));

        private delegate TNuint FnTranscodeAsciiElements<TElementFrom, TElementTo, TNuint>(TElementFrom* pBufferFrom, TElementTo* pBufferTo, TNuint bufferLenght) where TElementFrom : unmanaged where TElementTo : unmanaged;
        private static Lazy<FnTranscodeAsciiElements<char, byte, UIntPtr>> _fnNarrowAsciiCharsToBytes
            = new Lazy<FnTranscodeAsciiElements<char, byte, UIntPtr>>(() => CreateNuintDelegate<FnTranscodeAsciiElements<char, byte, UIntPtr>>("NarrowUtf16ToAscii"));
        private static Lazy<FnTranscodeAsciiElements<byte, char, UIntPtr>> _fnWidenAsciiBytesToChars
            = new Lazy<FnTranscodeAsciiElements<byte, char, UIntPtr>>(() => CreateNuintDelegate<FnTranscodeAsciiElements<byte, char, UIntPtr>>("WidenAsciiToUtf16"));

        [Fact]
        public unsafe static void GetIndexOfFirstNonAsciiByte_EmptyInput_NullReference()
        {
            Assert.Equal(0ul, CallGetIndexOfFirstNonAsciiByte(ref Unsafe.AsRef<byte>(null), 0));
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiByte_EmptyInput_NonNullReference()
        {
            byte b = default;
            Assert.Equal(0ul, CallGetIndexOfFirstNonAsciiByte(ref b, 0));
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiByte_Vector128Boundaries()
        {
            // The purpose of this test is to ensure that during our out-of-bounds SSE2
            // read at the beginning of data processing, we don't inadvertently treat
            // ASCII data outside the bounds of the vector as being part of the input data.

            const int SliceSize = 7;

            using (BoundedMemory<byte> mem = BoundedMemory.Allocate<byte>(2 * SizeOfVector128 + SliceSize))
            {
                mem.Span.Clear();

                for (int i = 0; i < 2 * SizeOfVector128; i++)
                {
                    // First, test an all-ASCII slice surrounded by non-ASCII data.

                    mem.Span.Fill(0x80);
                    Span<byte> slice = mem.Span.Slice(i, SliceSize);
                    slice.Clear();
                    Assert.Equal(SliceSize, CallGetIndexOfFirstNonAsciiByte(slice));

                    // Then, test an all-ASCII slice surrounded by ASCII data.

                    mem.Span.Clear();
                    Assert.Equal(SliceSize, CallGetIndexOfFirstNonAsciiByte(slice));

                    // Then, test various bytes within the slice being non-ASCII, even
                    // though the surrounding data is ASCII.

                    for (int j = slice.Length - 1; j >= 0; j--)
                    {
                        slice.Clear();
                        slice[j] = 0x80; // set non-ASCII byte
                        Assert.Equal(j, CallGetIndexOfFirstNonAsciiByte(slice));
                    }
                }
            }
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiByte_Vector128InnerLoop()
        {
            // The purpose of this test is to make sure we're identifying the correct
            // vector (of the two that we're reading simultaneously) when performing
            // the final ASCII drain at the end of the method once we've broken out
            // of the inner loop.

            using (BoundedMemory<byte> mem = BoundedMemory.Allocate<byte>(1024))
            {
                Span<byte> bytes = mem.Span;
                bytes.Clear();

                // Two vectors have offsets 0 .. 31. We'll go backward to avoid having to
                // re-clear the vector every time.

                for (int i = 2 * SizeOfVector128 - 1; i >= 0; i--)
                {
                    bytes[100 + i * 13] = 0x80; // 13 is relatively prime to 32, so it ensures all possible positions are hit
                    Assert.Equal(100 + i * 13, CallGetIndexOfFirstNonAsciiByte(bytes));
                }
            }
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiByte_Boundaries()
        {
            // The purpose of this test is to make sure we're hitting all of the vectorized
            // and draining logic correctly both in the SSE2 and in the non-SSE2 enlightened
            // code paths. We shouldn't be reading beyond the boundaries we were given.

            // The 5 * Vector test should make sure that we're exercising all possible
            // code paths across both implementations.
            using (BoundedMemory<byte> mem = BoundedMemory.Allocate<byte>(5 * Vector<byte>.Count))
            {
                Span<byte> bytes = mem.Span;

                // First, try it with all-ASCII buffers.

                bytes.Clear();
                for (int i = bytes.Length; i >= 0; i--)
                {
                    Assert.Equal(i, CallGetIndexOfFirstNonAsciiByte(bytes.Slice(0, i)));
                }

                // Then, try it with non-ASCII bytes.

                for (int i = bytes.Length; i >= 1; i--)
                {
                    bytes[i - 1] = 0x80; // set non-ASCII
                    Assert.Equal(i - 1, CallGetIndexOfFirstNonAsciiByte(bytes.Slice(0, i)));
                }
            }
        }

        [Fact]
        public unsafe static void GetIndexOfFirstNonAsciiChar_EmptyInput_NullReference()
        {
            Assert.Equal(0ul, CallGetIndexOfFirstNonAsciiChar(ref Unsafe.AsRef<char>(null), 0));
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiChar_EmptyInput_NonNullReference()
        {
            char c = default;
            Assert.Equal(0ul, CallGetIndexOfFirstNonAsciiChar(ref c, 0));
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiChar_Vector128Boundaries()
        {
            // The purpose of this test is to ensure that during our out-of-bounds SSE2
            // read at the beginning of data processing, we don't inadvertently treat
            // ASCII data outside the bounds of the vector as being part of the input data.

            const int SliceSize = 7;

            // Unlike the byte version, the char version uses 1 * SizeOfVector128 because
            // we already multiply by 2 (total bytes) to account for sizeof(char).

            using (BoundedMemory<char> mem = BoundedMemory.Allocate<char>(SizeOfVector128 + SliceSize))
            {
                for (int i = 0; i < SizeOfVector128; i++)
                {
                    // First, test an all-ASCII slice surrounded by non-ASCII data.
                    // Use U+0123 instead of U+0080 for this test because if our implementation incorrectly
                    // uses pmovmskb, U+0123 will incorrectly show up as ASCII, causing our test to fail
                    // and helping diagnose bugs.

                    mem.Span.Fill('\u0123');
                    Span<char> slice = mem.Span.Slice(i, SliceSize);
                    slice.Clear();
                    Assert.Equal(SliceSize, CallGetIndexOfFirstNonAsciiChar(slice));

                    // Then, test an all-ASCII slice surrounded by ASCII data.

                    mem.Span.Clear();
                    Assert.Equal(SliceSize, CallGetIndexOfFirstNonAsciiChar(slice));

                    // Then, test various bytes within the slice being non-ASCII, even
                    // though the surrounding data is ASCII.

                    for (int j = slice.Length - 1; j >= 0; j--)
                    {
                        slice.Clear();
                        slice[j] = '\u0123'; // set non-ASCII char
                        Assert.Equal(j, CallGetIndexOfFirstNonAsciiChar(slice));
                    }
                }
            }
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiChar_Vector128InnerLoop()
        {
            // The purpose of this test is to make sure we're identifying the correct
            // vector (of the two that we're reading simultaneously) when performing
            // the final ASCII drain at the end of the method once we've broken out
            // of the inner loop.

            using (BoundedMemory<char> mem = BoundedMemory.Allocate<char>(1024))
            {
                Span<char> chars = mem.Span;
                chars.Clear();

                // Two vectors have offsets 0 .. 31. We'll go backward to avoid having to
                // re-clear the vector every time.

                for (int i = 2 * SizeOfVector128 - 1; i >= 0; i--)
                {
                    chars[100 + i * 13] = '\u0123'; // 13 is relatively prime to 32, so it ensures all possible positions are hit
                    Assert.Equal(100 + i * 13, CallGetIndexOfFirstNonAsciiChar(chars));
                }
            }
        }

        [Fact]
        public static void GetIndexOfFirstNonAsciiChar_Boundaries()
        {
            // The purpose of this test is to make sure we're hitting all of the vectorized
            // and draining logic correctly both in the SSE2 and in the non-SSE2 enlightened
            // code paths. We shouldn't be reading beyond the boundaries we were given.

            // The 5 * Vector test should make sure that we're exercising all possible
            // code paths across both implementations. The sizeof(char) is because we're
            // specifying element count, but underlying implementation reintepret casts to bytes.
            using (BoundedMemory<char> mem = BoundedMemory.Allocate<char>(5 * Vector<byte>.Count / sizeof(char)))
            {
                Span<char> chars = mem.Span;

                // First, try it with all-ASCII buffers.

                chars.Clear();
                for (int i = chars.Length; i >= 0; i--)
                {
                    Assert.Equal(i, CallGetIndexOfFirstNonAsciiChar(chars.Slice(0, i)));
                }

                // Then, try it with non-ASCII bytes.

                for (int i = chars.Length; i >= 1; i--)
                {
                    chars[i - 1] = '\u0123'; // set non-ASCII
                    Assert.Equal(i - 1, CallGetIndexOfFirstNonAsciiChar(chars.Slice(0, i)));
                    Assert.Equal(i - 1, CallGetIndexOfFirstNonAsciiChar(chars));
                }
            }
        }

        [Fact]
        public unsafe static void WidenAsciiToUtf16_EmptyInput_NullReferences()
        {
            Assert.Equal(0ul, CallWidenAsciiToUtf16(ref Unsafe.AsRef<byte>(null), ref Unsafe.AsRef<char>(null), 0));
        }

        [Fact]
        public static void WidenAsciiToUtf16_EmptyInput_NonNullReference()
        {
            byte b = default;
            char c = default;
            Assert.Equal(0ul, CallWidenAsciiToUtf16(ref b, ref c, 0));
        }

        [Fact]
        public static void WidenAsciiToUtf16_AllAsciiInput()
        {
            using BoundedMemory<byte> asciiMem = BoundedMemory.Allocate<byte>(128);
            using BoundedMemory<char> utf16Mem = BoundedMemory.Allocate<char>(128);

            // Fill source with 00 .. 7F, then trap future writes.

            Span<byte> asciiSpan = asciiMem.Span;
            for (int i = 0; i < asciiSpan.Length; i++)
            {
                asciiSpan[i] = (byte)i;
            }
            asciiMem.MakeReadonly();

            // We'll write to the UTF-16 span.
            // We test with a variety of span lengths to test alignment and fallthrough code paths.

            Span<char> utf16Span = utf16Mem.Span;

            for (int i = 0; i < asciiSpan.Length; i++)
            {
                utf16Span.Clear(); // remove any data from previous iteration

                // First, validate that the workhorse saw the incoming data as all-ASCII.

                Assert.Equal(128 - i, CallWidenAsciiToUtf16(asciiSpan.Slice(i), utf16Span.Slice(i)));

                // Then, validate that the data was transcoded properly.

                for (int j = i; j < 128; j++)
                {
                    Assert.Equal((ushort)asciiSpan[i], (ushort)utf16Span[i]);
                }
            }
        }

        [Fact]
        public static void WidenAsciiToUtf16_SomeNonAsciiInput()
        {
            using BoundedMemory<byte> asciiMem = BoundedMemory.Allocate<byte>(128);
            using BoundedMemory<char> utf16Mem = BoundedMemory.Allocate<char>(128);

            // Fill source with 00 .. 7F, then trap future writes.

            Span<byte> asciiSpan = asciiMem.Span;
            for (int i = 0; i < asciiSpan.Length; i++)
            {
                asciiSpan[i] = (byte)i;
            }

            // We'll write to the UTF-16 span.

            Span<char> utf16Span = utf16Mem.Span;

            for (int i = asciiSpan.Length - 1; i >= 0; i--)
            {
                RandomNumberGenerator.Fill(MemoryMarshal.Cast<char, byte>(utf16Span)); // fill with garbage

                // First, keep track of the garbage we wrote to the destination.
                // We want to ensure it wasn't overwritten.

                char[] expectedTrailingData = utf16Span.Slice(i).ToArray();

                // Then, set the desired byte as non-ASCII, then check that the workhorse
                // correctly saw the data as non-ASCII.

                asciiSpan[i] |= (byte)0x80;
                Assert.Equal(i, CallWidenAsciiToUtf16(asciiSpan, utf16Span));

                // Next, validate that the ASCII data was transcoded properly.

                for (int j = 0; j < i; j++)
                {
                    Assert.Equal((ushort)asciiSpan[j], (ushort)utf16Span[j]);
                }

                // Finally, validate that the trailing data wasn't overwritten with non-ASCII data.

                Assert.Equal(expectedTrailingData, utf16Span.Slice(i).ToArray());
            }
        }

        [Fact]
        public unsafe static void NarrowUtf16ToAscii_EmptyInput_NullReferences()
        {
            Assert.Equal(0ul, CallNarrowUtf16ToAscii(ref Unsafe.AsRef<char>(null), ref Unsafe.AsRef<byte>(null), 0));
        }

        [Fact]
        public static void NarrowUtf16ToAscii_EmptyInput_NonNullReference()
        {
            char c = default;
            byte b = default;
            Assert.Equal(0ul, CallNarrowUtf16ToAscii(ref c, ref b, 0));
        }

        [Fact]
        public static void NarrowUtf16ToAscii_AllAsciiInput()
        {
            using BoundedMemory<char> utf16Mem = BoundedMemory.Allocate<char>(128);
            using BoundedMemory<byte> asciiMem = BoundedMemory.Allocate<byte>(128);

            // Fill source with 00 .. 7F.

            Span<char> utf16Span = utf16Mem.Span;
            for (int i = 0; i < utf16Span.Length; i++)
            {
                utf16Span[i] = (char)i;
            }
            utf16Mem.MakeReadonly();

            // We'll write to the ASCII span.
            // We test with a variety of span lengths to test alignment and fallthrough code paths.

            Span<byte> asciiSpan = asciiMem.Span;

            for (int i = 0; i < utf16Span.Length; i++)
            {
                asciiSpan.Clear(); // remove any data from previous iteration

                // First, validate that the workhorse saw the incoming data as all-ASCII.

                Assert.Equal(128 - i, CallNarrowUtf16ToAscii(utf16Span.Slice(i), asciiSpan.Slice(i)));

                // Then, validate that the data was transcoded properly.

                for (int j = i; j < 128; j++)
                {
                    Assert.Equal((ushort)utf16Span[i], (ushort)asciiSpan[i]);
                }
            }
        }

        [Fact]
        public static void NarrowUtf16ToAscii_SomeNonAsciiInput()
        {
            using BoundedMemory<char> utf16Mem = BoundedMemory.Allocate<char>(128);
            using BoundedMemory<byte> asciiMem = BoundedMemory.Allocate<byte>(128);

            // Fill source with 00 .. 7F.

            Span<char> utf16Span = utf16Mem.Span;
            for (int i = 0; i < utf16Span.Length; i++)
            {
                utf16Span[i] = (char)i;
            }

            // We'll write to the ASCII span.

            Span<byte> asciiSpan = asciiMem.Span;

            for (int i = utf16Span.Length - 1; i >= 0; i--)
            {
                RandomNumberGenerator.Fill(asciiSpan); // fill with garbage

                // First, keep track of the garbage we wrote to the destination.
                // We want to ensure it wasn't overwritten.

                byte[] expectedTrailingData = asciiSpan.Slice(i).ToArray();

                // Then, set the desired byte as non-ASCII, then check that the workhorse
                // correctly saw the data as non-ASCII.

                utf16Span[i] = '\u0123'; // use U+0123 instead of U+0080 since it catches inappropriate pmovmskb usage
                Assert.Equal(i, CallNarrowUtf16ToAscii(utf16Span, asciiSpan));

                // Next, validate that the ASCII data was transcoded properly.

                for (int j = 0; j < i; j++)
                {
                    Assert.Equal((ushort)utf16Span[j], (ushort)asciiSpan[j]);
                }

                // Finally, validate that the trailing data wasn't overwritten with non-ASCII data.

                Assert.Equal(expectedTrailingData, asciiSpan.Slice(i).ToArray());
            }
        }

        private static int CallGetIndexOfFirstNonAsciiByte(ReadOnlySpan<byte> buffer)
        {
            return checked((int)CallGetIndexOfFirstNonAsciiByte(ref MemoryMarshal.GetReference(buffer), (uint)buffer.Length));
        }

        private static ulong CallGetIndexOfFirstNonAsciiByte(ref byte buffer, ulong bufferLength)
        {
            // Conversions between UIntPtr <-> ulong are checked.
            fixed (byte* pBuffer = &buffer)
            {
                return (ulong)_fnGetIndexOfFirstNonAsciiByte.Value(pBuffer, (UIntPtr)bufferLength);
            }
        }

        private static int CallGetIndexOfFirstNonAsciiChar(ReadOnlySpan<char> buffer)
        {
            return checked((int)CallGetIndexOfFirstNonAsciiChar(ref MemoryMarshal.GetReference(buffer), (uint)buffer.Length));
        }

        private static ulong CallGetIndexOfFirstNonAsciiChar(ref char buffer, ulong bufferLength)
        {
            // Conversions between UIntPtr <-> ulong are checked.
            fixed (char* pBuffer = &buffer)
            {
                return (ulong)_fnGetIndexOfFirstNonAsciiChar.Value(pBuffer, (UIntPtr)bufferLength);
            }
        }

        private static int CallNarrowUtf16ToAscii(ReadOnlySpan<char> utf16, Span<byte> ascii)
        {
            Assert.Equal(utf16.Length, ascii.Length);
            return checked((int)CallNarrowUtf16ToAscii(ref MemoryMarshal.GetReference(utf16), ref MemoryMarshal.GetReference(ascii), (ulong)utf16.Length));
        }

        private static ulong CallNarrowUtf16ToAscii(ref char utf16, ref byte ascii, ulong bufferLength)
        {
            // Conversions between UIntPtr <-> ulong are checked.
            fixed (char* pUtf16 = &utf16)
            fixed (byte* pAscii = &ascii)
            {
                return (ulong)_fnNarrowAsciiCharsToBytes.Value(pUtf16, pAscii, (UIntPtr)bufferLength);
            }
        }

        private static int CallWidenAsciiToUtf16(ReadOnlySpan<byte> ascii, Span<char> utf16)
        {
            Assert.Equal(ascii.Length, utf16.Length);
            return checked((int)CallWidenAsciiToUtf16(ref MemoryMarshal.GetReference(ascii), ref MemoryMarshal.GetReference(utf16), (ulong)ascii.Length));
        }

        private static ulong CallWidenAsciiToUtf16(ref byte ascii, ref char utf16, ulong bufferLength)
        {
            // Conversions between UIntPtr <-> ulong are checked.
            fixed (byte* pAscii = &ascii)
            fixed (char* pUtf16 = &utf16)
            {
                return (ulong)_fnWidenAsciiBytesToChars.Value(pAscii, pUtf16, (UIntPtr)bufferLength);
            }
        }

        private static Type GetAsciiUtilityType()
        {
            return typeof(object).Assembly.GetType("System.Text.AsciiUtility");
        }

        private static TDelegate CreateNuintDelegate<TDelegate>(string methodName) where TDelegate : class
        {
            // First, replace UIntPtr with nuint (uint or ulong) everywhere it exists.

            Type[] invocationArguments = typeof(TDelegate).GetMethod("Invoke").GetParameters().Select(pi => pi.ParameterType).ToArray();
            for (int i = 0; i < invocationArguments.Length; i++)
            {
                if (invocationArguments[i] == typeof(UIntPtr))
                {
                    invocationArguments[i] = (IntPtr.Size >= 8) ? typeof(ulong) : typeof(uint);
                }
            }

            // Next, find the generic MethodInfo for the target method.
            // We'll forward the argument types of the delegate's Invoke method.

            MethodInfo methodInfo = GetAsciiUtilityType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                null,
                invocationArguments,
                null);

            Assert.NotNull(methodInfo);

            // Do the UIntPtr -> nuint conversion again and use it to create a Delegate from the MethodInfo.

            Type[] genericArguments = typeof(TDelegate).GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (genericArguments[i] == typeof(UIntPtr))
                {
                    genericArguments[i] = (IntPtr.Size >= 8) ? typeof(ulong) : typeof(uint);
                }
            }

            Delegate del = methodInfo.CreateDelegate(typeof(TDelegate).GetGenericTypeDefinition().MakeGenericType(genericArguments));

            // Now we lie to the type system, forcibly converting it to a delegate type
            // that uses UIntPtr rather than nuint. This still works at runtime because
            // the method signatures are blittable with one another.

            return Unsafe.As<TDelegate>(del);
        }
    }
}
