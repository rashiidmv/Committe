using System.Windows.Controls;

namespace Summary {
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl {
        public SummaryView() {
            InitializeComponent();
            this.DataContext = new SummaryViewModel();
        }
    }
}
