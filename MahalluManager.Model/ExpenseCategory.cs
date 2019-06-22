using MahalluManager.Infra;

namespace MahalluManager.Model {
    public class ExpenseCategory : ViewModelBase {
        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private string name;
        public string Name {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool detailsRequired;
        public bool DetailsRequired {
            get { return detailsRequired; }
            set {
                detailsRequired = value;
                OnPropertyChanged("DetailsRequired");
            }
        }
    }
}