using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal static class Parser
    {
        #region Constants and Fields

        private const int WaitTimeout = 15000;

        private const string ArgumentTemplate = @"--language C --position --hash --output ""{0}"" ""{1}""";

        private static readonly string SrcMLPath = Path.Combine(
            Path.GetDirectoryName(typeof(Parser).Assembly.GetLocalPath()).EnsureNotNull(),
            @"lib\srcML\srcml.exe");

        private static readonly string SrcMLDirectory = Path.GetDirectoryName(SrcMLPath).EnsureNotNull();

        #endregion

        #region Public Methods

        public static ParsedFileData[] ParseFiles(ICollection<string> filePaths)
        {
            #region Argument Check

            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (filePaths.Any(item => item.IsNullOrEmpty()))
            {
                throw new ArgumentException(@"The collection contains a null or empty element.", nameof(filePaths));
            }

            #endregion

            var outputFilePath = Path.GetTempFileName();

            var arguments = string.Format(ArgumentTemplate, outputFilePath, filePaths.Join(@""" """));
            var startInfo = new ProcessStartInfo(SrcMLPath, arguments)
            {
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = SrcMLDirectory
            };

            var process = Process.Start(startInfo).EnsureNotNull();

            var waitResult = process.WaitForExit(WaitTimeout);
            if (!waitResult)
            {
                process.KillNoThrow();

                throw new LoadRunnerDocumentationAddinException(
                    "Timeout has expired prior to the parsing of the source files has finished.");
            }

            var exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                throw new LoadRunnerDocumentationAddinException(
                    $@"Parsing of the source files has finished with the error code {exitCode}.");
            }

            XDocument xDocument;
            using (var fileStream = File.Open(outputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                xDocument = XDocument.Load(fileStream, LoadOptions.PreserveWhitespace);
                Trace.WriteLine(xDocument.Root);
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}