namespace BingoGame.Commands
{
    public class CommandTestedEventArgs : CommandExecutedEventArgs
    {
        /**********************************************************************/
        #region Construction

        public CommandTestedEventArgs(object parameter)
            : base(parameter) { }

        #endregion Construction

        /**********************************************************************/
        #region Properties

        public bool CanExecute { get; set; }

        #endregion Properties
    }
}
