using System;
using System.Diagnostics;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("@{LineIndex}: {Content}")]
    internal sealed class CommentData
    {
        #region Constructors

        public CommentData([NotNull] string content, int? lineIndex)
        {
            #region Argument Check

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            #endregion

            Content = content;
            LineIndex = lineIndex;
        }

        #endregion

        #region Public Properties

        public string Content
        {
            get;
        }

        public int? LineIndex
        {
            get;
        }

        #endregion
    }
}