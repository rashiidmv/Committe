using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.Common;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Expense {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        public MainViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Subscribe((e) => {
                CategoryList = e;
            });

            ExpenseColumns = WithoutDetailsColumns;
            SelectedExpenseColumns = new ObservableCollection<string>();

            ClearCategory = new DelegateCommand(ExecuteClearCategory, CanExecuteClearCategory);
            ClearName = new DelegateCommand(ExecuteClearName, CanExecuteClearName);
            ClearBillNo = new DelegateCommand(ExecuteClearBillNo, CanExecuteClearBillNo);
            ClearCareOf = new DelegateCommand(ExecuteClearCareOf, CanExecuteClearCareOf);
            ShowReportCommand = new DelegateCommand(ExecuteShowReport);
            PrintReportCommand = new DelegateCommand(ExecutePrintReport);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ShowDetailsFields = Visibility.Collapsed;
        }
        private string title;
        public string Title {
            get { return "Expense"; }
            set { title = value; }
        }

        public ObservableCollection<string> WithoutDetailsColumns {
            get {
                return new ObservableCollection<string>() { "Category Name", "Amount", "Date", "Bill No" };
            }
        }
        public ObservableCollection<string> WithDetailsColumns {
            get {
                return new ObservableCollection<string>() { "Category Name", "Name", "Amount", "Date", "Bill No", "Care Of" };
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
                OnPropertyChanged("EndDate");
            }
        }

        private bool showHeader;
        public bool ShowHeader {
            get { return showHeader; }
            set {
                showHeader = value;
                OnPropertyChanged("ShowHeader");
            }
        }
        private Visibility showDetailsFields;

        public Visibility ShowDetailsFields {
            get { return showDetailsFields; }
            set {
                showDetailsFields = value;
                OnPropertyChanged("ShowDetailsFields");

            }
        }

        private bool withDetails;
        public bool WithDetails {
            get { return withDetails; }
            set {
                withDetails = value;
                SelectedExpenseColumns.Clear();
                Result = null;
                SearchStatus = String.Empty;

                if(withDetails) {
                    ExpenseColumns = WithDetailsColumns;
                    ShowDetailsFields = Visibility.Visible;
                } else {
                    ExpenseColumns = WithoutDetailsColumns;
                    ShowDetailsFields = Visibility.Collapsed;
                }
                OnPropertyChanged("WithDetails");
            }
        }


        private FlowDocument result;
        public FlowDocument Result {
            get { return result; }
            set {
                result = value;
                if(result != null) {
                    result.PageWidth = 740;
                    result.TextAlignment = System.Windows.TextAlignment.Center;
                }
                OnPropertyChanged("Result");
            }
        }
        private string headerText;
        public string HeaderText {
            get { return headerText; }
            set {
                headerText = value;
                OnPropertyChanged("HeaderText");
            }
        }

        private String searchStatus;
        public String SearchStatus {
            get { return searchStatus; }
            set {
                searchStatus = value;
                OnPropertyChanged("SearchStatus");
            }
        }


        private ObservableCollection<ExpenseCategory> categoryList;
        public ObservableCollection<ExpenseCategory> CategoryList {
            get { return categoryList; }
            set {
                categoryList = value;
                OnPropertyChanged("CategoryList");
            }
        }
        private ObservableCollection<string> expenseColumns;
        public ObservableCollection<string> ExpenseColumns {
            get { return expenseColumns; }
            set {
                expenseColumns = value;
                OnPropertyChanged("ExpenseColumns");
            }
        }

        private ObservableCollection<string> selectedExpenseColumns;
        public ObservableCollection<string> SelectedExpenseColumns {
            get { return selectedExpenseColumns; }
            set { selectedExpenseColumns = value; }
        }
        private String name;
        public String Name {
            get { return name; }
            set {
                name = value;
                OnPropertyChanged("Name");
                ClearName.RaiseCanExecuteChanged();
            }
        }
        private String billNo;
        public String BillNo {
            get { return billNo; }
            set {
                billNo = value;
                OnPropertyChanged("BillNo");
                ClearBillNo.RaiseCanExecuteChanged();
            }
        }

        private String careOf;
        public String CareOf {
            get { return careOf; }
            set {
                careOf = value;
                OnPropertyChanged("CareOf");
                ClearCareOf.RaiseCanExecuteChanged();
            }
        }

        private ExpenseCategory category;
        public ExpenseCategory Category {
            get { return category; }
            set {
                category = value;
                OnPropertyChanged("Category");
                ClearCategory.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand clearCategory;
        public DelegateCommand ClearCategory {
            get { return clearCategory; }
            set { clearCategory = value; }
        }
        private bool CanExecuteClearCategory() {
            return Category != null;
        }
        private void ExecuteClearCategory() {
            Category = null;
        }
        private DelegateCommand clearName;
        public DelegateCommand ClearName {
            get { return clearName; }
            set { clearName = value; }
        }
        private bool CanExecuteClearName() {
            return !String.IsNullOrEmpty(Name);
        }
        private void ExecuteClearName() {
            Name = String.Empty;
        }
        private DelegateCommand clearBillNo;
        public DelegateCommand ClearBillNo {
            get { return clearBillNo; }
            set { clearBillNo = value; }
        }
        private bool CanExecuteClearBillNo() {
            return !String.IsNullOrEmpty(BillNo);
        }
        private void ExecuteClearBillNo() {
            BillNo = String.Empty;
        }
        private DelegateCommand clearCareOf;
        public DelegateCommand ClearCareOf {
            get { return clearCareOf; }
            set { clearCareOf = value; }
        }
        private bool CanExecuteClearCareOf() {
            return !String.IsNullOrEmpty(CareOf);
        }
        private void ExecuteClearCareOf() {
            CareOf = String.Empty;
        }

        private DelegateCommand showReportCommand;
        public DelegateCommand ShowReportCommand {
            get { return showReportCommand; }
            set { showReportCommand = value; }
        }
        private void ExecuteShowReport() {
            Result = null;
            SearchStatus = String.Empty;
            if(SelectedExpenseColumns.Count == 0) {
                MessageBox.Show("No Columns selected");
                return;
            }
            if(DateTime.Compare(StartDate.Date, EndDate.Date) > 0) {
                MessageBox.Show("Start Date should be before End Date");
                return;
            }
            Result = new FlowDocument();
            if(!WithDetails) {
                List<MahalluManager.Model.Expense> input = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    if(DateTime.Compare(StartDate, EndDate) == 0) {
                        input = unitOfWork.Expenses.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate.Date) == 0).ToList();
                    } else {
                        input = unitOfWork.Expenses.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate) >= 0 && DateTime.Compare(x.CreatedOn, EndDate) <= 0).ToList();
                    }
                    if(Category != null) {
                        input = input.Where(x => x.CategoryName == Category.Name).ToList(); ;
                    }
                }
                if(ValidateReportParameters()) {
                    BuildReport(input);
                }
            } else {
                List<MahalluManager.Model.Expense> input = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    input = unitOfWork.Expenses.GetAll().ToList();
                    if(Category != null) {
                        input = input.Where(x => x.CategoryName == Category.Name).ToList(); ;
                    }
                }

                List<ExpenseDetails> inputDetails = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    if(DateTime.Compare(StartDate, EndDate) == 0) {
                        inputDetails = unitOfWork.ExpenseDetails.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate.Date) == 0).ToList();
                    } else {
                        inputDetails = unitOfWork.ExpenseDetails.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate) >= 0 && DateTime.Compare(x.CreatedOn, EndDate) <= 0).ToList();
                    }
                }
                String[] temp = null;
                if(!String.IsNullOrEmpty(Name)) {
                    if(Name.Contains(";")) {
                        temp = Name.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = Name;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.Name, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(BillNo)) {
                    temp = null;
                    if(BillNo.Contains(";")) {
                        temp = BillNo.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = BillNo;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.BillNo, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(CareOf)) {
                    temp = null;
                    if(CareOf.Contains(";")) {
                        temp = CareOf.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = CareOf;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.CareOf, new ContainsComparer())).ToList();
                    }
                }
                if(ValidateReportParameters()) {
                    BuildReport(input, inputDetails);
                }
            }
        }

        private DelegateCommand printReportCommand;
        public DelegateCommand PrintReportCommand {
            get { return printReportCommand; }
            set { printReportCommand = value; }
        }
        private void ExecutePrintReport() {
            MessageBox.Show("Press Ctrl + P to print");
        }
        private void BuildReport(List<MahalluManager.Model.Expense> expenses) {
            var x = expenses.Select(expense =>
                       new {
                           CategoryName = expense.CategoryName,
                           ToatalAmount = expense.ToatalAmount,
                           CreatedOn = expense.CreatedOn.ToShortDateString(),
                           BillNo = expense.BillNo
                       });
            if(x != null && x.Count() > 0) {
                SearchStatus = x.Count() + " Found";
            } else {
                SearchStatus = "No Result Found";
            }
            for(int i = 0; i < x.Count(); i += 38) {
                var list = (from c in x
                            select c).Skip(i).Take(38);

                ListView l = new ListView();
                l.ItemsSource = list;
                GridView g = new GridView();
                l.Width = 700; //690
                l.MaxWidth = 700;
                if(SelectedExpenseColumns.Contains("Category Name")) {
                    GridViewColumn categoryName = new GridViewColumn() {
                        Header = "Category", DisplayMemberBinding = new Binding("CategoryName"), Width = 180
                    };
                    g.Columns.Add(categoryName);
                }

                if(SelectedExpenseColumns.Contains("Amount")) {
                    var amount = new GridViewColumn() {
                        Header = "Amount",
                        DisplayMemberBinding = new Binding("ToatalAmount"),
                        Width = 120
                    };
                    g.Columns.Add(amount);
                }
                if(SelectedExpenseColumns.Contains("Date")) {
                    var createdOn = new GridViewColumn() {
                        Header = "Date",
                        DisplayMemberBinding = new Binding("CreatedOn"),
                        Width = 100
                    };
                    g.Columns.Add(createdOn);
                }
                if(SelectedExpenseColumns.Contains("Bill No")) {
                    var receiptNo = new GridViewColumn() {
                        Header = "Bill No",
                        DisplayMemberBinding = new Binding("BillNo"),
                        Width = 80
                    };
                    g.Columns.Add(receiptNo);
                }

                l.View = g;
                inlineContainer = new InlineUIContainer();
                inlineContainer.Child = l;
                p = new Paragraph(inlineContainer);
                if(ShowHeader) {
                    Paragraph header = new Paragraph();
                    header.Inlines.Add(HeaderText);
                    Result.Blocks.Add(header);
                }
                Result.Blocks.Add(p);
                if(i + 38 < x.Count()) {
                    p = new Paragraph();
                    Result.Blocks.Add(p);
                }
            }
            if(SelectedExpenseColumns.Contains("Amount") && x.Count() > 0) {
                decimal totalAmount = 0;
                foreach(var item in x) {
                    totalAmount += item.ToatalAmount;
                }
                Paragraph totalAmountPara = new Paragraph();
                totalAmountPara.Inlines.Add("Total = " + totalAmount);
                Result.Blocks.Add(totalAmountPara);
            }
        }
        private void BuildReport(List<MahalluManager.Model.Expense> expenses, List<ExpenseDetails> expenseDetails) {
            var x = expenseDetails.Join(expenses, detail => detail.Expense_Id, expense => expense.Id, (detail, expense) =>
                       new {
                           CategoryName = expense.CategoryName,
                           Name = detail.Name,
                           BillNo = detail.BillNo,
                           CareOf = detail.CareOf,
                           Amount = detail.Amount,
                           CreatedOn = detail.CreatedOn.ToShortDateString()
                       });
            if(x.Count() == 0 && category != null && !Category.DetailsRequired) {
                MessageBox.Show("Details are not available for category " + Category.Name);
                return;
            }
            if(x != null && x.Count() > 0) {
                SearchStatus = x.Count() + " Found";
            } else {
                SearchStatus = "No Result Found";
            }
            for(int i = 0; i < x.Count(); i += 38) {
                var list = (from c in x
                            select c).Skip(i).Take(38);

                ListView l = new ListView();
                l.ItemsSource = list;
                GridView g = new GridView();
                l.Width = 700; //690
                l.MaxWidth = 700;
                if(SelectedExpenseColumns.Contains("Category Name")) {
                    GridViewColumn categoryName = new GridViewColumn() {
                        Header = "Category", DisplayMemberBinding = new Binding("CategoryName"), Width = 180
                    };
                    g.Columns.Add(categoryName);
                }
                if(SelectedExpenseColumns.Contains("Name")) {
                    GridViewColumn name = new GridViewColumn() {
                        Header = "Name", DisplayMemberBinding = new Binding("Name"), Width = 60
                    };
                    g.Columns.Add(name);
                }

                if(SelectedExpenseColumns.Contains("Bill No")) {
                    var billNo = new GridViewColumn() {
                        Header = "Bill No",
                        DisplayMemberBinding = new Binding("BillNo"),
                        Width = 80
                    };
                    g.Columns.Add(billNo);
                }

                if(SelectedExpenseColumns.Contains("Care Of")) {
                    var member = new GridViewColumn() {
                        Header = "Care Of",
                        DisplayMemberBinding = new Binding("CareOf"),
                        Width = 120
                    };
                    g.Columns.Add(member);
                }

                if(SelectedExpenseColumns.Contains("Amount")) {
                    var amount = new GridViewColumn() {
                        Header = "Amount",
                        DisplayMemberBinding = new Binding("Amount"),
                        Width = 100
                    };
                    g.Columns.Add(amount);
                }
                if(SelectedExpenseColumns.Contains("Date")) {
                    var createdOn = new GridViewColumn() {
                        Header = "Date",
                        DisplayMemberBinding = new Binding("CreatedOn"),
                        Width = 100
                    };
                    g.Columns.Add(createdOn);
                }
                l.View = g;
                inlineContainer = new InlineUIContainer();
                inlineContainer.Child = l;
                p = new Paragraph(inlineContainer);
                if(ShowHeader) {
                    Paragraph header = new Paragraph();
                    header.Inlines.Add(HeaderText);
                    Result.Blocks.Add(header);
                }
                Result.Blocks.Add(p);
                if(i + 38 < x.Count()) {
                    p = new Paragraph();
                    Result.Blocks.Add(p);
                }
            }
            if(SelectedExpenseColumns.Contains("Amount") && x.Count() > 0) {
                decimal totalAmount = 0;
                foreach(var item in x) {
                    totalAmount += item.Amount;
                }
                Paragraph totalAmountPara = new Paragraph();
                totalAmountPara.Inlines.Add("Total = " + totalAmount);
                Result.Blocks.Add(totalAmountPara);
            }
        }
        private Paragraph p = null;
        private InlineUIContainer inlineContainer = null;
        private bool ValidateReportParameters() {
            if(ShowHeader && String.IsNullOrEmpty(HeaderText)) {
                MessageBox.Show("Please enter report title");
                return false;
            }
            return true;
        }

    }
}
