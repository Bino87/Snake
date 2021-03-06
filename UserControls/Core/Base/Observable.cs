using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UserControls.Core.Base
{
    public abstract class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (oldValue == null)
            {
                if (newValue == null)
                    return false;
            }

            if (oldValue?.Equals(newValue) == true)
                return false;

            oldValue = newValue;
            OnPropertyChanged(propertyName);
            return true;

        }
    }
}
