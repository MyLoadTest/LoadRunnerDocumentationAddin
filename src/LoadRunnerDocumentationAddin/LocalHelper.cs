using System;
using System.Diagnostics;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn
{
    internal static class LocalHelper
    {
        #region Public Methods

        public static void KillNoThrow([CanBeNull] this Process process)
        {
            try
            {
                process?.Kill();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                // Nothing to do
            }
        }

        #endregion
    }
}