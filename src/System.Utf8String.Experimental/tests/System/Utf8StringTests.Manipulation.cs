// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Tests;
using Xunit;

using static System.Tests.Utf8TestUtilities;

namespace System.Tests
{
    public unsafe partial class Utf8StringTests
    {
        public static IEnumerable<object[]> Trim_TestData() => Utf8SpanTests.Trim_TestData();

        [Theory]
        [MemberData(nameof(Trim_TestData))]
        public static void Trim(string input)
        {
            if (input is null)
            {
                return; // don't want to null ref
            }

            Utf8String utf8Input = u8(input);

            Utf8String utf8TrimAll = utf8Input.Trim();
            Utf8String utf8TrimEnd = utf8Input.TrimEnd();
            Utf8String utf8TrimStart = utf8Input.TrimStart();

            Assert.True(Utf8String.AreEquivalent(utf8TrimAll, input.Trim()));
            Assert.True(Utf8String.AreEquivalent(utf8TrimEnd, input.TrimEnd()));
            Assert.True(Utf8String.AreEquivalent(utf8TrimStart, input.TrimStart()));

            if (utf8TrimAll.Length == utf8Input.Length)
            {
                Assert.Same(utf8Input, utf8TrimAll); // Shouldn't return new object if we didn't trim anything
            }

            if (utf8TrimEnd.Length == utf8Input.Length)
            {
                Assert.Same(utf8Input, utf8TrimEnd); // Shouldn't return new object if we didn't trim anything
            }

            if (utf8TrimStart.Length == utf8Input.Length)
            {
                Assert.Same(utf8Input, utf8TrimStart); // Shouldn't return new object if we didn't trim anything
            }
        }
    }
}
