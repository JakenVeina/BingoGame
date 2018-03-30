using System.Windows.Input;

using BingoGame.ViewModels;

namespace BingoGame.Controllers
{
    public interface IBingoGameController
    {
        /**********************************************************************/
        #region Properties

        BingoGameStateViewModel GameState { get; }

        #endregion Properties

        /**********************************************************************/
        #region Commands

        ICommand CallNextNumber { get; }

        ICommand Reset { get; }

        #endregion Commands
    }
}
