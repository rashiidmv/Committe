using System.Windows.Controls;

namespace Marriage {
    /// <summary>
    /// Interaction logic for MarriageCertificateView.xaml
    /// </summary>
    public partial class MarriageCertificateView : UserControl {
        public MarriageCertificateView() {
            InitializeComponent();
            this.DataContext = new MarriageCertificateViewModel();
        }
    }
}
