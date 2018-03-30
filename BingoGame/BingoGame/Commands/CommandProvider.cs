using System;
using System.Windows.Input;

namespace BingoGame.Commands
{
    public class CommandProvider : ICommandProvider
    {
        /**********************************************************************/
        #region Construction

        public CommandProvider()
        {
            Command = new CommandProviderRelay(this);
        }

        #endregion Construction

        /**********************************************************************/
        #region ICommandProvider

        public ICommand Command { get; }

        public void RaiseCommandCanExecuteChanged()
            => CommandCanExecuteChanged?.Invoke(Command, EventArgs.Empty);

        public event EventHandler<CommandExecutedEventArgs> CommandExecuted;

        public event EventHandler<CommandTestedEventArgs> CommandTested;

        #endregion Events

        /**********************************************************************/
        #region Private Methods

        private void CommandExecute(object parameter)
            => CommandExecuted?.Invoke(this, new CommandExecutedEventArgs(parameter));

        private bool CommandCanExecute(object parameter)
        {
            if (CommandTested == null)
                return (CommandExecuted != null);

            var e = new CommandTestedEventArgs(parameter)
            {
                CanExecute = false
            };

            CommandTested.Invoke(this, e);

            return e.CanExecute;
        }

        private event EventHandler CommandCanExecuteChanged;

        #endregion Private Methods

        /**********************************************************************/
        #region Private Types

        private class CommandProviderRelay : ICommand
        {
            /******************************************************************/
            #region Construction

            public CommandProviderRelay(CommandProvider owner)
            {
                _owner = owner;
            }

            #endregion Construction

            /******************************************************************/
            #region ICommand

            public void Execute(object parameter)
                => _owner.CommandExecute(parameter);

            public bool CanExecute(object parameter)
                => _owner.CommandCanExecute(parameter);

            public event EventHandler CanExecuteChanged
            {
                add => _owner.CommandCanExecuteChanged += value;
                remove => _owner.CommandCanExecuteChanged -= value;
            }

            #endregion ICommand

            /******************************************************************/
            #region Private Fields

            private readonly CommandProvider _owner;

            #endregion Private Fields
        }

        #endregion Private Types
    }
}
