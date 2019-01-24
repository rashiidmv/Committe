using MahalluManager.Infra;
using System.Windows.Controls;

namespace Administrator {
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl, IView {
        public Main(IMainViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
            TabItem newResident = new TabItem {
                Header = "New Resident",
                Content = new NewResident()
            };
            tabControl1.Items.Add(newResident);
            TabItem settings = new TabItem {
                Header = "Settings",
                Content = new Settings()
            };
            tabControl1.Items.Add(settings);
        }

        public IViewModel ViewModel {
            get {
                return (IViewModel)DataContext; ;
            }

            set {
                DataContext = value;
            }
        }

    }
}