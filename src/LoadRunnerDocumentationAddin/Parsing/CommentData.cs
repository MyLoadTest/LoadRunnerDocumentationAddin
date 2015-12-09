using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("@{LineIndex} : {Content}")]
    internal sealed class CommentData
    {
        #region Constructors

        public CommentData(int lineIndex, [NotNull] string content)
        {
            #region Argument Check

            if (lineIndex < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lineIndex),
                    lineIndex,
                    @"The value cannot be negative.");
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            #endregion

            LineIndex = lineIndex;
            Content = content;
        }

        #endregion

        #region Public Properties

        public int LineIndex
        {
            get;
        }

        public string Content
        {
            get;
        }

        #endregion
    }
}