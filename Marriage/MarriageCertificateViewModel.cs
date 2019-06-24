using MahalluManager.Infra;
using Microsoft.Practices.Prism.Commands;
using System;

namespace Marriage {
    public class MarriageCertificateViewModel : ViewModelBase {
        public MarriageCertificateViewModel() {
            InitializeDatePicker();
            SaveMarriageCommand = new DelegateCommand(ExecuteSaveMarriageCommand);
        }

        

        private DateTime startDate;
        public DateTime StartDate {
            get { return startDate; }
            set {
                startDate = value;
                OnPropertyChanged("startDate");
            }
        }
        private DateTime endDate;
        public DateTime EndDate {
            get { return endDate; }
            set {
                endDate = value;
                OnPropertyChanged("endtDate");
            }
        }
        private string bridePhoto;

        public string BridePhoto {
            get { return bridePhoto; }
            set {
                bridePhoto = value;
                OnPropertyChanged("BridePhoto");
            }
        }
        private DelegateCommand saveMarriageCommand;

        public DelegateCommand SaveMarriageCommand {
            get { return saveMarriageCommand; }
            set { saveMarriageCommand = value; }
        }
        private void ExecuteSaveMarriageCommand() {
            String s = BridePhoto;
        }

        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now;
        }
    }
}
