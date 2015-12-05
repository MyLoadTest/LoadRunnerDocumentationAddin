using System;
using System.Globalization;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal static class ParsingHelper
    {
        #region Public Methods

        public static int? TryParseInt([CanBeNull] this string value)
        {
            int result;

            return value != null
                && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)
                ? result
                : default(int?);
        }

        public static int ParseInt([CanBeNull] this string value)
        {
            return TryParseInt(value).EnsureNotNull();
        }

        #endregion
    }
}