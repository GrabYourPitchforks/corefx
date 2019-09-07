// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using System.Linq;

using static System.Tests.Utf8TestUtilities;

using ustring = System.Utf8String;

namespace System.Text.Tests
{
    public unsafe partial class Utf8SpanTests
    {
        /// <summary>
        /// All <see cref="Rune"/>s, U+0000..U+D800 and U+E000..U+10FFFF.
        /// </summary>
        private static IEnumerable<Rune> AllRunes
        {
            get
            {
                for (uint i = 0; i < 0xD800; i++)
                {
                    yield return new Rune(i);
                }
                for (uint i = 0xE000; i <= 0x10FFFF; i++)
                {
                    yield return new Rune(i);
                }
            }
        }

        /// <summary>
        /// All <see cref="Rune"/>s where <see cref="Rune.IsWhiteSpace(Rune)"/> returns <see langword="true"/>.
        /// </summary>
        private static readonly Lazy<Rune[]> WhiteSpaceRunes = new Lazy<Rune[]>(() => AllRunes.Where(Rune.IsWhiteSpace).ToArray());

        public static IEnumerable<object[]> Trim_TestData()
        {
            string[] testData = new string[]
            {
                null, // null
                "", // empty
                "\0", // contains null character - shouldn't be trimmed
                "Hello", // simple non-whitespace ASCII data
                "\u0009Hello\u000d", // C0 whitespace characters
                "\u0009\u0008\u0009Hello\u000e\u000b", // C0 whitespace + non-whitespace characters
                " Hello! ", // simple space chars (plus !, since it's adjacent to U+0020 SPACE)
                "\u0085\u0084\u0086\u0085", // U+0085 NEXT LINE (NEL), surrounded by adjacent non-whitespace chars
            };

            foreach (string entry in testData)
            {
                yield return new object[] { entry };
            }

            // A string with every possible whitespace character, just to test the limits

            StringBuilder builder = new StringBuilder();
            foreach (Rune whitespaceRune in WhiteSpaceRunes.Value)
            {
                builder.Append(whitespaceRune);
            }
            builder.Append("xyz");
            foreach (Rune whitespaceRune in WhiteSpaceRunes.Value)
            {
                builder.Append(whitespaceRune);
            }

            yield return new object[] { builder.ToString() };
        }
    }
}
