using System;
using System.Diagnostics;
using System.Linq;
using ICSharpCode.Core;
using MyLoadTest.LoadRunnerDocumentation.AddIn.Properties;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.HostCommands
{
    /// <summary>
    ///     Represents the command that is executed automatically when IDE starts.
    /// </summary>
    public sealed class StartupCommand : AbstractMenuCommand
    {
        #region Public Methods

        /// <summary>
        ///     Invokes the command.
        /// </summary>
        public override void Run()
        {
            ResourceService.RegisterNeutralImages(Resources.ResourceManager);
        }

        #endregion
    }
}