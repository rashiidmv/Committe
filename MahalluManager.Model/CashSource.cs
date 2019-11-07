using MahalluManager.Infra;
using System;

namespace MahalluManager.Model {
    public class CashSource : ViewModelBase {
        private int id;
        public int Id {
            get { return id; }
            set {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string sourceName;
        public string SourceName {
            get { return sourceName; }
            set {
                sourceName = value;
                OnPropertyChanged("SourceName");
            }
        }

        private Decimal amount;
        public Decimal Amount {
            get { return amount; }
            set {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }
    }
}
