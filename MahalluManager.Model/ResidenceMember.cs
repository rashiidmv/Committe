using MahalluManager.Infra;

namespace MahalluManager.Model {
    public class ResidenceMember : ViewModelBase {
        public int Residence_Id { get; set; }

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
        private string dob;

        public string DOB {
            get { return dob; }
            set {
                dob = value;
                OnPropertyChanged("DOB");
            }
        }

        private string job;

        public string Job {
            get { return job; }
            set {
                job = value;
                OnPropertyChanged("Job");
            }
        }
        private string mobile;

        public string Mobile {
            get { return mobile; }
            set {
                mobile = value;
                OnPropertyChanged("Mobile");
            }
        }

        private bool abroad;

        public bool Abroad {
            get { return abroad; }
            set {
                abroad = value;
                OnPropertyChanged("Abroad");
            }
        }
        private string country;

        public string Country {
            get { return country; }
            set {
                country = value;
                OnPropertyChanged("Country");
            }
        }

    }
}
