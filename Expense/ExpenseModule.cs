using MahalluManager.Infra;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace Expense {
    public class ExpenseModule : ModuleBase {
        public ExpenseModule(IUnityContainer uc, IRegionManager rm) : base(uc, rm) {

        }
        protected override void InitializeModules() {
            MyRegionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(Main));
            MyRegionManager.RequestNavigate(RegionNames.ContentRegion, typeof(Main).FullName);
        }

        protected override void RegisterTypes() {
            MyUnityContainer.RegisterType<IMainViewModel, MainViewModel>();
            MyUnityContainer.RegisterType<object, Main>(typeof(Main).FullName);
        }
    }
}
