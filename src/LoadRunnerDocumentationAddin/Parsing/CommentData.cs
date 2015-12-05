using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("@{StartLineIndex}-{EndLineIndex} : {Lines[0]} ...")]
    internal sealed class CommentData
    {
        #region Constructors

        public CommentData(int startLineIndex, [NotNull] ICollection<string> lines)
        {
            #region Argument Check

            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            if (lines.Count == 0)
            {
                throw new ArgumentException(@"The collection is empty.", nameof(lines));
            }

            if (lines.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(lines));
            }

            #endregion

            Lines = lines.ToArray().AsReadOnly();
            StartLineIndex = startLineIndex;
            EndLineIndex = startLineIndex + lines.Count - 1;
        }

        #endregion

        #region Public Properties

        public ReadOnlyCollection<string> Lines
        {
            get;
        }

        public int StartLineIndex
        {
            get;
        }

        public int EndLineIndex
        {
            get;
        }

        #endregion
    }
}