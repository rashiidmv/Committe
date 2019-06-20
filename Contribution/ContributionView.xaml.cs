using System.Windows.Controls;

namespace Contribution {
    /// <summary>
    /// Interaction logic for Contribution.xaml
    /// </summary>
    public partial class ContributionView : UserControl {
        public ContributionView() {
            InitializeComponent();
            this.DataContext = new ContributionViewModel();
        }
    }
}
