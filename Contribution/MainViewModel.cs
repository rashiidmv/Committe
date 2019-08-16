using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
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

namespace Contribution {
    public class MainViewModel : ViewModelBase, IMainViewModel {

        public MainViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Subscribe((e) => {
                CategoryList = e;
            });

            ContributionColumns = WithoutDetailsColumns;
            SelectedContributionColumns = new ObservableCollection<string>();

            ClearCategory = new DelegateCommand(ExecuteClearCategory, CanExecuteClearCategory);
            ShowReportCommand = new DelegateCommand(ExecuteShowReport);
            PrintReportCommand = new DelegateCommand(ExecutePrintReport);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public ObservableCollection<string> WithoutDetailsColumns {
            get {
                return new ObservableCollection<string>() { "Category Name", "Amount", "Date", "Receipt No" };
            }
        }
        public ObservableCollection<string> WithDetailsColumns {
            get {
                return new ObservableCollection<string>() { "Category Name", "House Number", "House Name", "Member", "Amount", "Date", "Receipt", "Care Of" };
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

        private bool withDetails;
        public bool WithDetails {
            get { return withDetails; }
            set {
                withDetails = value;
                SelectedContributionColumns.Clear();
                Result = null;
                SearchStatus = String.Empty;

                if(withDetails) {
                    ContributionColumns = WithDetailsColumns;
                } else {
                    ContributionColumns = WithoutDetailsColumns;
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
        private string title;
        public string Title {
            get { return "Contribution"; }
            set { title = value; }
        }

        private ObservableCollection<IncomeCategory> categoryList;
        public ObservableCollection<IncomeCategory> CategoryList {
            get { return categoryList; }
            set {
                categoryList = value;
                OnPropertyChanged("CategoryList");
            }
        }
        private ObservableCollection<string> contributionColumns;
        public ObservableCollection<string> ContributionColumns {
            get { return contributionColumns; }
            set {
                contributionColumns = value;
                OnPropertyChanged("ContributionColumns");
            }
        }

        private ObservableCollection<string> selectedContributionColumns;
        public ObservableCollection<string> SelectedContributionColumns {
            get { return selectedContributionColumns; }
            set { selectedContributionColumns = value; }
        }

        private IncomeCategory category;
        public IncomeCategory Category {
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
        private DelegateCommand showReportCommand;
        public DelegateCommand ShowReportCommand {
            get { return showReportCommand; }
            set { showReportCommand = value; }
        }
        private void ExecuteShowReport() {
            Result = null;
            SearchStatus = String.Empty;
            if(SelectedContributionColumns.Count == 0) {
                MessageBox.Show("No Columns selected");
                return;
            }
            if(DateTime.Compare(StartDate.Date, EndDate.Date) > 0) {
                MessageBox.Show("Start Date should be before End Date");
                return;
            }
            Result = new FlowDocument();
            if(!WithDetails) {
                List<MahalluManager.Model.Contribution> input = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    if(DateTime.Compare(StartDate, EndDate) == 0) {
                        input = unitOfWork.Contributions.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate.Date) == 0).ToList();
                    } else {
                        input = unitOfWork.Contributions.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate) >= 0 && DateTime.Compare(x.CreatedOn, EndDate) <= 0).ToList();
                    }
                    if(Category != null) {
                        input = input.Where(x => x.CategoryName == Category.Name).ToList(); ;
                    }
                }
                if(ValidateReportParameters()) {
                    BuildReport(input);
                }
            } else {
                List<MahalluManager.Model.Contribution> input = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    input = unitOfWork.Contributions.GetAll().ToList();
                    if(Category != null) {
                        input = input.Where(x => x.CategoryName == Category.Name).ToList(); ;
                    }
                }

                List<ContributionDetail> inputDetails = null;
                using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    if(DateTime.Compare(StartDate, EndDate) == 0) {
                        inputDetails = unitOfWork.ContributionDetails.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate.Date) == 0).ToList();
                    } else {
                        inputDetails = unitOfWork.ContributionDetails.GetAll().Where(x => DateTime.Compare(x.CreatedOn, StartDate) >= 0 && DateTime.Compare(x.CreatedOn, EndDate) <= 0).ToList();
                    }
                    //if(Category != null) {
                    //    input = input.Where(x => x.CategoryName == Category.Name).ToList(); ;
                    //}
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
        private void BuildReport(List<MahalluManager.Model.Contribution> contributions) {
            var x = contributions.Select(contribution =>
                       new {
                           CategoryName = contribution.CategoryName,
                           ToatalAmount = contribution.ToatalAmount,
                           CreatedOn = contribution.CreatedOn.ToShortDateString(),
                           ReceiptNo = contribution.ReceiptNo
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
                if(SelectedContributionColumns.Contains("Category Name")) {
                    GridViewColumn categoryName = new GridViewColumn() {
                        Header = "Category", DisplayMemberBinding = new Binding("CategoryName"), Width = 180
                    };
                    g.Columns.Add(categoryName);
                }

                if(SelectedContributionColumns.Contains("Amount")) {
                    var amount = new GridViewColumn() {
                        Header = "Amount",
                        DisplayMemberBinding = new Binding("ToatalAmount"),
                        Width = 120
                    };
                    g.Columns.Add(amount);
                }
                if(SelectedContributionColumns.Contains("Date")) {
                    var createdOn = new GridViewColumn() {
                        Header = "Date",
                        DisplayMemberBinding = new Binding("CreatedOn"),
                        Width = 100
                    };
                    g.Columns.Add(createdOn);
                }
                if(SelectedContributionColumns.Contains("Receipt No")) {
                    var receiptNo = new GridViewColumn() {
                        Header = "Receipt No",
                        DisplayMemberBinding = new Binding("ReceiptNo"),
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
            if(SelectedContributionColumns.Contains("Amount") && x.Count() > 0) {
                decimal totalAmount = 0;
                foreach(var item in x) {
                    totalAmount += item.ToatalAmount;
                }
                Paragraph totalAmountPara = new Paragraph();
                totalAmountPara.Inlines.Add("Total = " + totalAmount);
                Result.Blocks.Add(totalAmountPara);
            }
        }
        private void BuildReport(List<MahalluManager.Model.Contribution> contributions, List<ContributionDetail> contributionDetails) {
            var x = contributionDetails.Join(contributions, detail => detail.Contribution_Id, contribution => contribution.Id, (detail, contribution) =>
                       new {
                           CategoryName = contribution.CategoryName,
                           HouserNumber = detail.HouserNumber,
                           HouseName = detail.HouserName,
                           Member = detail.MemberName,
                           Amount = detail.Amount,
                           CreatedOn = detail.CreatedOn.ToShortDateString(),
                           ReceiptNo = detail.ReceiptNo,
                           CareOf = detail.CareOf
                       });
            if(x.Count() ==0 && category!=null && !Category.DetailsRequired) {
                MessageBox.Show("Details are not available for category "+Category.Name);
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
                if(SelectedContributionColumns.Contains("Category Name")) {
                    GridViewColumn categoryName = new GridViewColumn() {
                        Header = "Category", DisplayMemberBinding = new Binding("CategoryName"), Width = 180
                    };
                    g.Columns.Add(categoryName);
                }
                if(SelectedContributionColumns.Contains("House Number")) {
                    GridViewColumn houseNumber = new GridViewColumn() {
                        Header = "House #", DisplayMemberBinding = new Binding("HouserNumber"), Width = 180
                    };
                    g.Columns.Add(houseNumber);
                }

                if(SelectedContributionColumns.Contains("House Name")) {
                    var houseName = new GridViewColumn() {
                        Header = "House Name",
                        DisplayMemberBinding = new Binding("HouseName"),
                        Width = 120
                    };
                    g.Columns.Add(houseName);
                }

                if(SelectedContributionColumns.Contains("Member")) {
                    var member = new GridViewColumn() {
                        Header = "Member Name",
                        DisplayMemberBinding = new Binding("Member"),
                        Width = 120
                    };
                    g.Columns.Add(member);
                }

                if(SelectedContributionColumns.Contains("Amount")) {
                    var amount = new GridViewColumn() {
                        Header = "Amount",
                        DisplayMemberBinding = new Binding("Amount"),
                        Width = 80
                    };
                    g.Columns.Add(amount);
                }
                if(SelectedContributionColumns.Contains("Date")) {
                    var createdOn = new GridViewColumn() {
                        Header = "Date",
                        DisplayMemberBinding = new Binding("CreatedOn"),
                        Width = 100
                    };
                    g.Columns.Add(createdOn);
                }
                if(SelectedContributionColumns.Contains("Receipt No")) {
                    var receiptNo = new GridViewColumn() {
                        Header = "Receipt No",
                        DisplayMemberBinding = new Binding("ReceiptNo"),
                        Width = 80
                    };
                    g.Columns.Add(receiptNo);
                }
                if(SelectedContributionColumns.Contains("Care Of")) {
                    var careOf = new GridViewColumn() {
                        Header = "Care Of",
                        DisplayMemberBinding = new Binding("CareOf"),
                        Width = 80
                    };
                    g.Columns.Add(careOf);
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
            if(SelectedContributionColumns.Contains("Amount") && x.Count() > 0) {
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
