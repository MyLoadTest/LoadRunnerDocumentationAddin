using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn
{
    [Serializable]
    public sealed class LoadRunnerDocumentationAddinException : Exception
    {
        #region Constructors

        internal LoadRunnerDocumentationAddinException(string message)
            : base(message)
        {
            // Nothing to do
        }

        internal LoadRunnerDocumentationAddinException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing to do
        }

        private LoadRunnerDocumentationAddinException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Nothing to do
        }

        #endregion
    }
}