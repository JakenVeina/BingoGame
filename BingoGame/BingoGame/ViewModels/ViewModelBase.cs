using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BingoGame.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /**********************************************************************/
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        internal protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged
    }
}
