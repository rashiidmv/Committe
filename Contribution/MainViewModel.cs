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

namespace Contribution {
    public class MainViewModel : ViewModelBase, IMainViewModel {

        public MainViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Subscribe((e) => {
                CategoryList = e;
            });

            ContributionColumns = WithoutDetailsColumns;
            SelectedContributionColumns = new ObservableCollection<string>();

            ClearCategory = new DelegateCommand(ExecuteClearCategory, CanExecuteClearCategory);
            ClearHouseNumber = new DelegateCommand(ExecuteClearHouseNumber, CanExecuteClearHouseNumber);
            ClearHouseName = new DelegateCommand(ExecuteClearHouseName, CanExecuteClearHouseName);
            ClearMemberName = new DelegateCommand(ExecuteClearMemberName, CanExecuteClearMemberName);
            ShowReportCommand = new DelegateCommand(ExecuteShowReport);
            PrintReportCommand = new DelegateCommand(ExecutePrintReport);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ShowDetailsFields = Visibility.Collapsed;
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
                SelectedContributionColumns.Clear();
                Result = null;
                SearchStatus = String.Empty;

                if(withDetails) {
                    ContributionColumns = WithDetailsColumns;
                    ShowDetailsFields = Visibility.Visible;
                } else {
                    ContributionColumns = WithoutDetailsColumns;
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
        private String memberName;
        public String MemberName {
            get { return memberName; }
            set {
                memberName = value;
                OnPropertyChanged("MemberName");
                ClearMemberName.RaiseCanExecuteChanged();
            }
        }
        private String houseName;
        public String HouseName {
            get { return houseName; }
            set {
                houseName = value;
                OnPropertyChanged("HouseName");
                ClearHouseName.RaiseCanExecuteChanged();
            }
        }

        private String houseNumber;
        public String HouseNumber {
            get { return houseNumber; }
            set {
                houseNumber = value;
                OnPropertyChanged("HouseNumber");
                ClearHouseNumber.RaiseCanExecuteChanged();
            }
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
        private DelegateCommand clearHouseNumber;
        public DelegateCommand ClearHouseNumber {
            get { return clearHouseNumber; }
            set { clearHouseNumber = value; }
        }
        private bool CanExecuteClearHouseNumber() {
            return !String.IsNullOrEmpty(HouseNumber);
        }
        private void ExecuteClearHouseNumber() {
            HouseNumber = String.Empty;
        }
        private DelegateCommand clearHouseName;
        public DelegateCommand ClearHouseName {
            get { return clearHouseName; }
            set { clearHouseName = value; }
        }
        private bool CanExecuteClearHouseName() {
            return !String.IsNullOrEmpty(HouseName);
        }
        private void ExecuteClearHouseName() {
            HouseName = String.Empty;
        }
        private DelegateCommand clearMemberName;
        public DelegateCommand ClearMemberName {
            get { return clearMemberName; }
            set { clearMemberName = value; }
        }
        private bool CanExecuteClearMemberName() {
            return !String.IsNullOrEmpty(MemberName);
        }
        private void ExecuteClearMemberName() {
            MemberName = String.Empty;
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
                }
                String[] temp = null;
                if(!String.IsNullOrEmpty(HouseNumber)) {
                    if(HouseNumber.Contains(";")) {
                        temp = HouseNumber.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = HouseNumber;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.HouserNumber.ToString(), new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(HouseName)) {
                    temp = null;
                    if(HouseName.Contains(";")) {
                        temp = HouseName.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = HouseName;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.HouserName, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(MemberName)) {
                    temp = null;
                    if(MemberName.Contains(";")) {
                        temp = MemberName.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = MemberName;
                    }
                    if(temp != null) {
                        inputDetails = inputDetails.Where(x => temp.Contains(x.MemberName, new ContainsComparer())).ToList();
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
                if(SelectedContributionColumns.Contains("Category Name")) {
                    GridViewColumn categoryName = new GridViewColumn() {
                        Header = "Category", DisplayMemberBinding = new Binding("CategoryName"), Width = 180
                    };
                    g.Columns.Add(categoryName);
                }
                if(SelectedContributionColumns.Contains("House Number")) {
                    GridViewColumn houseNumber = new GridViewColumn() {
                        Header = "House#", DisplayMemberBinding = new Binding("HouserNumber"), Width = 60
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
                        Width = 100
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
