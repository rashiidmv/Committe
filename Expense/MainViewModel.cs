using MahalluManager.Infra;

namespace Expense {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        private string title;
        public string Title {
            get { return "Expense"; }
            set { title = value; }
        }
    }
}
