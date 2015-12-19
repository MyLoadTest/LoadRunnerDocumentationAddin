using System;
using System.Diagnostics;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("#{LineIndex} : TransactionEnd : {Name}")]
    internal sealed class ParsedTransactionEnd : ParsedTransactionBoundary
    {
        #region Constructors

        public ParsedTransactionEnd(int lineIndex, [NotNull] string name)
            : base(lineIndex, name)
        {
            // Nothing to do
        }

        #endregion
    }
}