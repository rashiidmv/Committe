using MahalluManager.Infra;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Marriage {
    public class MarriageModule : ModuleBase {
        public MarriageModule(IUnityContainer uc, IRegionManager rm) : base(uc, rm) {

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
