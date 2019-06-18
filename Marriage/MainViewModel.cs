using MahalluManager.Infra;

namespace Marriage {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        private string title;
        public string Title {
            get { return "Marriage"; }
            set { title = value; }
        }
    }
}
