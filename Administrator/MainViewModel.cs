using MahalluManager.Infra;

namespace Administrator {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        private string title;
        public string Title {
            get { return "Administrator"; }
            set { title = value; }
        }
    }
}