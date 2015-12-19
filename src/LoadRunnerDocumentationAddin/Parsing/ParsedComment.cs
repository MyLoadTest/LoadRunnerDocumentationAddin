using System;
using System.Diagnostics;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("@{LineIndex} : {Content}")]
    internal sealed class ParsedComment : ParsedElement
    {
        #region Constructors

        public ParsedComment(int lineIndex, [NotNull] string content)
            : base(lineIndex)
        {
            #region Argument Check

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            #endregion

            Content = content;
        }

        #endregion

        #region Public Properties

        public string Content
        {
            get;
        }

        #endregion
    }
}