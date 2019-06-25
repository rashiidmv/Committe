using MahalluManager.Infra;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Windows.Media.Imaging;

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

        private BitmapImage bridePhoto;
        public BitmapImage BridePhoto {
            get {
                if(!String.IsNullOrEmpty(bridePhotoPath)) {
                    var uriSource = new Uri(bridePhotoPath);
                    bridePhoto = new BitmapImage(uriSource);
                }
                return bridePhoto;
            }
        }

        private BitmapImage groomPhoto;
        public BitmapImage GroomPhoto {
            get {
                if(!String.IsNullOrEmpty(groomPhotoPath)) {
                    var uriSource = new Uri(groomPhotoPath);
                    groomPhoto = new BitmapImage(uriSource);
                }
                return groomPhoto;
            }
        }

        private string bridePhotoPath;
        public string BridePhotoPath {
            get { return bridePhotoPath; }
            set {
                bridePhotoPath = value;
                OnPropertyChanged("BridePhotoPath");
                OnPropertyChanged("BridePhoto");
            }
        }

        private string groomPhotoPath;
        public string GroomPhotoPath {
            get { return groomPhotoPath; }
            set {
                groomPhotoPath = value;
                OnPropertyChanged("GroomPhotoPath");
                OnPropertyChanged("GroomPhoto");
            }
        }

        private DelegateCommand saveMarriageCommand;

        public DelegateCommand SaveMarriageCommand {
            get { return saveMarriageCommand; }
            set { saveMarriageCommand = value; }
        }
        private void ExecuteSaveMarriageCommand() {
        }

        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now;
        }
    }
}
