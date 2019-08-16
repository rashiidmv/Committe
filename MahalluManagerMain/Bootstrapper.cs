using Administrator;
using Contribution;
using Expense;
using Marriage;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Resident;
using System.Windows;

namespace MahalluManagerMain {
    class Bootstrapper : UnityBootstrapper {
        protected override DependencyObject CreateShell() {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell() {
            base.InitializeShell();

            Window mainWindow = (Window)Shell;
            mainWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            App.Current.MainWindow = mainWindow;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer() {
            base.ConfigureContainer();
            Container.RegisterType<IShellViewModel, ShellViewModel>();
        }

        protected override IModuleCatalog CreateModuleCatalog() {
            ModuleCatalog m = new ModuleCatalog();
            m.AddModule(typeof(ContributionModule));
            m.AddModule(typeof(ResidentModule)); 
            //m.AddModule(typeof(MarriageModule));
            m.AddModule(typeof(ExpenseModule));
            m.AddModule(typeof(AdministratorModule));
            return m;
        }
    }
}
