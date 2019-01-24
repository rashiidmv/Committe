using MahalluManager.Infra;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator {
    public class AdministratorModule : ModuleBase {
        public AdministratorModule(IUnityContainer uc, IRegionManager rm) : base(uc, rm) {

        }
        protected override void InitializeModules() {
            MyRegionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(Main));
            //  MyRegionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ViewA));
            MyRegionManager.RequestNavigate(RegionNames.ContentRegion, typeof(Main).FullName);
        }

        protected override void RegisterTypes() {
            MyUnityContainer.RegisterType<IMainViewModel, MainViewModel>();
            MyUnityContainer.RegisterType<object, Main>(typeof(Main).FullName);
        }
    }
}