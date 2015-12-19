using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Omnifactotum;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing
{
    internal static class Parser
    {
        #region Constants and Fields

        private const int WaitTimeout = 15000;

        private const string ArgumentTemplate = @"--language C --position --hash --archive --output ""{0}"" ""{1}""";

        private static readonly string SrcMLPath = Path.Combine(
            Path.GetDirectoryName(typeof(Parser).Assembly.GetLocalPath()).EnsureNotNull(),
            @"lib\srcML\srcml.exe");

        private static readonly string SrcMLDirectory = Path.GetDirectoryName(SrcMLPath).EnsureNotNull();

        #endregion

        #region Public Methods

        public static ParsedFile[] ParseFiles(ICollection<string> filePaths)
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
                CreateNoWindow = true,
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

            XDocument document;
            using (var fileStream = File.Open(outputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                document = XDocument.Load(fileStream, LoadOptions.PreserveWhitespace);
            }

            File.Delete(outputFilePath);

            var unitElements = document
                .Root?
                .Elements(ParsingConstants.Element.Unit)
                .Where(element => element.Attribute(ParsingConstants.Attribute.FileName) != null)
                .ToArray()
                ?? new XElement[0];

            var resultList = new List<ParsedFile>(unitElements.Length);

            //// ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var unitElement in unitElements)
            {
                var fileName = unitElement.Attribute(ParsingConstants.Attribute.FileName).EnsureNotNull().Value;
                var hash = unitElement.Attribute(ParsingConstants.Attribute.Hash).EnsureNotNull().Value;

                var elements = new List<ParsedElement>();

                ParseDocComments(unitElement, elements);
                ParseTransactionBoundaries(unitElement, elements);

                var parsedFile = new ParsedFile(fileName, hash, elements);
                resultList.Add(parsedFile);
            }

            return resultList.ToArray();
        }

        private static void ParseDocComments(XContainer unitElement, ICollection<ParsedElement> elements)
        {
            var commentElements = unitElement.Descendants(ParsingConstants.Element.Comment).ToArray();

            var commentDatas = commentElements
                .Where(
                    element =>
                        element.Attribute(ParsingConstants.Attribute.Type)?.Value
                            == ParsingConstants.SingleLineCommentType)
                .Select(
                    element => new
                    {
                        Match = ParsingConstants.DocCommentRegex.Match(element.Value),
                        LineIndex = element.GetLineIndex()
                    })
                .Where(obj => obj.Match.Success)
                .Select(
                    obj =>
                        new
                        {
                            obj.LineIndex,
                            Content = obj.Match.Groups[ParsingConstants.SoleRegexGroupName].Value
                        })
                .OrderBy(obj => obj.LineIndex)
                .ToArray();

            var contentBuilder = new StringBuilder();
            var startLineIndex = ValueContainer.Create(int.MinValue);

            Action addData =
                () =>
                {
                    if (startLineIndex.Value >= 0 && contentBuilder.Length != 0)
                    {
                        var commentData = new ParsedComment(startLineIndex.Value, contentBuilder.ToString());
                        elements.Add(commentData);
                    }

                    contentBuilder.Clear();
                };

            for (var index = 0; index < commentDatas.Length; index++)
            {
                var rawCommentData = commentDatas[index];

                if (index == 0 || rawCommentData.LineIndex != commentDatas[index - 1].LineIndex + 1)
                {
                    addData();

                    startLineIndex.Value = rawCommentData.LineIndex;
                }

                if (contentBuilder.Length != 0)
                {
                    contentBuilder.AppendLine();
                }

                contentBuilder.Append(rawCommentData.Content);
            }

            addData();
        }

        private static void ParseTransactionBoundaries(XContainer unitElement, List<ParsedElement> elements)
        {
            var callNameElements = unitElement
                .Descendants(ParsingConstants.Element.ExpressionStatement)
                .Descendants(ParsingConstants.Element.Expression)
                .Descendants(ParsingConstants.Element.Call)
                .Descendants(ParsingConstants.Element.Name)
                .ToArray();

            var transactionBoundaries = callNameElements
                .Where(
                    obj =>
                        obj.Value == ParsingConstants.TransactionStartFunctionName
                            || obj.Value == ParsingConstants.TransactionEndFunctionName)
                .Select(
                    obj => new
                    {
                        LineIndex = obj.GetLineIndex(),
                        IsStart = obj.Value == ParsingConstants.TransactionStartFunctionName,
                        Name = obj
                            .Parent?
                            .Element(ParsingConstants.Element.ArgumentList)?
                            .Element(ParsingConstants.Element.Argument)?
                            .Element(ParsingConstants.Element.Expression)?
                            .Element(ParsingConstants.Element.Literal)?
                            .GetLiteralStringValue()
                    })
                .Where(obj => !obj.Name.IsNullOrEmpty())
                .Select(obj => ParsedTransactionBoundary.Create(obj.IsStart, obj.LineIndex, obj.Name))
                .ToArray();

            elements.AddRange(transactionBoundaries);
        }

        #endregion
    }
}