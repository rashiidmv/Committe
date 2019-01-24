using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace MahalluManager.Infra {
    public abstract class ModuleBase : IModule {
        protected IUnityContainer MyUnityContainer;
        protected IRegionManager MyRegionManager;
        protected ModuleBase(IUnityContainer uc, IRegionManager rm) {
            MyUnityContainer = uc;
            MyRegionManager = rm;

        }
        public void Initialize() {
            RegisterTypes();
            InitializeModules();
        }

        protected abstract void InitializeModules();
        protected abstract void RegisterTypes();
    }
}
