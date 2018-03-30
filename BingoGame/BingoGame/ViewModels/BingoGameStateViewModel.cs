using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BingoGame.ViewModels
{
    public class BingoGameStateViewModel : ViewModelBase
    {
        /**********************************************************************/
        #region Properties

        public IList<BingoNumberSetViewModel> Board { get; set; }

        public ObservableCollection<string> History { get; set; }

        #endregion Properties
    }
}
