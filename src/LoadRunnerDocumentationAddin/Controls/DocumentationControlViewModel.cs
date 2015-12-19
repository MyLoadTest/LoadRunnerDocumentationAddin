using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using CommonMark;
using CommonMark.Formatters;
using HP.LR.Vugen.BackEnd.Project.ProjectSystem;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.Utt.ProjectSystem;
using HP.Utt.ProjectSystem.ScriptItems;
using ICSharpCode.SharpDevelop.Project;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Commands;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Properties;
using Omnifactotum;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Controls
{
    internal sealed class DocumentationControlViewModel : INotifyPropertyChanged
    {
        #region Constants and Fields

        private readonly Dispatcher _dispatcher;
        private string _documentationHtmlContent;

        #endregion

        #region Constructors

        public DocumentationControlViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher.EnsureNotNull();

            RefreshCommand = new AsyncRelayCommand(ExecuteRefreshCommand);
            ExportToPdfCommand = new RelayCommand(ExecuteExportToPdfCommand);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public AsyncRelayCommand RefreshCommand
        {
            get;
        }

        public RelayCommand ExportToPdfCommand
        {
            get;
        }

        public string DocumentationHtmlContent
        {
            [DebuggerStepThrough]
            get
            {
                return _documentationHtmlContent;
            }

            private set
            {
                if (value == _documentationHtmlContent)
                {
                    return;
                }

                _documentationHtmlContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private Methods

        private static void ShowErrorMessageBoxInternal(string text)
        {
            var mainWindow = Application.Current?.MainWindow;

            if (mainWindow == null)
            {
                MessageBox.Show(
                    text,
                    Resources.AddInTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(
                    mainWindow,
                    text,
                    Resources.AddInTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowErrorMessageBox(string text)
        {
            if (_dispatcher.CheckAccess())
            {
                ShowErrorMessageBoxInternal(text);
            }
            else
            {
                _dispatcher.Invoke(() => ShowErrorMessageBoxInternal(text), DispatcherPriority.Send);
            }
        }

        private void ExecuteRefreshCommand(object parameter)
        {
            try
            {
                ExecuteRefreshCommandInternal();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                ShowErrorMessageBox($@"Error occurred: {ex.Message}");
            }
        }

        private void ExecuteRefreshCommandInternal()
        {
            DocumentationHtmlContent = null;

            IUttBaseScriptItem scriptItem = (ProjectService.CurrentProject as VuGenProject)?.Script;

            var actionFilePaths = new List<string>();

            Factotum.ProcessRecursively(
                scriptItem,
                item =>
                    //// ReSharper disable once RedundantEnumerableCastCall - False detection
                    (item as IUttCollectionScriptItem<IUttBaseScriptItem>)?.ScriptItems
                        ?? (item as IUttCollectionScriptItem<IActionScriptItem>)?.ScriptItems
                            .Cast<IUttBaseScriptItem>(),
                item =>
                {
                    var actionScriptItem = item as IActionScriptItem;
                    if (actionScriptItem != null)
                    {
                        actionFilePaths.Add(actionScriptItem.FullFileName);
                    }
                });

            var parsedFiles = Parser.ParseFiles(actionFilePaths);

            var content = parsedFiles
                .SelectMany(obj => obj.Elements)
                .OfType<ParsedComment>()
                .Select(obj => obj.Content)
                .Join(Environment.NewLine + Environment.NewLine);

            var settings = CommonMarkSettings.Default.Clone();
            settings.AdditionalFeatures = CommonMarkAdditionalFeatures.All;
            settings.RenderSoftLineBreaksAsLineBreaks = true;

            var document = CommonMarkConverter.Parse(content, settings);

            ////var htmlContent = CommonMarkConverter.Convert(content, settings);
            ////Trace.WriteLine(htmlContent);

            using (var stringWriter = new StringWriter())
            {
                var formatter = new HtmlFormatter(stringWriter, settings);
                formatter.WriteDocument(document);

                DocumentationHtmlContent = stringWriter.ToString();
            }
        }

        private void ExecuteExportToPdfCommand(object parameter)
        {
            try
            {
                ExecuteExportToPdfCommandInternal();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                ShowErrorMessageBox($@"Error occurred: {ex.Message}");
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void ExecuteExportToPdfCommandInternal()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}