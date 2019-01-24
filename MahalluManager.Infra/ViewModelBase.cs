using Prism.Events;
using System.ComponentModel;

namespace MahalluManager.Infra {
    public class ViewModelBase : INotifyPropertyChanged //IActiveAware
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEventAggregator eventAggregator = MyEventAggregator.GetEventAggregator();
    }
}
