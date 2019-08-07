using MahalluManager.Infra;
using System;

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
        private string memberName;

        public string MemberName {
            get { return memberName; }
            set {
                memberName = value;
                OnPropertyChanged("MemberName");
            }
        }
        private DateTime dob;

        public DateTime DOB {
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

        private bool isGuardian;

        public bool IsGuardian {
            get { return isGuardian; }
            set {
                isGuardian = value;
                OnPropertyChanged("IsGuardian");
                OnPropertyChanged("Guardian");
            }
        }
        private String gender;
        public String Gender {
            get { return gender; }
            set {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private String marriageStatus;
        public String MarriageStatus {
            get { return marriageStatus; }
            set {
                marriageStatus = value;
                OnPropertyChanged("MarriageStatus");
            }
        }
        private String qualification;
        public String Qualification {
            get { return qualification; }
            set {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }
    }
}
