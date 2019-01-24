using MahalluManager.Infra;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MahalluManager.Model {
    public class Residence : ViewModelBase {

        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string number;
        public string Number {
            get { return number; }
            set {
                number = value;
                OnPropertyChanged("Number");
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

        private string guardian;
        public string Guardian {
            get { return guardian; }
            set {
                guardian = value;
                OnPropertyChanged("Guardian");
            }
        }

        private string area;
        public string Area {
            get { return area; }
            set {
                area = value;
                OnPropertyChanged("Area");
            }
        }

        [ForeignKey("Residence_Id")]
        public ObservableCollection<ResidenceMember> Members { get; set; }
    }
}
