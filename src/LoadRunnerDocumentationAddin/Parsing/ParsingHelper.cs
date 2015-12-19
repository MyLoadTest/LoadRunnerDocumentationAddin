using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
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

        public static int GetLineIndex([NotNull] this XElement element)
        {
            #region Argument Check

            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            #endregion

            return element
                .Attribute(ParsingConstants.Attribute.Position.Line)
                .EnsureNotNull()
                .Value
                .ParseInt();
        }

        public static string GetLiteralStringValue([CanBeNull] this XElement element)
        {
            if (element == null)
            {
                return null;
            }

            #region Argument Check

            if (element.Name != ParsingConstants.Element.Literal)
            {
                throw new ArgumentException(
                    $@"The element must be '{ParsingConstants.Element.Literal.LocalName}'.",
                    nameof(element));
            }

            if (element.Attribute(ParsingConstants.Attribute.Type)?.Value != ParsingConstants.StringLiteralType)
            {
                throw new ArgumentException(
                    $@"The '{ParsingConstants.Element.Literal.LocalName}' element must be of type '{
                        ParsingConstants.StringLiteralType}'.",
                    nameof(element));
            }

            #endregion

            var match = ParsingConstants.StringLiteralValueRegex.Match(element.Value);
            return match.Success ? match.Groups[ParsingConstants.SoleRegexGroupName].Value : null;
        }

        #endregion
    }
}