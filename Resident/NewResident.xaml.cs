using MahalluManager.DataAccess;
using MahalluManager.Model;
using System.Windows;
using System.Windows.Controls;

namespace Resident {
    /// <summary>
    /// Interaction logic for NewResident.xaml
    /// </summary>
    public partial class NewResident : UserControl {
        public NewResident() {
            InitializeComponent();
            this.DataContext = new NewResidentViewModel();
        }
    }
}
