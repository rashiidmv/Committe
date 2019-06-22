using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Infra.EventTypes;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Summary {
    public class SummaryViewModel : ViewModelBase {
        public SummaryViewModel() {
            eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Subscribe((e) => {
                Selected = ((SelectedYearType)e).SelectedYear;
            });
            eventAggregator.GetEvent<PubSubEvent<TotalIncomeType>>().Subscribe((e) => {
                decimal income = ((TotalIncomeType)e).TotalIncome;
                TotalIncome = (Convert.ToDecimal(TotalIncome) + income).ToString();
            });
            RefreshContribution();
            Years = new ObservableCollection<string>();
            SetYears();
        }



        private ObservableCollection<String> years;
        public ObservableCollection<String> Years {
            get { return years; }
            set {
                years = value;
                OnPropertyChanged("Years");
            }
        }
        private string selected;
        public string Selected {
            get { return selected; }
            set {
                string temp = selected;
                selected = value;
                OnPropertyChanged("Selected");
                if(temp != value) {
                    SelectedYearType selectedYearType = new SelectedYearType() { SelectedYear = Selected };
                    eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Publish(selectedYearType);
                }

            }
        }



        private void RefreshContribution() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(unitofWork.Contributions.GetAll());
            }
        }

        private IList<MahalluManager.Model.Contribution> contributionList;
        public IList<MahalluManager.Model.Contribution> ContributionList {
            get { return contributionList; }
            set {
                contributionList = value;
                CalcuateTotalIncome();
                OnPropertyChanged("ContributionList");
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


        public void CalcuateTotalIncome() {
            decimal totalIncome = 0;
            foreach(var item in ContributionList) {
                totalIncome += item.ToatalAmount;
            }
            TotalIncome = totalIncome.ToString();
        }

        private void SetYears() {
            for(int i = -10; i <= 10; i++) {
                Years.Add(DateTime.Now.AddYears(i).Year.ToString());
            }
            Selected = DateTime.Now.Year.ToString();
        }
    }
}
