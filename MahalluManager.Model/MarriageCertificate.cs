using MahalluManager.Infra;
using System;

namespace MahalluManager.Model {
    public class MarriageCertificate : ViewModelBase {
        public int Id { get; set; }

        private String brideName;
        public String BrideName {
            get { return brideName; }
            set {
                brideName = value;
                OnPropertyChanged("BrideName");
            }
        }

        public byte[] BridePhoto { get; set; }

        private DateTime brideDOB;
        public DateTime BrideDOB {
            get { return brideDOB; }
            set {
                brideDOB = value;
                OnPropertyChanged("BrideDOB");
            }
        }

        private String brideFatherName;

        public String BrideFatherName {
            get { return brideFatherName; }
            set {
                brideFatherName = value;
                OnPropertyChanged("BrideFatherName");
            }
        }

        public String BrideHouseName { get; set; }
        public String BrideArea { get; set; }
        public String BridePincode { get; set; }
        public String BridePostOffice { get; set; }
        public String BrideDistrict { get; set; }
        public String BrideState { get; set; }
        public String BrideCountry { get; set; }

        private String groomName;
        public String GroomName {
            get { return groomName; }
            set {
                groomName = value;
                OnPropertyChanged("GroomName");
            }
        }

        public byte[] GroomPhoto { get; set; }
        public DateTime GroomDOB { get; set; }
        public string GroomFatherName { get; set; }
        public String GroomHouseName { get; set; }
        public String GroomArea { get; set; }
        public String GroomPincode { get; set; }
        public String GroomPostOffice { get; set; }
        public String GroomDistrict { get; set; }
        public String GroomState { get; set; }
        public String GroomCountry { get; set; }

        public DateTime MarriageDate { get; set; }
        public String MarriagePlace { get; set; }
    }
}
