using System;

namespace BingoGame.Commands
{
    public class CommandExecutedEventArgs : EventArgs
    {
        /**********************************************************************/
        #region Construction

        public CommandExecutedEventArgs(object parameter)
        {
            Parameter = parameter;
        }

        #endregion Construction

        /**********************************************************************/
        #region Properties

        public object Parameter { get; }

        #endregion Properties
    }
}
