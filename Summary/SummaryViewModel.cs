using MahalluManager.Infra;
using Prism.Events;
using System;

namespace Summary {
    public class SummaryViewModel : ViewModelBase {
        public SummaryViewModel() {
            eventAggregator.GetEvent<PubSubEvent<String>>().Subscribe((e) => {
                TotalIncome = e;
            });
        }
        public String BalanceSummary {
            get {
                return "Balance Summary for " + DateTime.Now.Year;
            }
        }

        private String totalIncome;
        public String TotalIncome {
            get { return totalIncome; }
            set {
                totalIncome = value;
                OnPropertyChanged("TotalIncome");
            }
        }

        private String totalExpense;
        public String TotalExpense {
            get { return "17533"; }
            set {
                totalExpense = value;
                OnPropertyChanged("TotalExpense");
            }
        }

        private String totalBalance;
        public String TotalBalance {
            get { return "6432"; }
            set {
                totalBalance = value;
                OnPropertyChanged("TotalBalance");
            }
        }
    }
}
