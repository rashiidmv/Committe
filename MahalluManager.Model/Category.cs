using MahalluManager.Infra;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MahalluManager.Model {
    public class Category : ViewModelBase {
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

        //[ForeignKey("Category_Id")]
        //public ObservableCollection<Contribution> Contributions { get; set; }
    }
}
