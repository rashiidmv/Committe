using System.Windows.Controls;

namespace Common {
    /// <summary>
    /// Interaction logic for BrowseAndUpload.xaml
    /// </summary>
    public partial class BrowseAndUploadView : UserControl {
        public BrowseAndUploadView() {
            InitializeComponent();
            this.DataContext = new BrowseAndUploadViewModel();
        }
    }
}
