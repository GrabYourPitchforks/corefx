// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Xunit;

using static System.Tests.Utf8TestUtilities;
using ustring = System.Utf8String;

namespace System.Tests
{
    public partial class Utf8StringFactoryTests
    {
        [Fact]
        public static void CreateFromFile_NullOrEmptyParams_Throws()
        {
            // Try null

            static void AssertThrowsForNull(Func<string, ustring> factory)
            {
                Assert.Throws<ArgumentNullException>("path", () => factory(null));
            }

            AssertThrowsForNull(Utf8StringFactory.CreateFromFile);
            AssertThrowsForNull(Utf8StringFactory.CreateFromFileRelaxed);
            AssertThrowsForNull(Utf8StringFactory.UnsafeCreateFromFileWithoutValidation);

            // Try empty

            static void AssertThrowsForEmpty(Func<string, ustring> factory)
            {
                // TODO_UTF8STRING: This logic should be updated to handle the appropriate exception.
                Assert.Throws<ArgumentNullException>("path", () => factory(string.Empty));
            }

            AssertThrowsForEmpty(Utf8StringFactory.CreateFromFile);
            AssertThrowsForEmpty(Utf8StringFactory.CreateFromFileRelaxed);
            AssertThrowsForEmpty(Utf8StringFactory.UnsafeCreateFromFileWithoutValidation);
        }

        [Fact]
        public static void CreateFromFile_HandlesByteOrderMarks()
        {
            RunTest(Utf8StringFactory.CreateFromFile);
            RunTest(Utf8StringFactory.CreateFromFileRelaxed);
            RunTest(Utf8StringFactory.UnsafeCreateFromFileWithoutValidation);

            static void RunTest(Func<string, ustring> factory)
            {
                // First try an empty file.

                RunFileTest(factory, actualFileContents: new byte[0], expectedUtf8StringContents: new byte[0]);

                // Then try a file with BOM-only.

                RunFileTest(factory, actualFileContents: new byte[] { 0xEF, 0xBB, 0xBF }, expectedUtf8StringContents: new byte[0]);

                // Then a file with BOM + some additional data.

                RunFileTest(factory, actualFileContents: new byte[] { 0xEF, 0xBB, 0xBF, 0xC2, 0x80 }, expectedUtf8StringContents: u8("\u0080").ToByteArray());

                // Then something that's not a UTF-8 BOM.

                RunFileTest(factory, actualFileContents: new byte[] { 0xEF, 0xBB, 0xBE }, expectedUtf8StringContents: u8("\ufefe").ToByteArray());

                // Then 1-byte and 2-byte files.

                RunFileTest(factory, actualFileContents: new byte[] { 0x61 }, expectedUtf8StringContents: new byte[] { 0x61 });
                RunFileTest(factory, actualFileContents: new byte[] { 0xC2, 0x80 }, expectedUtf8StringContents: new byte[] { 0xC2, 0x80 });
            }
        }

        [Fact]
        public static void CreateFromFile_ThrowsOnBadData()
        {
            RunFileTest(Utf8StringFactory.CreateFromFile, new byte[] { 0xC1, 0x80 }, expectedUtf8StringContents: null);
        }

        [Fact]
        public static void CreateFromFileRelaxed_ReplacesBadData()
        {
            RunFileTest(Utf8StringFactory.CreateFromFileRelaxed, new byte[] { 0x61, 0xC1, 0x80 }, expectedUtf8StringContents: u8("a\ufffd\ufffd").ToByteArray());
        }

        private static void RunFileTest(Func<string, ustring> factory, byte[] actualFileContents, byte[] expectedUtf8StringContents)
        {
            string tempFileName = Path.GetTempFileName();

            try
            {
                File.WriteAllBytes(tempFileName, actualFileContents);

                if (expectedUtf8StringContents is null)
                {
                    // We expect an exception due to malformed file contents.
                    Assert.Throws<ArgumentException>(() => factory(tempFileName));
                }
                else
                {
                    // We expect a ustring with the specified contents.
                    ustring ustr = factory(tempFileName);
                    Assert.Equal(expectedUtf8StringContents, ustr.ToByteArray());
                }
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }
    }
}
