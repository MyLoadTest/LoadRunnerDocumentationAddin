using System;
using System.Diagnostics;
using System.Linq;
using ICSharpCode.SharpDevelop.Gui;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Controls;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Pads
{
    public sealed class DocumentationPad : AbstractPadContent
    {
        #region Constants and Fields

        private readonly DocumentationControl _innerControl;

        #endregion

        #region Constructors

        public DocumentationPad()
        {
            _innerControl = new DocumentationControl();
        }

        #endregion

        #region Public Properties

        public override object Control
        {
            [DebuggerStepThrough]
            get
            {
                return _innerControl;
            }
        }

        #endregion
    }
}