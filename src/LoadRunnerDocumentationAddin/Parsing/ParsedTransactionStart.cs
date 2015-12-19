using System;
using System.Diagnostics;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay("#{LineIndex} : TransactionStart : {Name}")]
    internal sealed class ParsedTransactionStart : ParsedTransactionBoundary
    {
        #region Constructors

        public ParsedTransactionStart(int lineIndex, [NotNull] string name)
            : base(lineIndex, name)
        {
            // Nothing to do
        }

        #endregion
    }
}