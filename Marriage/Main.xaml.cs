using MahalluManager.Infra;
using System.Windows.Controls;

namespace Marriage {
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl, IView {
        public Main(IMainViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
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
