using System.ComponentModel;

namespace SystemPlus.ComponentModel
{
    /// <summary>
    /// Convenience class that implements INotifyPropertyChanged
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}