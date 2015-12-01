using System;
using System.Linq;
using Omnifactotum.Annotations;

namespace MyLoadTest.LoadRunnerDocumentation.AddIn.Commands
{
    internal sealed class AsyncRelayCommand : RelayCommandBase
    {
        #region Constructors

        public AsyncRelayCommand([NotNull] Action<object> execute, [CanBeNull] Func<object, bool> canExecute)
            : base(execute, canExecute)
        {
            // Nothing to do
        }

        public AsyncRelayCommand([NotNull] Action<object> execute)
            : this(execute, null)
        {
            // Nothing to do
        }

        #endregion

        #region Public Properties

        public bool IsExecuting => IsExecutingInternal;

        #endregion

        #region Public Methods

        public override void Execute(object parameter)
        {
            ExecuteInternal(parameter, true);
        }

        #endregion
    }
}