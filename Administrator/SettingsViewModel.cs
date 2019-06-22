using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Administrator {
    public class SettingsViewModel : ViewModelBase {
        private string areaText;
        public string AreaText {
            get { return areaText; }
            set {
                areaText = value;
                AddAreaCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("AreaText");
            }
        }

        private ObservableCollection<Area> areaList;
        public ObservableCollection<Area> AreaList {
            get { return areaList; }
            set {
                areaList = value;
                OnPropertyChanged("AreaList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
            }
        }

        private string contributionCategoryText;
        public string ContributionCategoryText {
            get { return contributionCategoryText; }
            set {
                contributionCategoryText = value;
                AddContributionCategoryCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("ContributionCategoryText");
            }
        }
        private bool contributionDetailsRequired;
        public bool ContributionDetailsRequired {
            get { return contributionDetailsRequired; }
            set {
                contributionDetailsRequired = value;
                OnPropertyChanged("ContributionDetailsRequired");
            }
        }

        private ObservableCollection<IncomeCategory> contributionCategoryList;
        public ObservableCollection<IncomeCategory> ContributionCategoryList {
            get { return contributionCategoryList; }
            set {
                contributionCategoryList = value;
                OnPropertyChanged("ContributionCategoryList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
            }
        }

        private string expenseCategoryText;
        public string ExpenseCategoryText {
            get { return expenseCategoryText; }
            set {
                expenseCategoryText = value;
                AddExpenseCategoryCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("ExpenseCategoryText");
            }
        }

        private bool expenseDetailsRequired;
        public bool ExpenseDetailsRequired {
            get { return expenseDetailsRequired; }
            set {
                expenseDetailsRequired = value;
                OnPropertyChanged("ExpenseDetailsRequired");
            }
        }

        private ObservableCollection<ExpenseCategory> expenseCategoryList;
        public ObservableCollection<ExpenseCategory> ExpenseCategoryList {
            get { return expenseCategoryList; }
            set {
                expenseCategoryList = value;
                OnPropertyChanged("ExpenseCategoryList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
            }
        }
        public SettingsViewModel() {
            AddAreaCommand = new DelegateCommand(ExecuteAddArea, CanExecuteAddArea);
            DeleteCommand = new DelegateCommand<Area>(ExecuteDelete);

            AddContributionCategoryCommand = new DelegateCommand(ExecuteAddContributionCategory, CanExecuteAddContributionCategory);
            DeleteContributionCategoryCommand = new DelegateCommand<IncomeCategory>(ExecuteContributionCategoryDelete);

            AddExpenseCategoryCommand = new DelegateCommand(ExecuteAddExpenseCategory, CanExecuteAddExpenseCategory);
            DeleteExpenseCategoryCommand = new DelegateCommand<ExpenseCategory>(ExecuteExpenseCategoryDelete);

            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                AreaList = new ObservableCollection<Area>(unitofWork.Areas.GetAll());
                ContributionCategoryList = new ObservableCollection<IncomeCategory>(unitofWork.IncomeCategories.GetAll());
                ExpenseCategoryList = new ObservableCollection<ExpenseCategory>(unitofWork.ExpenseCategories.GetAll());
            }
        }

        private DelegateCommand addAreaCommand;
        public DelegateCommand AddAreaCommand {
            get { return addAreaCommand; }
            set { addAreaCommand = value; }
        }

        private void ExecuteAddArea() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var area = new Area() { Name = AreaText };
                unitofWork.Areas.Add(area);
                AreaList.Add(area);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
                unitofWork.Complete();
                AreaText = String.Empty;
            }
        }
        private bool CanExecuteAddArea() {
            return AreaText != null && AreaText != String.Empty;
        }

        private DelegateCommand<Area> deleteCommand;
        public DelegateCommand<Area> DeleteCommand {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }

        private void ExecuteDelete(Area area) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    AreaList.Remove(area);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
                    var result = unitofWork.Areas.Find((x) => x.Id == area.Id).FirstOrDefault();
                    unitofWork.Areas.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private DelegateCommand addContributionCategoryCommand;
        public DelegateCommand AddContributionCategoryCommand {
            get { return addContributionCategoryCommand; }
            set { addContributionCategoryCommand = value; }
        }

        private void ExecuteAddContributionCategory() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var category = new IncomeCategory() { Name = ContributionCategoryText, DetailsRequired = ContributionDetailsRequired };
                unitofWork.IncomeCategories.Add(category);
                ContributionCategoryList.Add(category);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
                ContributionCategoryText = String.Empty;
                ContributionDetailsRequired = default(bool);
                unitofWork.Complete();
            }
        }
        private bool CanExecuteAddContributionCategory() {
            return ContributionCategoryText != null && ContributionCategoryText != String.Empty;
        }

        private DelegateCommand<IncomeCategory> deleteContributionCategoryCommand;
        public DelegateCommand<IncomeCategory> DeleteContributionCategoryCommand {
            get { return deleteContributionCategoryCommand; }
            set { deleteContributionCategoryCommand = value; }
        }

        private void ExecuteContributionCategoryDelete(IncomeCategory category) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ContributionCategoryList.Remove(category);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
                    var result = unitofWork.IncomeCategories.Find((x) => x.Id == category.Id).FirstOrDefault();
                    unitofWork.IncomeCategories.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private DelegateCommand addExpenseCategoryCommand;
        public DelegateCommand AddExpenseCategoryCommand {
            get { return addExpenseCategoryCommand; }
            set { addExpenseCategoryCommand = value; }
        }

        private void ExecuteAddExpenseCategory() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var expense = new ExpenseCategory() { Name = ExpenseCategoryText, DetailsRequired = ExpenseDetailsRequired };
                unitofWork.ExpenseCategories.Add(expense);
                ExpenseCategoryList.Add(expense);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
                ExpenseCategoryText = String.Empty;
                ExpenseDetailsRequired = default(bool);
                unitofWork.Complete();
            }
        }

        private bool CanExecuteAddExpenseCategory() {
            return ExpenseCategoryText != null && ExpenseCategoryText != String.Empty;
        }

        private DelegateCommand<ExpenseCategory> deleteExpenseCategoryCommand;
        public DelegateCommand<ExpenseCategory> DeleteExpenseCategoryCommand {
            get { return deleteExpenseCategoryCommand; }
            set { deleteExpenseCategoryCommand = value; }
        }

        private void ExecuteExpenseCategoryDelete(ExpenseCategory expenseCategory) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ExpenseCategoryList.Remove(expenseCategory);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
                    var result = unitofWork.ExpenseCategories.Find((x) => x.Id == expenseCategory.Id).FirstOrDefault();
                    unitofWork.ExpenseCategories.Remove(result);
                    unitofWork.Complete();
                }
            }
        }
    }
}
