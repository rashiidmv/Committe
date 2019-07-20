using MahalluManager.Infra;
using MahalluManager.Model.EventTypes;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;

namespace MahalluManagerMain {
    public class ShellViewModel : ViewModelBase, IShellViewModel {

        private String masjidName;
        public String MasjidName {
            get { return masjidName; }
            set {
                masjidName = value;
                OnPropertyChanged("MasjidName");
            }
        }

        private readonly IRegionManager RegionManager;
        public DelegateCommand<Object> NavigateCommand { get; private set; }
        public ShellViewModel(IUnityContainer uc, IRegionManager rm) {
            eventAggregator.GetEvent<PubSubEvent<CommonDetailsType>>().Subscribe((e) => {
                MasjidName = ((CommonDetailsType)e).MasjidName;
            });
            NavigateCommand = new DelegateCommand<object>(OnNavigate);
            RegionManager = rm;
            // ApllicationCommands.NavigationCommand.RegisterCommand(NavigateCommand);
        }

        private void OnNavigate(object navigatePath) {
            if(navigatePath != null) {
                RegionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath.ToString());
            }
        }
    }
}
