using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal sealed class ParsedFileData
    {
        #region Constructors

        public ParsedFileData(string filePath, string hash, ICollection<CommentData> comments)
        {
            #region Argument Check

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    @"The value can be neither empty nor whitespace-only string nor null.",
                    nameof(filePath));
            }

            if (string.IsNullOrWhiteSpace(hash))
            {
                throw new ArgumentException(
                    @"The value can be neither empty nor whitespace-only string nor null.",
                    nameof(hash));
            }

            if (comments == null)
            {
                throw new ArgumentNullException(nameof(comments));
            }

            if (comments.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(comments));
            }

            #endregion

            FilePath = filePath;
            Hash = hash;
            Comments = comments.ToArray().AsReadOnly();
        }

        #endregion

        #region Public Properties

        public string FilePath
        {
            get;
        }

        public string Hash
        {
            get;
        }

        public ReadOnlyCollection<CommentData> Comments
        {
            get;
        }

        #endregion
    }
}