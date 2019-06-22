using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.EventTypes;
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
            eventAggregator.GetEvent<PubSubEvent<IncomeType>>().Subscribe((e) => {
                Contribution income = ((IncomeType)e).Contribution;
                bool isPresent = false;
                foreach(var item in ContributionList) {
                    if(item.Id==income.Id) {
                        item.ToatalAmount = income.ToatalAmount;
                        isPresent = true;
                    }
                }
                if(!isPresent) {
                    ContributionList.Add(income);
                }
                TotalIncome = CalcuateTotalIncome();
            });

            eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Subscribe((e) => {
                Expense expense = ((ExpenseType)e).Expense;
                bool isPresent = false;
                foreach(var item in ExpenseList) {
                    if(item.Id == expense.Id) {
                        item.ToatalAmount = expense.ToatalAmount;
                        isPresent = true;
                    }
                }
                if(!isPresent) {
                    ExpenseList.Add(expense);
                }
                TotalIncome = CalcuateTotalExpense();
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
        private String selected;
        public String Selected {
            get { return selected; }
            set {
                String temp = selected;
                selected = value;
                OnPropertyChanged("Selected");
                if(temp != value) {
                    TotalIncome = CalcuateTotalIncome();
                    TotalExpense = CalcuateTotalExpense();
                    SelectedYearType selectedYearType = new SelectedYearType() { SelectedYear = Selected.ToString() };
                    eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Publish(selectedYearType);
                }
            }
        }

        private void RefreshContribution() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(unitofWork.Contributions.GetAll());
                ExpenseList = new ObservableCollection<MahalluManager.Model.Expense>(unitofWork.Expenses.GetAll());
            }
        }

        private IList<MahalluManager.Model.Contribution> contributionList;
        public IList<MahalluManager.Model.Contribution> ContributionList {
            get { return contributionList; }
            set {
                contributionList = value;
                TotalIncome = CalcuateTotalIncome();
                OnPropertyChanged("ContributionList");
            }
        }

        private IList<MahalluManager.Model.Expense> expenseList;
        public IList<MahalluManager.Model.Expense> ExpenseList {
            get { return expenseList; }
            set {
                expenseList = value;
                TotalExpense = CalcuateTotalExpense();
                OnPropertyChanged("ExpenseList");
            }
        }

        private Decimal totalIncome;
        public Decimal TotalIncome {
            get { return totalIncome; }
            set {
                totalIncome = value;
                OnPropertyChanged("TotalIncome");
                OnPropertyChanged("TotalBalance");
                OnPropertyChanged("SelectedYearBalance");

            }
        }

        private Decimal totalExpense;
        public Decimal TotalExpense {
            get { return totalExpense; }
            set {
                totalExpense = value;
                OnPropertyChanged("TotalExpense");
                OnPropertyChanged("TotalBalance");
                OnPropertyChanged("SelectedYearBalance");
            }
        }
        public Decimal SelectedYearBalance {
            get {
                return (TotalIncome - TotalExpense);
            }
        }

        public String TotalBalance {
            get {
                Decimal selectedYearBalance = (TotalIncome - TotalExpense);
                return "Total Balance = " + PreviousYearBalance + "(Previous Balance) + " + selectedYearBalance + " = " + (PreviousYearBalance + selectedYearBalance);
            }
        }

        private Decimal previousYearBalance;
        public Decimal PreviousYearBalance {
            get {
                previousYearBalance = CalcuateTotalPreviousIncome() - CalcuateTotalPreviousExpense();
                return previousYearBalance;
            }
        }
        public Decimal CalcuateTotalPreviousIncome() {
            decimal totalIncome = 0;
            foreach(var item in ContributionList) {
                if(item.CreatedOn.Year < Convert.ToInt32(Selected)) {
                    totalIncome += item.ToatalAmount;
                }
            }
            return totalIncome;
        }
        public Decimal CalcuateTotalPreviousExpense() {
            decimal totalExpense = 0;
            foreach(var item in ExpenseList) {
                if(item.CreatedOn.Year < Convert.ToInt32(Selected)) {
                    totalExpense += item.ToatalAmount;
                }
            }
            return totalExpense;
        }

        public Decimal CalcuateTotalIncome() {
            decimal totalIncome = 0;
            foreach(var item in ContributionList) {
                if(item.CreatedOn.Year.ToString() == Selected) {
                    totalIncome += item.ToatalAmount;
                }
            }
            return totalIncome;
        }

        public Decimal CalcuateTotalExpense() {
            decimal totalExpense = 0;
            foreach(var item in ExpenseList) {
                if(item.CreatedOn.Year.ToString() == Selected) {
                    totalExpense += item.ToatalAmount;
                }
            }
            return totalExpense;
        }

        private void SetYears() {
            for(int i = -10; i <= 10; i++) {
                Years.Add(DateTime.Now.AddYears(i).Year.ToString());
            }
            Selected = DateTime.Now.Year.ToString();
        }
    }
}
