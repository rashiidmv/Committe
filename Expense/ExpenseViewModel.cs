using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.EventTypes;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Expense {
    public class ExpenseViewModel : ViewModelBase {
        public ExpenseViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Subscribe((e) => {
                ExpenseCategoryList = e;
            });

            ClearExpenseCommand = new DelegateCommand(ExecuteClearExpense, CanExecuteClearExpense);
            DeleteExpenseCommand = new DelegateCommand(ExecuteDeleteExpense, CanExecuteDeleteExpense);
            NewExpenseCommand = new DelegateCommand(ExecuteNewExpense);
            SaveExpenseCommand = new DelegateCommand(ExecuteSaveExpense, CanExecuteSaveExpense);

            //SearchExpenseCommand = new DelegateCommand(ExecuteSearchExpense, CanExecuteSearchExpense);
            //ClearSearchExpenseCommand = new DelegateCommand(ExecuteClearSearchExpense);

            ClearExpenseDetailCommand = new DelegateCommand(ExecuteClearExpenseDetail, CanExecuteClearExpenseDetail);
            DeleteExpenseDetailCommand = new DelegateCommand(ExecueDeleteExpenseDetail, CanExecueDeleteExpenseDetail);
            NewExpenseDetailCommand = new DelegateCommand(ExecuteNewExpenseDetail, CanExecuteNewExpenseDetail);
            SaveExpenseDetailCommand = new DelegateCommand(ExecuteSaveExpenseDetail, CanExecuteSaveExpenseDetail);
            RefreshExpense();

            InitializeDatePicker();
            //InitializeSearchPanel();

            //eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Subscribe((e) => {
            //    SelectedYear = ((SelectedYearType)e).SelectedYear;
            //});
        }


        private decimal _amount;


        private ExpenseCategory expenseCategory;
        public ExpenseCategory ExpenseCategory {
            get { return expenseCategory; }
            set {
                expenseCategory = value;
                OnCategoryChanged();
                OnPropertyChanged("ExpenseCategory");
            }
        }

        private ObservableCollection<ExpenseCategory> expensecategoryList;
        public ObservableCollection<ExpenseCategory> ExpenseCategoryList {
            get { return expensecategoryList; }
            set {
                expensecategoryList = value;
                OnPropertyChanged("ExpenseCategoryList");
                if(ExpenseCategoryList != null) {
                    ExpenseCategory = ExpenseCategoryList.FirstOrDefault(x => x.Name == CurrentExpense?.CategoryName);
                }
            }
        }

        private ObservableCollection<MahalluManager.Model.Expense> expenseList;
        public ObservableCollection<MahalluManager.Model.Expense> ExpenseList {
            get { return expenseList; }
            set {
                expenseList = value;
                OnPropertyChanged("ExpenseList");
            }
        }
        private bool isEnableDetail;
        public bool IsEnableDetail {
            get { return isEnableDetail && ExpenseCategory != null && ExpenseCategory.DetailsRequired; }
            set {
                isEnableDetail = value;
                OnPropertyChanged("IsEnableDetail");
            }
        }

        private ObservableCollection<ExpenseDetails> expenseDetailList;
        public ObservableCollection<ExpenseDetails> ExpenseDetailList {
            get { return expenseDetailList; }
            set {
                expenseDetailList = value;
                OnPropertyChanged("ExpenseDetailList");
            }
        }

        private bool isEnable;
        public bool IsEnable {
            get { return isEnable; }
            set {
                isEnable = value;
                OnPropertyChanged("IsEnable");
            }
        }

        private String expenseBillNo;
        public String ExpenseBillNo {
            get { return expenseBillNo; }
            set {
                expenseBillNo = value;
                OnPropertyChanged("ExpenseBillNo");
            }
        }
       
        private String totalAmount;
        public String TotalAmount {
            get { return totalAmount; }
            set {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        private DateTime createdOn;
        public DateTime CreatedOn {
            get {
                if(createdOn == DateTime.MinValue)
                    return DateTime.Now;
                return createdOn;
            }
            set {
                createdOn = value;
                OnPropertyChanged("CreatedOn");
            }
        }

        private DateTime startDate;
        public DateTime StartDate {
            get { return startDate; }
            set {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTime endDate;
        public DateTime EndDate {
            get { return endDate; }
            set {
                endDate = value;
                OnPropertyChanged("EndtDate");
            }
        }

        //private decimal totalExpense;
        //public decimal TotalExpense {
        //    get { return totalExpense; }
        //    set {
        //        totalExpense = value;
        //        OnPropertyChanged("TotalExpense");
                
        //    }
        //}

        private bool isEnableExpenseCategory;
        public bool IsEnableExpenseCategory {
            get { return isEnableExpenseCategory; }
            set {
                isEnableExpenseCategory = value;
                OnPropertyChanged("IsEnableExpenseCategory");
            }
        }

        private string name;
        public string Name {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string amount;
        public string Amount {
            get { return amount; }
            set {
                amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private DateTime date;
        public DateTime Date {
            get { return date; }
            set {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        private String expenseDetailBillNo;
        public String ExpenseDetailBillNo {
            get { return expenseDetailBillNo; }
            set {
                expenseDetailBillNo = value;
                OnPropertyChanged("ExpenseDetailBillNo");
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

        private MahalluManager.Model.Expense currentExpense;
        public MahalluManager.Model.Expense CurrentExpense {
            get { return currentExpense; }
            set {
                currentExpense = value;
                CurrentExpenseChanged();
                //CalculateTotalAmount();
                OnPropertyChanged("IsEnable");
                OnPropertyChanged("IsEnableExpenseCategory");
                OnPropertyChanged("ExpenseCategory");
                ClearExpenseCommand.RaiseCanExecuteChanged();
                DeleteExpenseCommand.RaiseCanExecuteChanged();
                SaveExpenseCommand.RaiseCanExecuteChanged();
                NewExpenseDetailCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentExpense");
            }
        }

        private ExpenseDetails currentExpenseDetail;

        public ExpenseDetails CurrentExpenseDetail {
            get { return currentExpenseDetail; }
            set {
                currentExpenseDetail = value;
                CurrentExpenseDetailChanged();
                ClearExpenseDetailCommand.RaiseCanExecuteChanged();
                DeleteExpenseDetailCommand.RaiseCanExecuteChanged();
                SaveExpenseDetailCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentExpenseDetail");
            }
        }

        private DelegateCommand clearExpenseCommand;

        public DelegateCommand ClearExpenseCommand {
            get { return clearExpenseCommand; }
            set { clearExpenseCommand = value; }
        }

        private bool CanExecuteClearExpense() {
            return CurrentExpense != null;
        }

        private void ExecuteClearExpense() {
            CurrentExpense = null;
            IsEnableDetail = false;
        }


        private DelegateCommand deleteExpenseCommand;
        public DelegateCommand DeleteExpenseCommand {
            get { return deleteExpenseCommand; }
            set { deleteExpenseCommand = value; }
        }
        private bool CanExecuteDeleteExpense() {
            return CurrentExpense != null;
        }

        private void ExecuteDeleteExpense() {
            MessageBoxResult result = MessageBox.Show("Deleting Expense will delete all of the details also, \nAre you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentExpense != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        MahalluManager.Model.Expense expense = unitofWork.Expenses.Get(CurrentExpense.Id);
                        unitofWork.Expenses.Remove(expense);
                        unitofWork.Complete();

                        ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                        eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);

                        ExpenseList.Remove(CurrentExpense);
                        CurrentExpense = null;
                    }
                }
            }
        }

        private DelegateCommand newExpenseCommand;

        public DelegateCommand NewExpenseCommand {
            get { return newExpenseCommand; }
            set { newExpenseCommand = value; }
        }

        private void ExecuteNewExpense() {
            CurrentExpense = null;
        }

        private DelegateCommand saveExpenseCommand;

        public DelegateCommand SaveExpenseCommand {
            get { return saveExpenseCommand; }
            set { saveExpenseCommand = value; }
        }
        private bool CanExecuteSaveExpense() {
            return CurrentExpense == null;
        }

        private void ExecuteSaveExpense() {
            if(ValidateExpense()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    MahalluManager.Model.Expense expense = GetExpense();
                    unitOfWork.Expenses.Add(expense);
                    unitOfWork.Complete();
                    ExpenseList.Add(expense);
                    CurrentExpense = expense;
                    ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                    eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);
                }
            }
        }

        private DelegateCommand clearExpenseDetailCommand;

        public DelegateCommand ClearExpenseDetailCommand {
            get { return clearExpenseDetailCommand; }
            set { clearExpenseDetailCommand = value; }
        }

        private bool CanExecuteClearExpenseDetail() {
            return CurrentExpenseDetail != null && ExpenseCategory != null && ExpenseCategory.DetailsRequired;
        }

        private void ExecuteClearExpenseDetail() {
            CurrentExpenseDetail = null;
        }

        private DelegateCommand deleteExpenseDetailCommand;

        public DelegateCommand DeleteExpenseDetailCommand {
            get { return deleteExpenseDetailCommand; }
            set { deleteExpenseDetailCommand = value; }
        }
        private bool CanExecueDeleteExpenseDetail() {
            return CurrentExpenseDetail != null && ExpenseCategory != null && ExpenseCategory.DetailsRequired;
        }

        private void ExecueDeleteExpenseDetail() {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete " + CurrentExpenseDetail.Name, "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentExpenseDetail != null) {
                    using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                        ExpenseDetails expenseDetail = unitOfWork.ExpenseDetails.Get(CurrentExpenseDetail.Id);
                        if(expenseDetail != null) {
                            unitOfWork.ExpenseDetails.Remove(expenseDetail);
                            unitOfWork.Complete();

                            decimal amount = CurrentExpenseDetail.Amount;
                            ExpenseDetailList.Remove(CurrentExpenseDetail);
                            CurrentExpenseDetail = null;

                            //To update total amount
                            CurrentExpense.ToatalAmount = Convert.ToDecimal(TotalAmount) - amount;
                            TotalAmount = (Convert.ToDecimal(TotalAmount) - amount).ToString();
                            //TotalExpense = -amount;
                            unitOfWork.Expenses.Update(CurrentExpense);
                            unitOfWork.Complete();

                            ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                            eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);
                        }
                    }
                }
            }
        }

        private DelegateCommand newExpenseDetailCommand;
        public DelegateCommand NewExpenseDetailCommand {
            get { return newExpenseDetailCommand; }
            set { newExpenseDetailCommand = value; }
        }

        private bool CanExecuteNewExpenseDetail() {
            return ExpenseCategory != null && ExpenseCategory.DetailsRequired;
        }

        private void ExecuteNewExpenseDetail() {
            CurrentExpenseDetail = null;
        }

        private DelegateCommand saveExpenseDetailCommand;
        public DelegateCommand SaveExpenseDetailCommand {
            get { return saveExpenseDetailCommand; }
            set { saveExpenseDetailCommand = value; }
        }

        private bool CanExecuteSaveExpenseDetail() {
            return CurrentExpenseDetail == null && ExpenseCategory != null && ExpenseCategory.DetailsRequired;
        }

        private void ExecuteSaveExpenseDetail() {
            if(ValidateExpenseDetail()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    ExpenseDetails expenseDetail = GetExpenseDetails();
                    unitOfWork.ExpenseDetails.Add(expenseDetail);
                    unitOfWork.Complete();
                    ExpenseDetailList.Add(expenseDetail);
                    CurrentExpenseDetail = expenseDetail;
                    //To update total amount
                    CurrentExpense.ToatalAmount = Convert.ToDecimal(TotalAmount) + _amount;
                    TotalAmount = (Convert.ToDecimal(TotalAmount) + _amount).ToString();
                    //TotalExpense = _amount;
                    unitOfWork.Expenses.Update(CurrentExpense);
                    unitOfWork.Complete();

                    ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                    eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);
                }
            }
        }


        private void OnCategoryChanged() {
            if(ExpenseCategory != null && ExpenseCategory.DetailsRequired && CurrentExpense != null) {
                IsEnableDetail = true;
                IsEnable = false;
            } else if(ExpenseCategory != null && !ExpenseCategory.DetailsRequired && CurrentExpense == null) {
                IsEnableDetail = false;
                IsEnable = true;
            } else {
                IsEnable = false;
            }
            if(CurrentExpense == null) {
                ExpenseBillNo = string.Empty;
                TotalAmount = string.Empty;
                CreatedOn = DateTime.Now;
            }
        }
        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now.AddMonths(2);
        }

        private MahalluManager.Model.Expense GetExpense() {
            var expense = new MahalluManager.Model.Expense();
            if(!ExpenseCategory.DetailsRequired) {
                expense.ToatalAmount = Convert.ToDecimal(TotalAmount?.Trim());
            }
            expense.BillNo = ExpenseBillNo?.Trim();
            expense.CategoryName = ExpenseCategory.Name?.Trim();
            expense.CreatedOn = CreatedOn;
            return expense;
        }

        private bool ValidateExpense() {
            if(ExpenseCategory == null ||
                String.IsNullOrEmpty(ExpenseCategory.Name)) {
                MessageBox.Show("Please enter Category");
                return false;
            }
            if(String.IsNullOrEmpty(TotalAmount) && !ExpenseCategory.DetailsRequired) {
                MessageBox.Show("Please enter Total Amount");
                return false;
            }
            _amount = 0;
            if(!String.IsNullOrEmpty(TotalAmount) && !Decimal.TryParse(TotalAmount, out _amount) && !ExpenseCategory.DetailsRequired) {
                MessageBox.Show("Please enter valid Total Amount");
                return false;
            }
            if(!String.IsNullOrEmpty(TotalAmount) && _amount <= 0 && !ExpenseCategory.DetailsRequired) {
                MessageBox.Show("Please enter Total Amount greater than zero");
                return false;
            }
            return true;
        }

        private void RefreshExpense() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                ExpenseList = new ObservableCollection<MahalluManager.Model.Expense>(unitofWork.Expenses.GetAll());
                if(ExpenseList != null && ExpenseList.Count > 0) {
                    CurrentExpense = ExpenseList[0];
                } else {
                    IsEnable = true;
                    IsEnableExpenseCategory = true;
                }
            }
        }
        private void CurrentExpenseChanged() {
            if(CurrentExpense != null) {
                if(ExpenseCategoryList != null) {
                    ExpenseCategory = ExpenseCategoryList.FirstOrDefault(x => x.Name == CurrentExpense.CategoryName);
                }
                TotalAmount = CurrentExpense.ToatalAmount.ToString();
                CreatedOn = CurrentExpense.CreatedOn;
                ExpenseBillNo = currentExpense.BillNo;
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ExpenseDetailList = new ObservableCollection<ExpenseDetails>(unitofWork.ExpenseDetails.Find((x) => x.Expense_Id == CurrentExpense.Id));
                    if(ExpenseDetailList != null && ExpenseDetailList.Count > 0) {
                        CurrentExpenseDetail = ExpenseDetailList[0];
                    } else {
                        ClearExpensesDetailsList();
                    }
                }
                IsEnable = false;
                IsEnableExpenseCategory = false;
                IsEnableDetail = false;
            } else {
                IsEnable = true;
                IsEnableDetail = false;
                IsEnableExpenseCategory = true;
                ClearExpense();
                ClearExpensesDetailsList();
            }
        }
        private void ClearExpense() {
            ExpenseCategory = null;
            TotalAmount = ExpenseBillNo = string.Empty;
            CreatedOn = DateTime.Now;
        }
        private void ClearExpensesDetailsList() {
            if(ExpenseDetailList != null && ExpenseDetailList.Count > 0) {
                ExpenseDetailList.Clear();
            }
            CurrentExpenseDetail = null;
        }

        private bool ValidateExpenseDetail() {
            if(CurrentExpense == null) {
                MessageBox.Show("Please select Expense first to add a details");
                return false;
            }
            if(String.IsNullOrEmpty(Name)) {
                MessageBox.Show("Please enter Name");
                return false;
            }
            if(String.IsNullOrEmpty(Amount)) {
                MessageBox.Show("Please enter Amount");
                return false;
            }
            _amount = 0;
            if(!Decimal.TryParse(Amount, out _amount)) {
                MessageBox.Show("Please enter valid Amount");
                return false;
            }
            if(_amount <= 0) {
                MessageBox.Show("Please enter Amount greater than zero");
                return false;
            }
            return true;
        }

        private ExpenseDetails GetExpenseDetails() {
            var expenseDetail = new ExpenseDetails();
            expenseDetail.Name = Name?.Trim();
            expenseDetail.CreatedOn = CreatedOn;
            expenseDetail.BillNo = ExpenseDetailBillNo?.Trim();
            expenseDetail.CareOf = CareOf?.Trim();
            expenseDetail.Expense_Id = CurrentExpense.Id;
            expenseDetail.Amount = _amount;
            return expenseDetail;
        }
        private void CurrentExpenseDetailChanged() {
            if(CurrentExpenseDetail != null) {
                                Name = CurrentExpenseDetail.Name;
                Amount = CurrentExpenseDetail.Amount.ToString();
                Date = CurrentExpenseDetail.CreatedOn;
                ExpenseDetailBillNo = CurrentExpenseDetail.BillNo;
                CareOf = CurrentExpenseDetail.CareOf;

                IsEnableDetail = false;
            } else {
                IsEnableDetail = true;
                ClearExpenseDetails();
            }
        }
        private void ClearExpenseDetails() {
            Name = Amount = ExpenseDetailBillNo = CareOf = String.Empty;
            Date = DateTime.Now;
        }
    }
}
