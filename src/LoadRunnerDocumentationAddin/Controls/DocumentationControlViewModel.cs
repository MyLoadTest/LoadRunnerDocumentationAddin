using System;
using System.Linq;
using System.Windows;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Commands;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Properties;

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