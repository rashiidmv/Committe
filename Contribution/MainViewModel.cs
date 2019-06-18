using MahalluManager.Infra;

namespace Contribution {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        private string title;
        public string Title {
            get { return "Contributions"; }
            set { title = value; }
        }
    }
}
