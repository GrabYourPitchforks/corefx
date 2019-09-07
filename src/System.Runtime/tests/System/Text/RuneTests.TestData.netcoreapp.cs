// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace System.Text.Tests
{
    public static partial class RuneTests
    {
        private static IEnumerable<Rune> AllPossibleScalars
        {
            get
            {
                // BMP before surrogate range
                for (uint i = 0; i < 0xD800; i++)
                {
                    yield return new Rune(i);
                }

                // BMP after surrogate range, plus astral planes
                for (uint i = 0xE000; i <= 0x10FFFF; i++)
                {
                    yield return new Rune(i);
                }
            }
        }

        public static IEnumerable<object[]> GeneralTestData_BmpCodePoints_NoSurrogates()
        {
            yield return new object[]
            {
                // first BMP code point / first ASCII code point
                new GeneralTestData
                {
                    ScalarValue = 0x0000,
                    IsAscii = true,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\u0000' },
                    Utf8Sequence = new byte[] { 0x00 }
                }
            };

            yield return new object[]
            {
                // last ASCII code point
                new GeneralTestData
                {
                    ScalarValue = 0x007F,
                    IsAscii = true,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\u007F' },
                    Utf8Sequence = new byte[] { 0x7F }
                }
            };

            yield return new object[] {
                // first non-ASCII code point / first UTF-8 two-code unit code point
                new GeneralTestData
                {
                    ScalarValue = 0x0080,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\u0080' },
                    Utf8Sequence = new byte[] { 0xC2, 0x80 }
                }
            };

            yield return new object[] {
                // last UTF-8 two-code unit code point
                new GeneralTestData
                {
                    ScalarValue = 0x07FF,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\u07FF' },
                    Utf8Sequence = new byte[] { 0xDF, 0xBF }
                }
            };

            yield return new object[] {
                // first UTF-8 three-code unit code point
                new GeneralTestData
                {
                    ScalarValue = 0x0800,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\u0800' },
                    Utf8Sequence = new byte[] { 0xE0, 0xA0, 0x80 }
                }
            };

            yield return new object[] {
                // last code point before the surrogate range
                new GeneralTestData
                {
                    ScalarValue = 0xD7FF,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\uD7FF' },
                    Utf8Sequence = new byte[] { 0xED, 0x9F, 0xBF }
                }
            };

            yield return new object[] {
                // first code point after the surrogate range
                new GeneralTestData
                {
                    ScalarValue = 0xE000,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\uE000' },
                    Utf8Sequence = new byte[] { 0xEE, 0x80, 0x80 }
                }
            };

            yield return new object[] {
                // replacement character
                new GeneralTestData
                {
                    ScalarValue = 0xFFFD,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\uFFFD' },
                    Utf8Sequence = new byte[] { 0xEF, 0xBF, 0xBD }
                }
            };

            yield return new object[] {
                // last BMP code point / last UTF-8 two-code unit code point
                new GeneralTestData
                {
                    ScalarValue = 0xFFFF,
                    IsAscii = false,
                    IsBmp = true,
                    Plane = 0,
                    Utf16Sequence = new char[] { '\uFFFF' },
                    Utf8Sequence = new byte[] { 0xEF, 0xBF, 0xBF }
                }
            };
        }

        public static IEnumerable<object[]> GeneralTestData_SupplementaryCodePoints_ValidOnly()
        {
            yield return new object[]
            {
                // first BMP code point / first ASCII code point
                new GeneralTestData
                {
                    ScalarValue = 0x10000,
                    IsAscii = false,
                    IsBmp = false,
                    Plane = 1,
                    Utf16Sequence = new char[] { '\uD800', '\uDC00' },
                    Utf8Sequence = new byte[] { 0xF0, 0x90, 0x80, 0x80 }
                }
            };

            yield return new object[]
            {
                // last supplementary code point
                new GeneralTestData
                {
                    ScalarValue = 0x10FFFF,
                    IsAscii = false,
                    IsBmp = false,
                    Plane = 16,
                    Utf16Sequence = new char[] { '\uDBFF', '\uDFFF' },
                    Utf8Sequence = new byte[] { 0xF4, 0x8F, 0xBF, 0xBF }
                }
            };
        }
        public static IEnumerable<object[]> IsValidTestData()
        {
            foreach (var obj in GeneralTestData_BmpCodePoints_NoSurrogates().Concat(GeneralTestData_SupplementaryCodePoints_ValidOnly()))
            {
                yield return new object[] { ((GeneralTestData)obj[0]).ScalarValue /* value */, true /* isValid */ };
            }

            foreach (var obj in BmpCodePoints_SurrogatesOnly().Concat(SupplementaryCodePoints_InvalidOnly()))
            {
                yield return new object[] { Convert.ToInt32(obj[0], CultureInfo.InvariantCulture) /* value */, false /* isValid */ };
            }
        }

        public static IEnumerable<object[]> BmpCodePoints_SurrogatesOnly()
        {
            yield return new object[] { '\uD800' }; // first high surrogate code point
            yield return new object[] { '\uDBFF' }; // last high surrogate code point
            yield return new object[] { '\uDC00' }; // first low surrogate code point
            yield return new object[] { '\uDFFF' }; // last low surrogate code point
        }

        public static IEnumerable<object[]> SupplementaryCodePoints_InvalidOnly()
        {
            yield return new object[] { (int)-1 }; // negative code points are disallowed
            yield return new object[] { (int)0x110000 }; // just past the end of the allowed code point range
            yield return new object[] { int.MaxValue }; // too large
        }

        public static IEnumerable<object[]> SurrogatePairTestData_InvalidOnly()
        {
            yield return new object[] { '\ud800', '\ud800' }; // two high surrogates
            yield return new object[] { '\udfff', '\udfff' }; // two low surrogates
            yield return new object[] { '\ude00', '\udb00' }; // low surrogate before high surrogate
            yield return new object[] { '\ud900', '\u1234' }; // high surrogate followed by non-surrogate
            yield return new object[] { '\ud900', '\ue000' }; // high surrogate followed by value just beyond low surrogate range
            yield return new object[] { '\u1234', '\ude00' }; // low surrogate preceded by non-surrogate
            yield return new object[] { '\udc00', '\ude00' }; // low surrogate preceded by value just beyond high surrogate range
        }

        public static IEnumerable<object[]> SurrogatePairTestData_ValidOnly()
        {
            yield return new object[] { '\ud800', '\udc00', 0x00010000 }; // lower bound for high & low surrogate
            yield return new object[] { '\udbff', '\udfff', 0x0010FFFF }; // upper bound for high & low surrogate
            yield return new object[] { '\ud83c', '\udfa8', 0x0001F3A8 }; // U+1F3A8 ARTIST PALETTE
        }

        public class GeneralTestData
        {
            public int ScalarValue;
            public bool IsAscii;
            public bool IsBmp;
            public int Plane;
            public char[] Utf16Sequence;
            public byte[] Utf8Sequence;
        }

        public static IEnumerable<object[]> UnicodeInfoTestData_Latin1AndSelectOthers()
        {
            // ASCII

            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x00), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x01), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x02), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x03), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x04), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x05), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x06), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x07), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x08), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x09), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0A), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0B), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0C), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0D), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0E), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x0F), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x10), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x11), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x12), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x13), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x14), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x15), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x16), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x17), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x18), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x19), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1A), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1B), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1C), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1D), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1E), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x1F), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x20), UnicodeCategory = UnicodeCategory.SpaceSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x21), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x22), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x23), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x24), UnicodeCategory = UnicodeCategory.CurrencySymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x25), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x26), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x27), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x28), UnicodeCategory = UnicodeCategory.OpenPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x29), UnicodeCategory = UnicodeCategory.ClosePunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2A), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2B), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2C), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2D), UnicodeCategory = UnicodeCategory.DashPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2E), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2F), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x30), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 0, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x31), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 1, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x32), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 2, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x33), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 3, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x34), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 4, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x35), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 5, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x36), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 6, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x37), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 7, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x38), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 8, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x39), UnicodeCategory = UnicodeCategory.DecimalDigitNumber, NumericValue = 9, IsControl = false, IsDigit = true, IsLetter = false, IsLetterOrDigit = true, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3A), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3B), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3C), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3D), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3E), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x3F), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x40), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x41), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x42), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x43), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x44), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x45), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x46), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x47), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x48), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x49), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4A), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4B), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4C), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4D), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4E), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x4F), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x50), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x51), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x52), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x53), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x54), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x55), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x56), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x57), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x58), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x59), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5A), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5B), UnicodeCategory = UnicodeCategory.OpenPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5C), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5D), UnicodeCategory = UnicodeCategory.ClosePunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5E), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x5F), UnicodeCategory = UnicodeCategory.ConnectorPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x60), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x61), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x62), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x63), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x64), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x65), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x66), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x67), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x68), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x69), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6A), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6B), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6C), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6D), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6E), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x6F), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x70), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x71), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x72), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x73), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x74), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x75), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x76), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x77), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x78), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x79), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7A), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7B), UnicodeCategory = UnicodeCategory.OpenPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7C), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7D), UnicodeCategory = UnicodeCategory.ClosePunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7E), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x7F), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

            // Remainder of Latin-1

            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x80), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x81), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x82), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x83), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x84), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x85), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x86), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x87), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x88), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x89), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8A), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8B), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8C), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8D), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8E), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x8F), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x90), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x91), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x92), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x93), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x94), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x95), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x96), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x97), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x98), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x99), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9A), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9B), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9C), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9D), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9E), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x9F), UnicodeCategory = UnicodeCategory.Control, NumericValue = -1, IsControl = true, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA0), UnicodeCategory = UnicodeCategory.SpaceSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA1), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA2), UnicodeCategory = UnicodeCategory.CurrencySymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA3), UnicodeCategory = UnicodeCategory.CurrencySymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA4), UnicodeCategory = UnicodeCategory.CurrencySymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA5), UnicodeCategory = UnicodeCategory.CurrencySymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA6), UnicodeCategory = UnicodeCategory.OtherSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA7), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA8), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xA9), UnicodeCategory = UnicodeCategory.OtherSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAA), UnicodeCategory = UnicodeCategory.OtherLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAB), UnicodeCategory = UnicodeCategory.InitialQuotePunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAC), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAD), UnicodeCategory = UnicodeCategory.Format, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAE), UnicodeCategory = UnicodeCategory.OtherSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xAF), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB0), UnicodeCategory = UnicodeCategory.OtherSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB1), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB2), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 2, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB3), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 3, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB4), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB5), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB6), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB7), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB8), UnicodeCategory = UnicodeCategory.ModifierSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xB9), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBA), UnicodeCategory = UnicodeCategory.OtherLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBB), UnicodeCategory = UnicodeCategory.FinalQuotePunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBC), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 0.25, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBD), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 0.5, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBE), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 0.75, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xBF), UnicodeCategory = UnicodeCategory.OtherPunctuation, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = true, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC0), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC1), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC2), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC3), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC4), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC5), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC6), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC7), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC8), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xC9), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCA), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCB), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCC), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCD), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCE), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xCF), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD0), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD1), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD2), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD3), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD4), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD5), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD6), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD7), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD8), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xD9), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDA), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDB), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDC), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDD), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDE), UnicodeCategory = UnicodeCategory.UppercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = true, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xDF), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE0), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE1), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE2), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE3), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE4), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE5), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE6), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE7), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE8), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xE9), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xEA), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xEB), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xEC), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xED), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xEE), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xEF), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF0), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF1), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF2), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF3), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF4), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF5), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF6), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF7), UnicodeCategory = UnicodeCategory.MathSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF8), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xF9), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFA), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFB), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFC), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFD), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFE), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFF), UnicodeCategory = UnicodeCategory.LowercaseLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = true, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

            // Select others

            // U+2000 EN QUAD
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2000), UnicodeCategory = UnicodeCategory.SpaceSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };

            // U+2028 LINE SEPARATOR
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2028), UnicodeCategory = UnicodeCategory.LineSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };

            // U+2029 PARAGRAPH SEPARATOR
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2029), UnicodeCategory = UnicodeCategory.ParagraphSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };

            // U+202F NARROW NO-BREAK SPACE
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x202F), UnicodeCategory = UnicodeCategory.SpaceSeparator, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = true, IsSymbol = false, IsUpper = false, IsWhiteSpace = true } };

            // U+2154 VULGAR FRACTION TWO THIRDS
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x2154), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 2.0 / 3, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

            // U+FFFD REPLACEMENT CHARACTER
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0xFFFD), UnicodeCategory = UnicodeCategory.OtherSymbol, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = true, IsUpper = false, IsWhiteSpace = false } };

            // U+10000 LINEAR B SYLLABLE B008 A
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x10000), UnicodeCategory = UnicodeCategory.OtherLetter, NumericValue = -1, IsControl = false, IsDigit = false, IsLetter = true, IsLetterOrDigit = true, IsLower = false, IsNumber = false, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

            // U+10110 AEGEAN NUMBER TEN
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x10110), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 10, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

            // U+10177 GREEK TWO THIRDS SIGN
            yield return new object[] { new UnicodeInfoTestData { ScalarValue = (Rune)(0x10177), UnicodeCategory = UnicodeCategory.OtherNumber, NumericValue = 2.0 / 3, IsControl = false, IsDigit = false, IsLetter = false, IsLetterOrDigit = false, IsLower = false, IsNumber = true, IsPunctuation = false, IsSeparator = false, IsSymbol = false, IsUpper = false, IsWhiteSpace = false } };

        }

        private static HashSet<Rune> ReadListOfWhiteSpaceScalarsFromUnicodeDataFile()
        {
            // The full list of whitespace characters can be found at:
            // https://unicode.org/cldr/utility/list-unicodeset.jsp?a=%5Cp%7Bwhitespace%7D
            // https://www.unicode.org/Public/UCD/latest/ucd/PropList.txt (top of file)

            using var reader = new StreamReader(typeof(RuneTests).Assembly.GetManifestResourceStream("PropList-12.1.0.txt"));
            HashSet<Rune> whiteSpaceRunes = new HashSet<Rune>();

            string thisLine;
            while ((thisLine = reader.ReadLine()) != null)
            {
                // Strip off the comment (starts with '#') if it exists

                int idxOfComment = thisLine.IndexOf('#');
                if (idxOfComment >= 0)
                {
                    thisLine = thisLine.Substring(0, idxOfComment);
                }

                // Split the string on the ';' separator

                string[] split = thisLine.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (split.Length != 2)
                {
                    continue; // There's no data in this line
                }

                if (split[1].Trim() != "White_Space")
                {
                    continue; // This line isn't denoting a white space scalar
                }

                string scalarAsString = split[0];

                // At this point, scalarAsString will be "XXXX" for a single scalar or "XXXX..YYYY" for a scalar range.

                int idxOfDot = scalarAsString.IndexOf("..", StringComparison.Ordinal);

                if (idxOfDot < 0)
                {
                    // Add a single scalar
                    whiteSpaceRunes.Add(new Rune(uint.Parse(scalarAsString, NumberStyles.HexNumber, CultureInfo.InvariantCulture)));
                }
                else
                {
                    // Add a range of scalars
                    uint rangeStartInclusive = uint.Parse(scalarAsString.Substring(0, idxOfDot), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    uint rangeEndInclusive = uint.Parse(scalarAsString.Substring(idxOfDot + 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    for (uint i = rangeStartInclusive; i <= rangeEndInclusive; i++)
                    {
                        whiteSpaceRunes.Add(new Rune(i));
                    }
                }
            }

            return whiteSpaceRunes;
        }

        public class UnicodeInfoTestData
        {
            // named such so that it appears at the beginning of the console output for any failing unit test
            public string __DebugDisplay => $"U+{ScalarValue.Value:X4}";

            public Rune ScalarValue;
            public UnicodeCategory UnicodeCategory;
            public double NumericValue;
            public bool IsControl;
            public bool IsDigit;
            public bool IsLetter;
            public bool IsLetterOrDigit;
            public bool IsLower;
            public bool IsNumber;
            public bool IsPunctuation;
            public bool IsSeparator;
            public bool IsSymbol;
            public bool IsUpper;
            public bool IsWhiteSpace;
        }
    }
}
