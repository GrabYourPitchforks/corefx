// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace System.Text.Tests
{
    public static partial class TR29UtilityTests
    {
        private const string EmojiDataFileName = "emoji-data-12.0.txt";
        private const string GraphemeBreakPropertyFileName = "GraphemeBreakProperty-12.1.0.txt";
        private const string GraphemeBreakTestFileName = "GraphemeBreakTest-12.1.0.txt";

        private static readonly Lazy<Dictionary<uint, GraphemeClusterCategory>> _lazyGraphemeCategoryMap
            = new Lazy<Dictionary<uint, GraphemeClusterCategory>>(CreateGraphemeCategoryMap);

        private static Dictionary<uint, GraphemeClusterCategory> CreateGraphemeCategoryMap()
        {
            Dictionary<uint, GraphemeClusterCategory> map = new Dictionary<uint, GraphemeClusterCategory>();

            // First process GraphemeBreakProperty.txt

            foreach (string line in EnumerateLines(GraphemeBreakPropertyFileName))
            {
                // We're looking for lines in the form "<codepoints> ; <cluster_break_prop> # Comment"

                string[] split = line.Split('#');
                if (split.Length == 0 || string.IsNullOrWhiteSpace(split[0]))
                {
                    continue; // blank line
                }

                split = split[0].Split(';');

                string codepoints = split[0];
                GraphemeClusterCategory category = (GraphemeClusterCategory)Enum.Parse(typeof(GraphemeClusterCategory), split[1]);

                split = codepoints.Split("..");

                uint startInclusive = uint.Parse(split[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                uint endInclusive = startInclusive;

                if (split.Length > 1)
                {
                    endInclusive = uint.Parse(split[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                for (uint i = startInclusive; i <= endInclusive; i++)
                {
                    map.Add(i, category);
                }
            }

            // Then process emoji-data.txt

            foreach (string line in EnumerateLines(EmojiDataFileName))
            {
                // We're looking for lines in the form "<codepoints> ; <cluster_break_prop> # Comment"

                string[] split = line.Split('#');
                if (split.Length == 0 || string.IsNullOrWhiteSpace(split[0]))
                {
                    continue; // blank line
                }

                split = split[0].Split(';');

                if (split[1].Trim() != "Extended_Pictographic")
                {
                    continue; // not a value we care about
                }

                split = split[0].Split("..");

                uint startInclusive = uint.Parse(split[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                uint endInclusive = startInclusive;

                if (split.Length > 1)
                {
                    endInclusive = uint.Parse(split[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                for (uint i = startInclusive; i <= endInclusive; i++)
                {
                    map.Add(i, GraphemeClusterCategory.Extended_Pictographic);
                }
            }

            return map;
        }

        private static IEnumerable<string> EnumerateLines(string resourceName)
        {
            var reader = new StreamReader(typeof(TR29UtilityTests).Assembly.GetManifestResourceStream(resourceName));

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private static IEnumerable<IEnumerable<IEnumerable<Rune>>> GetGraphemeClusterBreakTestData()
        {
            // Process GraphemeBreakTest.txt

            foreach (string line in EnumerateLines(GraphemeBreakTestFileName))
            {
                // We're looking for lines in the form "<test_data> # Comment"

                string[] split = line.Split('#');
                if (split.Length == 0 || string.IsNullOrWhiteSpace(split[0]))
                {
                    continue; // blank line
                }

                // breaks are denoted in the test data by U+00F7 DIVISION SIGN
                // non-breaking separations between scalars denoted in the test data by U+00D7 MULTIPLICATION SIGN

                yield return split[0].Split('\u00F7', StringSplitOptions.RemoveEmptyEntries)
                    .Select(cluster => cluster.Split('\u00D7', StringSplitOptions.RemoveEmptyEntries)
                        .Select(value => new Rune(uint.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture))));
            }
        }

        private enum GraphemeClusterCategory
        {
            None,
            Control,
            CR,
            LF,
            L,
            V,
            LV,
            LVT,
            T,
            Extend,
            ZWJ,
            SpacingMark,
            Prepend,
            Extended_Pictographic,
            Regional_Indicator
        }

        public static IEnumerable<object[]> GraphemeClusterBreakTestData()
        {
            // Process GraphemeBreakTest.txt

            foreach (string line in EnumerateLines(GraphemeBreakTestFileName))
            {
                // We're looking for lines in the form "<test_data> # Comment"

                string[] split = line.Split('#');
                if (split.Length == 0 || string.IsNullOrWhiteSpace(split[0]))
                {
                    continue; // blank line
                }

                yield return new object[]
                {
                    split[0].Trim()
                };
            }
        }

        public class TR29UtilityTestData
        {
            // named such so that it appears at the beginning of the console output for any failing unit test
            public string __DebugDisplay => CombinedString;

            public IEnumerable<IEnumerable<Rune>> Clusters;
            public string CombinedString => string.Concat(Clusters.Select(string.Concat<Rune>));
        }
    }
}
