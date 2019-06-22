using MahalluManager.Infra;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MahalluManager.Model {
    public class Expense : ViewModelBase {

        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private String categoryName;

        public String CategoryName {
            get { return categoryName; }
            set {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }


        private decimal toatalAmount;

        public decimal ToatalAmount {
            get { return toatalAmount; }
            set {
                toatalAmount = value;
                OnPropertyChanged("ToatalAmount");
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

        [ForeignKey("Expense_Id")]
        public ObservableCollection<ExpenseDetails> ExpenseDetails { get; set; }
    }
}