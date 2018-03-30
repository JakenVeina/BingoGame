using System;
using System.Windows.Input;

namespace BingoGame.Commands
{
    public interface ICommandProvider
    {
        /**********************************************************************/
        #region Properties

        ICommand Command { get; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        void RaiseCommandCanExecuteChanged();

        #endregion Methods

        /**********************************************************************/
        #region Events

        event EventHandler<CommandExecutedEventArgs> CommandExecuted;

        event EventHandler<CommandTestedEventArgs> CommandTested;

        #endregion Events
    }
}
