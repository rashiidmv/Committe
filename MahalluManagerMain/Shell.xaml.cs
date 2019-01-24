using MahalluManager.Infra;
using System.Windows;

namespace MahalluManagerMain {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window, IView {
        public Shell(IShellViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
        }

        public IViewModel ViewModel {
            get {
                return (IViewModel)DataContext;
            }

            set {
                DataContext = value;
            }
        }
    }
}
