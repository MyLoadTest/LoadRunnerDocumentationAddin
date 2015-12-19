using System;
using System.Diagnostics;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal abstract class ParsedTransactionBoundary : ParsedElement
    {
        #region Constructors

        protected ParsedTransactionBoundary(int lineIndex, [NotNull] string name)
            : base(lineIndex)
        {
            #region Argument Check

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            #endregion

            Name = name;
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get;
        }

        #endregion

        #region Public Methods

        public static ParsedTransactionBoundary Create(bool isStart, int lineIndex, [NotNull] string name)
        {
            return isStart
                ? (ParsedTransactionBoundary)new ParsedTransactionStart(lineIndex, name)
                : new ParsedTransactionEnd(lineIndex, name);
        }

        #endregion
    }
}