using System.ComponentModel;
using System.Runtime.CompilerServices;
using TipSplit.Annotations;

namespace TipSplit.ViewModel
{
    abstract internal class BaseViewModel :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null, bool force = false)
        {
            if (Equals(storage, value) && !force)
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
