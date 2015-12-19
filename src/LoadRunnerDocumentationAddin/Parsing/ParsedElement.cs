using System;
using System.Diagnostics;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("[{GetType().Name,nq}] @{LineIndex}")]
    internal abstract class ParsedElement
    {
        #region Constructors

        protected ParsedElement(int lineIndex)
        {
            #region Argument Check

            if (lineIndex < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(lineIndex),
                    lineIndex,
                    @"The value cannot be negative.");
            }

            #endregion

            LineIndex = lineIndex;
        }

        #endregion

        #region Public Properties

        public int LineIndex
        {
            get;
        }

        #endregion
    }
}