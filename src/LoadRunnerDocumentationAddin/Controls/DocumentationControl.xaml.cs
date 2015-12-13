using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Controls
{
    public sealed partial class DocumentationControl
    {
        #region Constructors

        public DocumentationControl()
        {
            InitializeComponent();

            DocumentationWebBrowser.Loaded += (sender, args) => RefreshDocumentation();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        #endregion

        #region Private Methods

        private void RefreshDocumentationInternal()
        {
            var htmlBody = ViewModel.DocumentationHtmlContent;

            var htmlContent =
                $@"
            <html>
                <head>
                    <meta http-equiv=""Content-Type"" content=""text/html;charset=UTF-8""/>
                    <style type=""text/css"">{
                    Properties.Resources.DefaultDocumentationCssStyle
                    }</style>
                </head>
                <body oncontextmenu=""return false;"">
                    {
                    htmlBody}
                </body>
            </html>";

            DocumentationWebBrowser.NavigateToString(htmlContent);
        }

        private void RefreshDocumentation()
        {
            if (Dispatcher.CheckAccess())
            {
                RefreshDocumentationInternal();
            }
            else
            {
                Dispatcher.BeginInvoke((Action)RefreshDocumentationInternal, DispatcherPriority.Render);
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ViewModel.DocumentationHtmlContent))
            {
                RefreshDocumentation();
            }
        }

        #endregion
    }
}