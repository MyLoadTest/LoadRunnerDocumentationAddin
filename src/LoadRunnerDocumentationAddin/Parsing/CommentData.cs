using System;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal sealed class CommentData
    {
        #region Constructors

        public CommentData([NotNull] string comment)
        {
            #region Argument Check

            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            #endregion

            Comment = comment;
        }

        #endregion

        #region Public Properties

        public string Comment
        {
            get;
        }

        #endregion
    }
}