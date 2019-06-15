using MahalluManager.Infra;
using System;

namespace MahalluManager.Model {
    public class ContributionDetail : ViewModelBase {
        public int Contribution_Id { get; set; }

        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private int houserNumber;
        public int HouserNumber {
            get { return houserNumber; }
            set { houserNumber = value;
                OnPropertyChanged("HouserNumber");
            }
        }

        private String houserName;
        public String HouserName {
            get { return houserName; }
            set { houserName = value;
                OnPropertyChanged("HouserName");
            }
        }

        private String memberName;
        public String MemberName {
            get { return memberName; }
            set { memberName = value;
                OnPropertyChanged("MemberName");
            }
        }
        private decimal amount;

        public decimal Amount {
            get { return amount; }
            set {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private DateTime createdOn;
        public DateTime CreatedOn {
            get { return createdOn; }
            set {
                createdOn = value;
                OnPropertyChanged("CreatedOn");
            }
        }

        private string receiptNo;
        public string ReceiptNo {
            get { return receiptNo; }
            set {
                receiptNo = value;
                OnPropertyChanged("ReceiptNo");
            }
        }

        private string careOf;

        public string CareOf {
            get { return careOf; }
            set {
                careOf = value;
                OnPropertyChanged("CareOf");
            }
        }

    }
}
