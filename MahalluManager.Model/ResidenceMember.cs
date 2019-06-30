using MahalluManager.Infra;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

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
        private DateTime dob;

        public DateTime DOB {
            get { return dob; }
            set {
                dob = value;
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

        private bool isGuardian;

        public bool IsGuardian {
            get { return isGuardian; }
            set {
                isGuardian = value;
                OnPropertyChanged("IsGuardian");
                OnPropertyChanged("Guardian");
            }
        }
        public String Gender { get; set; }
        public String MarriageStatus { get; set; }
        public String Qualification { get; set; }
    }
}
