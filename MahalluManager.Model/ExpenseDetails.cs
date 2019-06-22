using MahalluManager.Infra;
using System;

namespace MahalluManager.Model {
    public class ExpenseDetails : ViewModelBase {
        public int Expense_Id { get; set; }

        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private String name;
        public String Name {
            get { return name; }
            set { name = value; }
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

        private string billNo;
        public string BillNo {
            get { return billNo; }
            set {
                billNo = value;
                OnPropertyChanged("BillNo");
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
