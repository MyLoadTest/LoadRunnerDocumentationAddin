using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    [DebuggerDisplay(@"\{ {FilePath} : {Elements.Count} : {Hash} \}")]
    internal sealed class ParsedFile
    {
        #region Constructors

        public ParsedFile(string filePath, string hash, ICollection<ParsedElement> elements)
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

            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (elements.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(elements));
            }

            #endregion

            FilePath = filePath;
            Hash = hash;
            Elements = elements.ToArray().AsReadOnly();
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

        public ReadOnlyCollection<ParsedElement> Elements
        {
            get;
        }

        #endregion
    }
}