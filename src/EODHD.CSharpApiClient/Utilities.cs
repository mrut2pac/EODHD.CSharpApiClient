using System;
using System.Globalization;

namespace EODHD.CSharpApiClient
{
    /// <summary>
    /// Internal parsing helpers shared by the data-model classes for converting raw EODHD wire strings
    /// (dates and split ratios) into typed values.
    /// </summary>
    internal static class Utilities
    {
        private static readonly char[] SplitFactorSeparators = new char[] { ':', '/' };

        /// <summary>
        /// Parses an EODHD date string into a <see cref="DateTime"/>, returning <c>null</c> when the
        /// value is missing or not a valid date.
        /// </summary>
        public static DateTime? ParseDate(string dateStr)
        {
            if(DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }

            return null;
        }

        /// <summary>
        /// Parses an EODHD split ratio string (e.g. <c>"2.000000/1.000000"</c> or <c>"2:1"</c>) into the
        /// old-to-new share factor, returning <c>null</c> when it is missing or invalid.
        /// </summary>
        public static double? ParseSplitFactor(string splitFactorStr)
        {
            if(splitFactorStr == null)
            {
                return null;
            }

            int dividerIndex = splitFactorStr.IndexOfAny(SplitFactorSeparators);
            if(dividerIndex == -1)
            {
                return null;
            }

            try
            {
                double newShares = double.Parse(splitFactorStr.AsSpan(0, dividerIndex), provider: CultureInfo.InvariantCulture);
                double oldShares = double.Parse(splitFactorStr.AsSpan(dividerIndex + 1), provider: CultureInfo.InvariantCulture);

                // Guard against malformed ratios such as "0:1".
                if(newShares != 0)
                {
                    return oldShares / newShares;
                }
            }
            catch(FormatException)
            {
                // Not a parseable ratio.
            }

            return null;
        }
    }
}
