using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal static class ParsingConstants
    {
        #region Constants and Fields

        public const string SoleRegexGroupName = "Group";
        public const string StringLiteralType = "string";
        public const string ArgumentSeparator = ",";

        public const string SingleLineCommentType = "line";
        public const string DocCommentPrefix = "///";

        public static readonly Regex StringLiteralValueRegex = new Regex(
            $@"^ \"" (?<{SoleRegexGroupName}>.*) \"" $",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled);

        public static readonly Regex DocCommentRegex = new Regex(
            $@"^ {DocCommentPrefix} \s* (?<{SoleRegexGroupName}>.*) $",
            RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled);

        #endregion

        #region Namespace Class

        public static class Namespace
        {
            public static readonly XNamespace Default = XNamespace.Get("http://www.srcML.org/srcML/src");
            public static readonly XNamespace CPlusPlus = XNamespace.Get("http://www.srcML.org/srcML/cpp");
            public static readonly XNamespace Literal = XNamespace.Get("http://www.srcML.org/srcML/literal");
            public static readonly XNamespace Position = XNamespace.Get("http://www.srcML.org/srcML/position");
            public static readonly XNamespace Operator = XNamespace.Get("http://www.srcML.org/srcML/operator");
            public static readonly XNamespace Modifier = XNamespace.Get("http://www.srcML.org/srcML/modifier");
            public static readonly XNamespace SourceError = XNamespace.Get("http://www.srcML.org/srcML/srcerr");

            public static readonly XmlNamespaceManager NamespaceManager = CreateNamespaceManager();

            private static XmlNamespaceManager CreateNamespaceManager()
            {
                var result = new XmlNamespaceManager(new NameTable());

                result.AddNamespace(string.Empty, Default.NamespaceName);
                result.AddNamespace("cpp", CPlusPlus.NamespaceName);
                result.AddNamespace("lit", Literal.NamespaceName);
                result.AddNamespace("op", Operator.NamespaceName);
                result.AddNamespace("type", Modifier.NamespaceName);
                result.AddNamespace("pos", Position.NamespaceName);
                result.AddNamespace("err", SourceError.NamespaceName);

                return result;
            }
        }

        #endregion

        #region Element Class

        public static class Element
        {
            public static readonly XName Unit = Namespace.Default + "unit";
            public static readonly XName Comment = Namespace.Default + "comment";
            public static readonly XName ArgumentList = Namespace.Default + "argument_list";
            public static readonly XName Argument = Namespace.Default + "argument";
            public static readonly XName ExpressionStatement = Namespace.Default + "expr_stmt";
            public static readonly XName Expression = Namespace.Default + "expr";
            public static readonly XName Call = Namespace.Default + "call";
            public static readonly XName Name = Namespace.Default + "name";
            public static readonly XName Literal = Namespace.Literal + "literal";
        }

        #endregion

        #region Attribute Class

        public static class Attribute
        {
            public static readonly XName Type = XNamespace.None + "type";
            public static readonly XName Language = XNamespace.None + "language";
            public static readonly XName FileName = XNamespace.None + "filename";
            public static readonly XName Hash = XNamespace.None + "hash";

            #region Position Class

            public static class Position
            {
                public static readonly XName Line = Namespace.Position + "line";
                public static readonly XName Column = Namespace.Position + "column";
                public static readonly XName Tabs = Namespace.Position + "tabs";
            }

            #endregion
        }

        #endregion
    }
}