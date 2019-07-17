using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.EventTypes;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Contribution {
    public class ContributionViewModel : ViewModelBase {
        public ContributionViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Subscribe((e) => {
                CategoryList = e;
            });

            ClearContributionCommand = new DelegateCommand(ExecuteClearContribution, CanExecuteClearContribution);
            DeleteContributionCommand = new DelegateCommand(ExecuteDeleteContribution, CanExecuteDeleteContribution);
            NewContributionCommand = new DelegateCommand(ExecuteNewContribution);
            SaveContributionCommand = new DelegateCommand(ExecuteSaveContribution, CanExecuteSaveContribution);

            ClearSearchContributionCommand = new DelegateCommand(ExecuteClearSearchContribution);
            SearchContributionCommand = new DelegateCommand(ExecuteSearchContribution, CanExecuteSearchContribution);

            ClearDetailCommand = new DelegateCommand(ExecuteClearDetail, CanExecuteClearDetail);
            DeleteDetailCommand = new DelegateCommand(ExecueDeleteDetail, CanExecueDeleteDetail);
            NewDetailCommand = new DelegateCommand(ExecuteNewDetail, CanExecuteNewDetail);
            SaveDetailCommand = new DelegateCommand(ExecuteSaveDetail, CanExecuteSaveDetail);
            IsMember = true;
            RefreshContribution();

            SetupSearchableIndexes();

            eventAggregator.GetEvent<PubSubEvent<ResidenceType>>().Subscribe((e) => {
                Residence residence = ((ResidenceType)e).Residence;
                bool isPresent = false;
                foreach(var item in SearchableResidenceNumbers) {
                    if(item == residence.Number) {
                        isPresent = true;
                        break;
                    }
                }
                if(!isPresent) {
                    SearchableResidenceNumbers.Add(residence.Number);
                } else if(isPresent && e.Operation == MahalluManager.Model.Common.Operation.Delete) {
                    SearchableResidenceNumbers.Remove(residence.Number);
                }

                isPresent = false;
                foreach(var item in SearchableResidenceNames) {
                    if(item.Equals(residence.Name)) {
                        isPresent = true;
                    }
                }
                if(!isPresent) {
                    SearchableResidenceNames.Add(residence.Name);
                } else if(isPresent && e.Operation == MahalluManager.Model.Common.Operation.Delete) {
                    SearchableResidenceNames.Remove(residence.Name);
                }
            });

            eventAggregator.GetEvent<PubSubEvent<ResidenceMemberType>>().Subscribe((e) => {
                ResidenceMember residenceMember = ((ResidenceMemberType)e).ResidenceMember;
                bool isPresent = false;
                foreach(var item in SearchableMemberNames) {
                    if(item.Equals(residenceMember.MemberName)) {
                        isPresent = true;
                    }
                }
                if(!isPresent) {
                    SearchableMemberNames.Add(residenceMember.MemberName);
                } else if(isPresent && e.Operation == MahalluManager.Model.Common.Operation.Delete) {
                    SearchableMemberNames.Remove(residenceMember.MemberName);
                }

                isPresent = false;
                foreach(var item in members) {
                    if(item.Id == residenceMember.Id) {
                        isPresent = true;
                    }
                }
                Residence residence = GetResidence(residenceMember);
                residenceMember.Job = residence.Name;
                residenceMember.Qualification = residence.Number;
                if(!isPresent) {
                    members.Add(residenceMember);
                    SearchableMembers.Add(residenceMember.MemberName + " \t@" + residenceMember.Job + "_" + residenceMember.Qualification + "@");
                } else if(isPresent && e.Operation == MahalluManager.Model.Common.Operation.Delete) {
                    members.Remove(residenceMember);
                    SearchableMembers.Remove(residenceMember.MemberName + " \t@" + residenceMember.Job + "_" + residenceMember.Qualification + "@");
                }
            });

            InitializeDatePicker();
            InitializeSearchPanel();

            eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Subscribe((e) => {
                SelectedYear = ((SelectedYearType)e).SelectedYear;
            });
        }

        private static Residence GetResidence(ResidenceMember residenceMember) {
            using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                return unitOfWork.Residences.Get(residenceMember.Residence_Id);
            }
        }

        private decimal _amount;
        private List<MahalluManager.Model.Contribution> searchSource = null;


        private String selectedYear;
        public String SelectedYear {
            get { return selectedYear; }
            set {
                String temp = selectedYear;
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
                if(temp != value) {
                    SelectedYearType selectedYearType = new SelectedYearType() { SelectedYear = SelectedYear };
                    eventAggregator.GetEvent<PubSubEvent<SelectedYearType>>().Publish(selectedYearType);
                }
            }
        }


        private bool isEnableDetail;
        public bool IsEnableDetail {
            get { return isEnableDetail && Category != null && Category.DetailsRequired; }
            set {
                isEnableDetail = value;
                OnPropertyChanged("IsEnableDetail");
            }
        }

        private bool isMember;
        public bool IsMember {
            get { return isMember; }
            set {
                isMember = value;
                if(CurrentContributionDetail == null) {
                    MemberName = String.Empty;
                }
                OnPropertyChanged("IsMember");
                OnPropertyChanged("MemberNameTextVisibility");
                OnPropertyChanged("MemberNameAutoTextVisibility");
            }
        }

        private Visibility memberNameTextVisibility;
        public Visibility MemberNameTextVisibility {
            get { return IsMember ? Visibility.Collapsed : Visibility.Visible; }
            private set { memberNameTextVisibility = value; }
        }

        private Visibility memberNameAutoTextVisibility;
        public Visibility MemberNameAutoTextVisibility {
            get { return IsMember ? Visibility.Visible : Visibility.Collapsed; }
            private set { memberNameAutoTextVisibility = value; }
        }


        private List<String> searchableYears;
        public List<String> SearchableYears {
            get { return searchableYears; }
            set {
                searchableYears = value;
                OnPropertyChanged("SearchableYears");
            }
        }

        private Visibility showYearSearch;
        public Visibility ShowYearSearch {
            get {
                return SearchByYear ? Visibility.Visible : Visibility.Collapsed;
            }
            private set { showYearSearch = value; }
        }

        private Visibility showOtherSearch;
        public Visibility ShowOtherSearch {
            get {
                return (SearchByHouseNumber || SearchByHouseName || SearchByMemberName) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set {
                showOtherSearch = value;
            }
        }

        private Visibility showCategorySearch;
        public Visibility ShowCategorySearch {
            get { return SearchByCategory ? Visibility.Visible : Visibility.Collapsed; }
            private set { showCategorySearch = value; }
        }

        private bool searchByYear;
        public bool SearchByYear {
            get { return searchByYear; }
            set {
                searchByYear = value;
                if(searchByYear) {
                    SearchByHouseNumber = SearchByMemberName = SearchByCategory = SearchByHouseName = false;
                    SearchContributionText = String.Empty;
                }
                OnPropertyChanged("SearchByYear");
                OnPropertyChanged("ShowYearSearch");
            }
        }

        private bool searchByHouseNumber;

        public bool SearchByHouseNumber {
            get { return searchByHouseNumber; }
            set {
                searchByHouseNumber = value;
                if(searchByHouseNumber) {
                    SearchByYear = SearchByMemberName = SearchByCategory = SearchByHouseName = false;
                    SearchContributionText = String.Empty;
                    SearchableIndexes = SearchableResidenceNumbers;
                }
                OnPropertyChanged("SearchByHouseNumber");
                OnPropertyChanged("ShowOtherSearch");

            }
        }

        private bool searchByHouseName;
        public bool SearchByHouseName {
            get { return searchByHouseName; }
            set {
                searchByHouseName = value;
                if(SearchByHouseName) {
                    SearchByYear = SearchByMemberName = SearchByCategory = SearchByHouseNumber = false;
                    SearchContributionText = String.Empty;
                    SearchableIndexes = SearchableResidenceNames;
                }
                OnPropertyChanged("SearchByHouseName");
                OnPropertyChanged("ShowOtherSearch");
            }
        }


        private bool searchByMemberName;
        public bool SearchByMemberName {
            get { return searchByMemberName; }
            set {
                searchByMemberName = value;
                if(searchByMemberName) {
                    SearchByYear = SearchByHouseNumber = SearchByCategory = SearchByHouseName = false;
                    SearchContributionText = String.Empty;
                    SearchableIndexes = SearchableMemberNames;
                }
                OnPropertyChanged("SearchByMemberName");
                OnPropertyChanged("ShowOtherSearch");
            }
        }

        private bool searchByCategory;
        public bool SearchByCategory {
            get { return searchByCategory; }
            set {
                searchByCategory = value;
                if(searchByCategory) {
                    SearchByYear = SearchByHouseNumber = SearchByMemberName = false;
                    SearchContributionText = String.Empty;
                }
                OnPropertyChanged("SearchByCategory");
                OnPropertyChanged("ShowCategorySearch");
            }
        }

        private DelegateCommand saveDetailCommand;
        public DelegateCommand SaveDetailCommand {
            get { return saveDetailCommand; }
            set { saveDetailCommand = value; }
        }
        private bool CanExecuteSaveDetail() {
            return CurrentContributionDetail == null && Category != null && Category.DetailsRequired;
        }
        private void ExecuteSaveDetail() {
            if(ValidateContributionDetail()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    ContributionDetail contributionDetail = GetContributionDetails();
                    unitOfWork.ContributionDetails.Add(contributionDetail);
                    unitOfWork.Complete();
                    ContributionDetailList.Add(contributionDetail);
                    CurrentContributionDetail = contributionDetail;
                    //To update total amount
                    CurrentContribution.ToatalAmount = Convert.ToDecimal(TotalAmount) + _amount;
                    TotalAmount = (Convert.ToDecimal(TotalAmount) + _amount).ToString();
                    unitOfWork.Contributions.Update(CurrentContribution);
                    unitOfWork.Complete();

                    IncomeType incomeType = new IncomeType() { Contribution = CurrentContribution };
                    eventAggregator.GetEvent<PubSubEvent<IncomeType>>().Publish(incomeType);
                }
            }
        }

        private DelegateCommand newDetailCommand;
        public DelegateCommand NewDetailCommand {
            get { return newDetailCommand; }
            set {
                newDetailCommand = value;
                OnPropertyChanged("NewDetailCommand");
            }
        }
        private void ExecuteNewDetail() {
            CurrentContributionDetail = null;
        }
        private bool CanExecuteNewDetail() {
            return Category != null && Category.DetailsRequired;
        }

        private DelegateCommand clearDetailCommand;
        public DelegateCommand ClearDetailCommand {
            get { return clearDetailCommand; }
            set { clearDetailCommand = value; }
        }

        private bool CanExecuteClearDetail() {
            return CurrentContributionDetail != null && Category != null && Category.DetailsRequired;
        }

        private void ExecuteClearDetail() {
            CurrentContributionDetail = null;
        }

        private DelegateCommand deleteDetailCommand;

        public DelegateCommand DeleteDetailCommand {
            get { return deleteDetailCommand; }
            set {
                deleteDetailCommand = value;
                OnPropertyChanged("DeleteDetailCommand");
            }
        }

        private bool CanExecueDeleteDetail() {
            return CurrentContributionDetail != null && Category != null && Category.DetailsRequired;
        }

        private void ExecueDeleteDetail() {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete " + CurrentContributionDetail.MemberName, "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentContributionDetail != null) {
                    using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                        ContributionDetail contributionDetail = unitOfWork.ContributionDetails.Get(CurrentContributionDetail.Id);
                        if(contributionDetail != null) {
                            unitOfWork.ContributionDetails.Remove(contributionDetail);
                            unitOfWork.Complete();

                            decimal amount = CurrentContributionDetail.Amount;
                            ContributionDetailList.Remove(CurrentContributionDetail);
                            CurrentContributionDetail = null;

                            //To update total amount
                            CurrentContribution.ToatalAmount = Convert.ToDecimal(TotalAmount) - amount;
                            TotalAmount = (Convert.ToDecimal(TotalAmount) - amount).ToString();
                            unitOfWork.Contributions.Update(CurrentContribution);
                            unitOfWork.Complete();
                            IncomeType incomeType = new IncomeType() { Contribution = CurrentContribution };
                            eventAggregator.GetEvent<PubSubEvent<IncomeType>>().Publish(incomeType);
                        }
                    }
                }
            }
        }


        private DelegateCommand saveContributionCommand;

        public DelegateCommand SaveContributionCommand {
            get { return saveContributionCommand; }
            set { saveContributionCommand = value; }
        }

        private bool CanExecuteSaveContribution() {
            return CurrentContribution == null;
        }
        private void ExecuteSaveContribution() {
            if(ValidateContribution()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    MahalluManager.Model.Contribution contribution = GetContribution();
                    unitOfWork.Contributions.Add(contribution);
                    unitOfWork.Complete();
                    ContributionList.Add(contribution);

                    CurrentContribution = contribution;
                    IncomeType incomeType = new IncomeType() { Contribution = CurrentContribution };
                    eventAggregator.GetEvent<PubSubEvent<IncomeType>>().Publish(incomeType);
                }
            }
        }

        private DelegateCommand newContributionCommand;
        public DelegateCommand NewContributionCommand {
            get { return newContributionCommand; }
            set { newContributionCommand = value; }
        }
        private void ExecuteNewContribution() {
            CurrentContribution = null;
        }

        private DelegateCommand deleteContributionCommand;

        public DelegateCommand DeleteContributionCommand {
            get { return deleteContributionCommand; }
            set { deleteContributionCommand = value; }
        }
        private bool CanExecuteDeleteContribution() {
            return CurrentContribution != null;
        }

        private void ExecuteDeleteContribution() {
            MessageBoxResult result = MessageBox.Show("Deleting Contribution will delete all of the details also, \nAre you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentContribution != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        MahalluManager.Model.Contribution contribution = unitofWork.Contributions.Get(CurrentContribution.Id);
                        unitofWork.Contributions.Remove(contribution);
                        unitofWork.Complete();

                        IncomeType incomeType = new IncomeType() { Contribution = CurrentContribution };
                        eventAggregator.GetEvent<PubSubEvent<IncomeType>>().Publish(incomeType);

                        ContributionList.Remove(CurrentContribution);
                        CurrentContribution = null;
                    }
                }
            }
        }

        private DelegateCommand searchContributionCommand;
        public DelegateCommand SearchContributionCommand {
            get { return searchContributionCommand; }
            set { searchContributionCommand = value; }
        }

        private bool CanExecuteSearchContribution() {
            return ContributionList != null && !String.IsNullOrEmpty(SearchContributionText);
        }

        private void ExecuteSearchContribution() {
            RefreshContribution();
            searchSource = ContributionList.ToList(); ;
            if(SearchByYear) {
                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(searchSource.FindAll((x) => x.CreatedOn.Year == Convert.ToInt32(SearchContributionText)));
                if(ContributionList != null && ContributionList.Count == 0) {
                    MessageBox.Show("No Contribution Found in " + SearchContributionText);
                }
            } else if(SearchByHouseNumber) {
                int houseNumber;
                if(Int32.TryParse(SearchContributionText.Trim(), out houseNumber)) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        List<ContributionDetail> tempContributionDetails = unitofWork.ContributionDetails.Find((x) => x.HouserNumber == houseNumber).ToList();
                        if(tempContributionDetails != null && tempContributionDetails.Count == 0) {
                            MessageBox.Show("No Contribution Found with House number " + SearchContributionText);
                        } else {
                            ContributionList.Clear();
                            foreach(var item in tempContributionDetails) {
                                MahalluManager.Model.Contribution contribution = searchSource.Find((x) => x.Id == item.Contribution_Id);
                                if(contribution != null && !ContributionList.Contains(contribution)) {
                                    ContributionList.Add(contribution);
                                }
                            }
                        }
                    }
                } else {
                    MessageBox.Show("Please enter house number");
                }
            } else if(SearchByHouseName) {
                string houseName = SearchContributionText.Trim();
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    List<ContributionDetail> tempContributionDetails = unitofWork.ContributionDetails.Find((x) => x.HouserName.Contains(houseName)).ToList();
                    if(tempContributionDetails != null && tempContributionDetails.Count == 0) {
                        MessageBox.Show("No Contribution Found with House name " + SearchContributionText);
                    } else {
                        ContributionList.Clear();
                        foreach(var item in tempContributionDetails) {
                            MahalluManager.Model.Contribution contribution = searchSource.Find((x) => x.Id == item.Contribution_Id);
                            if(contribution != null && !ContributionList.Contains(contribution)) {
                                ContributionList.Add(contribution);
                            }
                        }
                    }
                }
            } else if(SearchByMemberName) {
                string memberName = SearchContributionText.Trim();
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    List<ContributionDetail> tempContributionDetails = unitofWork.ContributionDetails.Find((x) => x.MemberName.Contains(memberName)).ToList();
                    if(tempContributionDetails != null && tempContributionDetails.Count == 0) {
                        MessageBox.Show("No Contribution Found with Member name " + SearchContributionText);
                    } else {
                        ContributionList.Clear();
                        foreach(var item in tempContributionDetails) {
                            MahalluManager.Model.Contribution contribution = searchSource.Find((x) => x.Id == item.Contribution_Id);
                            if(contribution != null && !ContributionList.Contains(contribution)) {
                                ContributionList.Add(contribution);
                            }
                        }
                    }
                }
            } else if(SearchByCategory) {
                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(searchSource.FindAll((x) => x.CategoryName == SearchContributionText));
                if(ContributionList != null && ContributionList.Count == 0) {
                    MessageBox.Show("No Contribution Found with " + SearchContributionText);
                }
            }
        }


        private DelegateCommand clearSearchContributionCommand;
        public DelegateCommand ClearSearchContributionCommand {
            get { return clearSearchContributionCommand; }
            set { clearSearchContributionCommand = value; }
        }

        private void ExecuteClearSearchContribution() {
            if(searchSource != null && searchSource.Count != ContributionList.Count) {
                if(ContributionList != null) {
                    ContributionList.Clear();
                }
                searchSource = null;
                RefreshContribution();
            }
            SearchContributionText = String.Empty;
        }

        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now.AddMonths(2);
        }

        private void InitializeSearchPanel() {
            SearchByYear = true;
            SearchByHouseNumber = false;
            SearchByHouseName = false;
            SearchByMemberName = false;
            SearchByCategory = false;
            SearchableYears = new List<string>();
            for(int i = -10; i <= 10; i++) {
                SearchableYears.Add(DateTime.Now.AddYears(i).Year.ToString());
            }

        }

        private DateTime startDate;
        public DateTime StartDate {
            get { return startDate; }
            set {
                startDate = value;
                OnPropertyChanged("startDate");
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

        private bool isEnableCategory;
        public bool IsEnableCategory {
            get { return isEnableCategory; }
            set {
                isEnableCategory = value;
                OnPropertyChanged("IsEnableCategory");
            }
        }

        private String searchContributionText;

        public String SearchContributionText {
            get { return searchContributionText; }
            set {
                searchContributionText = value;
                OnPropertyChanged("SearchContributionText");
                SearchContributionCommand.RaiseCanExecuteChanged();
            }
        }


        private DateTime endDate;
        public DateTime EndDate {
            get { return endDate; }
            set {
                endDate = value;
                OnPropertyChanged("endtDate");
            }
        }

        private ObservableCollection<IncomeCategory> categoryList;
        public ObservableCollection<IncomeCategory> CategoryList {
            get { return categoryList; }
            set {
                categoryList = value;
                OnPropertyChanged("CategoryList");
                if(CategoryList != null) {
                    Category = CategoryList.FirstOrDefault(x => x.Name == CurrentContribution?.CategoryName);
                }
            }
        }

        private ObservableCollection<MahalluManager.Model.Contribution> contributionList;
        public ObservableCollection<MahalluManager.Model.Contribution> ContributionList {
            get { return contributionList; }
            set {
                contributionList = value;
                OnPropertyChanged("ContributionList");
            }
        }

        private ObservableCollection<ContributionDetail> contributionDetailList;
        public ObservableCollection<ContributionDetail> ContributionDetailList {
            get { return contributionDetailList; }
            set {
                contributionDetailList = value;
                OnPropertyChanged("ContributionDetailList");
            }
        }

        private MahalluManager.Model.Contribution currentContribution;
        public MahalluManager.Model.Contribution CurrentContribution {
            get { return currentContribution; }
            set {
                currentContribution = value;
                CurrentContributionChanged();
                CalculateTotalAmount();
                OnPropertyChanged("IsEnable");
                OnPropertyChanged("IsEnableCategory");
                OnPropertyChanged("Category");
                ClearContributionCommand.RaiseCanExecuteChanged();
                DeleteContributionCommand.RaiseCanExecuteChanged();
                SaveContributionCommand.RaiseCanExecuteChanged();
                NewDetailCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentContribution");
            }
        }

        private void CalculateTotalAmount() {
            if(ContributionDetailList != null && contributionDetailList.Count > 0) {
                decimal amount = 0;
                foreach(var contributionDetail in ContributionDetailList) {
                    amount += contributionDetail.Amount;
                }
                TotalAmount = amount.ToString();
            }
        }

        private ContributionDetail currentContributionDetail;

        public ContributionDetail CurrentContributionDetail {
            get { return currentContributionDetail; }
            set {
                currentContributionDetail = value;
                CurrentContributionDetailChanged();
                ClearDetailCommand.RaiseCanExecuteChanged();
                DeleteDetailCommand.RaiseCanExecuteChanged();
                SaveDetailCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentContributionDetail");
            }
        }



        private IncomeCategory category;

        public IncomeCategory Category {
            get { return category; }
            set {
                category = value;
                OnCategoryChanged();
                OnPropertyChanged("Category");
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

        private String contributionReceiptNumber;
        public String ContributionReceiptNumber {
            get { return contributionReceiptNumber; }
            set {
                contributionReceiptNumber = value;
                OnPropertyChanged("ContributionReceiptNumber");
            }
        }
        private String detailReceiptNumber;

        public String DetailReceiptNumber {
            get { return detailReceiptNumber; }
            set {
                detailReceiptNumber = value;
                OnPropertyChanged("DetailReceiptNumber");
            }
        }


        private string memberName;
        public string MemberName {
            get { return memberName; }
            set {
                memberName = value;
                OnPropertyChanged("MemberName");
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

        private string careOf;
        public string CareOf {
            get { return careOf; }
            set {
                careOf = value;
                OnPropertyChanged("CareOf");
            }
        }


        private DelegateCommand clearContributionCommand;

        public DelegateCommand ClearContributionCommand {
            get { return clearContributionCommand; }
            set { clearContributionCommand = value; }
        }

        private void ExecuteClearContribution() {
            CurrentContribution = null;
            IsEnableDetail = false;
        }

        private bool CanExecuteClearContribution() {
            return CurrentContribution != null;
        }

        private ObservableCollection<string> searchableMembers;
        public ObservableCollection<string> SearchableMembers {
            get { return searchableMembers; }
            set {
                searchableMembers = value;
                OnPropertyChanged("SearchableMembers");
            }
        }

        private List<ResidenceMember> members;

        private ObservableCollection<string> searchableIndexes;
        public ObservableCollection<string> SearchableIndexes {
            get { return searchableIndexes; }
            set {
                searchableIndexes = value;
                OnPropertyChanged("SearchableIndexes");
            }
        }

        private ObservableCollection<string> searchableMemberNames;
        public ObservableCollection<string> SearchableMemberNames {
            get { return searchableMemberNames; }
            set {
                searchableMemberNames = value;
                OnPropertyChanged("SearchableMemberNames");
            }
        }

        private ObservableCollection<string> searchableResidenceNames;
        public ObservableCollection<string> SearchableResidenceNames {
            get { return searchableResidenceNames; }
            set {
                searchableResidenceNames = value;
                OnPropertyChanged("SearchableResidenceNames");
            }
        }

        private ObservableCollection<string> searchableResidenceNumbers;
        public ObservableCollection<string> SearchableResidenceNumbers {
            get { return searchableResidenceNumbers; }
            set {
                searchableResidenceNumbers = value;
                OnPropertyChanged("SearchableResidenceNumbers");
            }
        }

        private void SetupSearchableIndexes() {
            using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                members = unitOfWork.ResidenceMembers.GetAll().ToList();
                foreach(var member in members) {
                    Residence residence = unitOfWork.Residences.Get(member.Residence_Id);
                    member.Job = residence.Name;
                    member.Qualification = residence.Number;
                }

                SearchableMemberNames = new ObservableCollection<String>(members.Select(x => x.MemberName));
                var residences = unitOfWork.Residences.GetAll();
                SearchableResidenceNames = new ObservableCollection<String>(residences.Select(x => x.Name));
                SearchableResidenceNumbers = new ObservableCollection<String>(residences.Select(x => x.Number));

                SearchableMembers = new ObservableCollection<string>(members.Select(x => x.MemberName + " \t@" + x.Job + "_" + x.Qualification + "@"));
            }
        }

        private void RefreshContribution() {
            using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(unitOfWork.Contributions.GetAll());
                if(ContributionList != null && ContributionList.Count > 0) {
                    CurrentContribution = ContributionList[0];
                } else {
                    IsEnable = true;
                    IsEnableCategory = true;
                }
            }
        }

        private void CurrentContributionChanged() {
            if(CurrentContribution != null) {
                if(CategoryList != null) {
                    Category = CategoryList.FirstOrDefault(x => x.Name == CurrentContribution.CategoryName);
                }
                TotalAmount = CurrentContribution.ToatalAmount.ToString();
                CreatedOn = CurrentContribution.CreatedOn;
                ContributionReceiptNumber = currentContribution.ReceiptNo;
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ContributionDetailList = new ObservableCollection<ContributionDetail>(unitofWork.ContributionDetails.Find((x) => x.Contribution_Id == CurrentContribution.Id));
                    if(ContributionDetailList != null && ContributionDetailList.Count > 0) {
                        CurrentContributionDetail = ContributionDetailList[0];
                    } else {
                        ClearContributionsDetailsList();
                    }
                }
                //SetGuardian();
                IsEnable = false;
                IsEnableCategory = false;
                IsEnableDetail = false;
            } else {
                IsEnable = true;
                IsEnableDetail = false;
                IsEnableCategory = true;
                ClearContribution();
                ClearContributionsDetailsList();
            }
        }



        private void ClearContribution() {
            Category = null;
            TotalAmount = ContributionReceiptNumber = string.Empty;
            CreatedOn = DateTime.Now;
        }

        private bool ValidateContribution() {
            if(Category == null ||
                String.IsNullOrEmpty(Category.Name)) {
                MessageBox.Show("Please enter Category");
                return false;
            }
            if(String.IsNullOrEmpty(TotalAmount) && !Category.DetailsRequired) {
                MessageBox.Show("Please enter Total Amount");
                return false;
            }
            _amount = 0;
            if(!String.IsNullOrEmpty(TotalAmount) && !Decimal.TryParse(TotalAmount, out _amount) && !Category.DetailsRequired) {
                MessageBox.Show("Please enter valid Total Amount");
                return false;
            }
            if(!String.IsNullOrEmpty(TotalAmount) && _amount <= 0 && !Category.DetailsRequired) {
                MessageBox.Show("Please enter Total Amount greater than zero");
                return false;
            }
            return true;
        }

        private MahalluManager.Model.Contribution GetContribution() {
            var contribution = new MahalluManager.Model.Contribution();
            if(!Category.DetailsRequired) {
                contribution.ToatalAmount = Convert.ToDecimal(TotalAmount?.Trim());
            }
            contribution.ReceiptNo = ContributionReceiptNumber?.Trim();
            contribution.CategoryName = Category.Name?.Trim();
            contribution.CreatedOn = CreatedOn;
            return contribution;
        }
        private ContributionDetail GetContributionDetails() {
            var contributionDetail = new ContributionDetail();
            if(IsMember) {
                try {
                    String temp = MemberName;
                    String memberName = temp.Substring(0, temp.IndexOf("@") - 1).Trim();
                    contributionDetail.MemberName = memberName?.Trim();

                    String houseName = temp.Substring(temp.IndexOf("@") + 1);
                    houseName = houseName.Substring(0, houseName.IndexOf("_"));
                    contributionDetail.HouserName = houseName;

                    String houseNumber = temp.Substring(temp.IndexOf("_") + 1);
                    houseNumber = houseNumber.Substring(0, houseNumber.IndexOf("@"));
                    contributionDetail.HouserNumber = Convert.ToInt32(houseNumber);

                } catch(Exception) {
                    MessageBox.Show("Please don't change anything member, unless he is an outsider");
                }
            } else {
                contributionDetail.MemberName = MemberName?.Trim();
            }

            contributionDetail.Amount = _amount;
            contributionDetail.CreatedOn = CreatedOn;
            contributionDetail.ReceiptNo = DetailReceiptNumber?.Trim();
            contributionDetail.CareOf = CareOf?.Trim();
            contributionDetail.Contribution_Id = CurrentContribution.Id;
            return contributionDetail;
        }

        private void CurrentContributionDetailChanged() {
            if(CurrentContributionDetail != null) {
                if(CurrentContributionDetail.HouserNumber > 0) {
                    IsMember = true;
                } else {
                    IsMember = false;
                }
                MemberName = CurrentContributionDetail.MemberName;
                Amount = CurrentContributionDetail.Amount.ToString();
                Date = CurrentContributionDetail.CreatedOn;
                DetailReceiptNumber = CurrentContributionDetail.ReceiptNo;
                CareOf = CurrentContributionDetail.CareOf;

                IsEnableDetail = false;
            } else {
                IsMember = true;
                IsEnableDetail = true;
                ClearContributionDetails();
            }
        }

        private void ClearContributionDetails() {
            MemberName = Amount = DetailReceiptNumber = CareOf = String.Empty;
            Date = DateTime.Now;
        }
        private void ClearContributionsDetailsList() {
            if(ContributionDetailList != null && ContributionDetailList.Count > 0) {
                ContributionDetailList.Clear();
            }
            CurrentContributionDetail = null;
        }

        private bool ValidateContributionDetail() {
            if(CurrentContribution == null) {
                MessageBox.Show("Please select contribution first to add a details");
                return false;
            }
            if(String.IsNullOrEmpty(MemberName)) {
                MessageBox.Show("Please enter Member Name");
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
        private void OnCategoryChanged() {
            if(Category != null && Category.DetailsRequired && CurrentContribution != null) {
                IsEnableDetail = true;
                IsEnable = false;
            } else if(Category != null && !Category.DetailsRequired && CurrentContribution == null) {
                IsEnableDetail = false;
                IsEnable = true;
            } else {
                IsEnable = false;
            }
            if(CurrentContribution == null) {
                ContributionReceiptNumber = string.Empty;
                TotalAmount = string.Empty;
                CreatedOn = DateTime.Now;
            }
        }
    }
}
