namespace BingoGame.ViewModels
{
    public class BingoNumberViewModel : ViewModelBase
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

        public bool HasBeenCalled
        {
            get => _hasBeenCalled;
            set
            {
                if (_hasBeenCalled == value)
                    return;
                _hasBeenCalled = value;

                RaisePropertyChanged();
            }
        }
        private bool _hasBeenCalled;

        #endregion Properties
    }
}
