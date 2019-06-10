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

        private int resideneId;

        public int ResidenceId {
            get { return resideneId; }
            set { resideneId = value; }
        }

        private int memberId;

        public int MemberId {
            get { return memberId; }
            set { memberId = value; }
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
