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

            ContributionColumns = new ObservableCollection<string>() { "Category Name", "Amount",
                "Date","Receipt No"
            };
            SelectedContributionColumns = new ObservableCollection<string>();

            ClearCategory = new DelegateCommand(ExecuteClearCategory, CanExecuteClearCategory);
            ShowReportCommand = new DelegateCommand(ExecuteShowReport);
            PrintReportCommand = new DelegateCommand(ExecutePrintReport);
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
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
            set { contributionColumns = value; }
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
            if(SelectedContributionColumns.Count == 0) {
                MessageBox.Show("No Columns selected");
                Result = null;
                SearchStatus = String.Empty;
                return;
            }
            if(DateTime.Compare(StartDate.Date,EndDate.Date) > 0) {
                MessageBox.Show("Start Date should be before End Date");
                return;
            }
            Result = new FlowDocument();
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
                p = new Paragraph();
                Result.Blocks.Add(p);
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
