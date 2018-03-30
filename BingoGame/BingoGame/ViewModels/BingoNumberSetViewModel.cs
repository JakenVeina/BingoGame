using System.Collections.Generic;

namespace BingoGame.ViewModels
{
    public class BingoNumberSetViewModel : ViewModelBase
    {
        /**********************************************************************/
        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;

                RaisePropertyChanged();
            }
        }
        private string _name;

        public IList<BingoNumberViewModel> Numbers { get; set; }

        #endregion Properties
    }
}
