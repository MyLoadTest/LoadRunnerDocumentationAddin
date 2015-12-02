using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using HP.LR.Vugen.BackEnd.Project.ProjectSystem;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.Utt.ProjectSystem;
using HP.Utt.ProjectSystem.ScriptItems;
using ICSharpCode.SharpDevelop.Project;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Commands;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Parsing;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Properties;
using Omnifactotum;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Controls
{
    internal sealed class DocumentationControlViewModel
    {
        #region Constructors

        public DocumentationControlViewModel()
        {
            RefreshCommand = new AsyncRelayCommand(ExecuteRefreshCommand);
            ExportToPdfCommand = new RelayCommand(ExecuteExportToPdfCommand);
        }

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

        #endregion

        #region Private Methods

        private void ExecuteRefreshCommand(object parameter)
        {
            try
            {
                ExecuteRefreshCommandInternal();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                MessageBox.Show(
                    $@"Error occurred: {ex.Message}",
                    Resources.AddInTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void ExecuteRefreshCommandInternal()
        {
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

            var parsedFileDatas = Parser.ParseFiles(actionFilePaths);
            Trace.WriteLine(parsedFileDatas);

            throw new NotImplementedException();
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
                MessageBox.Show(
                    $@"Error occurred: {ex.Message}",
                    Resources.AddInTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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