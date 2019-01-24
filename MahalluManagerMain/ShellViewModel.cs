using MahalluManager.Infra;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;

namespace MahalluManagerMain {
    public class ShellViewModel : IShellViewModel {
        public string MasjidName { get; private set; }
        private readonly IRegionManager RegionManager;
        public DelegateCommand<Object> NavigateCommand { get; private set; }
        public ShellViewModel(IUnityContainer uc, IRegionManager rm) {
            NavigateCommand = new DelegateCommand<object>(OnNavigate);
            RegionManager = rm;
            MasjidName = ConfigurationManager.AppSettings.Get("masjidname");
            // ApllicationCommands.NavigationCommand.RegisterCommand(NavigateCommand);
        }

        private void OnNavigate(object navigatePath) {
            if(navigatePath != null) {
                RegionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath.ToString());
            }
        }
    }
}
