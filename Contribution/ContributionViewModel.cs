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

namespace Contribution {
    public class ContributionViewModel : ViewModelBase {
        public ContributionViewModel() {
            eventAggregator.GetEvent<PubSubEvent<Category>>().Subscribe((e) => {
                CategoryList.Add(e);
            });
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Category>>>().Subscribe((e) => {
                CategoryList = e;
            });
            ClearContributionCommand = new DelegateCommand(ExecuteClearContribution, CanExecuteClearContribution);
            SearchContributionCommand = new DelegateCommand(ExecuteSearchContribution, CanExecuteSearchContribution);
            ClearSearchContributionCommand = new DelegateCommand(ExecuteClearSearchContribution);
            DeleteContributionCommand = new DelegateCommand(ExecuteDeleteContribution, CanExecuteDeleteContribution);
            NewContributionCommand = new DelegateCommand(ExecuteNewContribution);
            SaveContributionCommand = new DelegateCommand(ExecuteSaveContribution, CanExecuteSaveContribution);


            ClearDetailCommand = new DelegateCommand(ExecuteClearDetail, CanExecuteClearDetail);
            DeleteDetailCommand = new DelegateCommand(ExecueDeleteDetail, CanExecueDeleteDetail);
            NewDetailCommand = new DelegateCommand(ExecuteNewDetail);
            SaveDetailCommand = new DelegateCommand(ExecuteSaveDetail, CanExecuteSaveDetail);
            IsMember = true;
            RefreshContribution();
            SearchableMembers = members.Select(x => x.Name + " \t@" + x.Job + "_" + x.DOB + "@").ToList();
            InitializeDatePicker();
            InitializeSearchPanel();
        }

        private IEnumerable<ResidenceMember> members = null;

        private bool disableDetail;
        public bool DisableDetail {
            get { return disableDetail; }
            set {
                disableDetail = value;
                OnPropertyChanged("DisableDetail");
            }
        }


        private bool isMember;
        public bool IsMember {
            get { return isMember; }
            set {
                isMember = value;
                MemberName = String.Empty;
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
                return (SearchByHouseNumber || SearchByMemberName) ? Visibility.Visible : Visibility.Collapsed;
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
                    SearchByHouseNumber = SearchByMemberName = SearchByCategory = false;
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
                    SearchByYear = SearchByMemberName = SearchByCategory = false;
                    SearchContributionText = String.Empty;
                }
                OnPropertyChanged("SearchByHouseNumber");
                OnPropertyChanged("ShowOtherSearch");

            }
        }

        private bool searchByMemberName;
        public bool SearchByMemberName {
            get { return searchByMemberName; }
            set {
                searchByMemberName = value;
                if(searchByMemberName) {
                    SearchByYear = SearchByHouseNumber = SearchByCategory = false;
                    SearchContributionText = String.Empty;
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
            return CurrentContributionDetail == null;
        }
        private void ExecuteSaveDetail() {
            //if(CurrentContribution != null) {
            //    MessageBoxResult result = MessageBox.Show("You can't edit existing contribution detail,\nPlease click on 'New Detail' button to create new contributions detail ", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            if(ValidateContributionDetail()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    ContributionDetail contributionDetail = GetContributionDetails();
                    //if(CurrentContributionDetail != null) {
                    //    ResidenceMember existingMember = unitOfWork.ResidenceMembers.Find((x) => x.Id == CurrentContributionDetail.Id).FirstOrDefault();
                    //    if(existingMember != null) {
                    //        unitOfWork.ResidenceMembers.Remove(existingMember);
                    //        ContributionDetailList.Remove(C);
                    //    }
                    //}
                    unitOfWork.ContributionDetails.Add(contributionDetail);
                    unitOfWork.Complete();
                    ContributionDetailList.Add(contributionDetail);
                    CurrentContributionDetail = contributionDetail;
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

        private DelegateCommand clearDetailCommand;
        public DelegateCommand ClearDetailCommand {
            get { return clearDetailCommand; }
            set { clearDetailCommand = value; }
        }

        private bool CanExecuteClearDetail() {
            return CurrentContributionDetail != null;
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
            return CurrentContributionDetail != null;
        }

        private void ExecueDeleteDetail() {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete " + CurrentContributionDetail.MemberName, "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentContributionDetail != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        ContributionDetail contributionDetail = unitofWork.ContributionDetails.Get(CurrentContributionDetail.Id);
                        if(contributionDetail != null) {
                            unitofWork.ContributionDetails.Remove(contributionDetail);
                            unitofWork.Complete();

                            ContributionDetailList.Remove(CurrentContributionDetail);
                            CurrentContributionDetail = null;
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
            //if(CurrentContribution != null) {
            //    MessageBoxResult result = MessageBox.Show("You can't edit existing contribution,\nPlease click on 'New Contribution' button to create new contributions ", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            if(ValidateContribution()) {
                using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                    MahalluManager.Model.Contribution contribution = GetContribution();
                    // List<ResidenceMember> tempMemberList = null;
                    //if(CurrentContribution != null) {

                    //    Residence existingResidence = unitOfWork.Residences.Find((x) => x.Id == CurrentResidence.Id).FirstOrDefault();
                    //    if(existingResidence != null) {
                    //        tempMemberList = new List<ResidenceMember>();
                    //        foreach(var item in MemberList) {
                    //            tempMemberList.Add(item);
                    //        }
                    //        unitOfWork.Residences.Remove(existingResidence);
                    //        ResidenceList.Remove(CurrentResidence);
                    //    }

                    //}
                    //if(CurrentContribution == null && IsHouserNumberExists(unitOfWork)) {
                    //    return;
                    //}
                    unitOfWork.Contributions.Add(contribution);
                    //if(tempMemberList != null && tempMemberList.Count > 0) {
                    //    foreach(var item in tempMemberList) {
                    //        item.Residence_Id = residence.Id;
                    //    }
                    //    unitOfWork.ResidenceMembers.AddRange(tempMemberList);
                    //}
                    unitOfWork.Complete();
                    ContributionList.Add(contribution);
                    CurrentContribution = contribution;
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
                        //List<ResidenceMember> members = new List<ResidenceMember>(unitofWork.ResidenceMembers.Find((x) => x.Residence_Id == CurrentResidence.Id));
                        //unitofWork.ResidenceMembers.RemoveRange(members);
                        MahalluManager.Model.Contribution contribution = unitofWork.Contributions.Get(CurrentContribution.Id);
                        unitofWork.Contributions.Remove(contribution);
                        unitofWork.Complete();

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
            //throw new NotImplementedException();
        }

        private DelegateCommand clearSearchContributionCommand;
        public DelegateCommand ClearSearchContributionCommand {
            get { return clearSearchContributionCommand; }
            set { clearSearchContributionCommand = value; }
        }

        private void ExecuteClearSearchContribution() {
            if(ContributionList != null) {
                ContributionList.Clear();
                SearchContributionText = String.Empty;
            }
            RefreshContribution();
        }

        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now.AddMonths(2);
        }

        private void InitializeSearchPanel() {
            SearchByYear = true;
            SearchByHouseNumber = false;
            SearchByMemberName = false;
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
            set { isEnable = value; }
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

        private ObservableCollection<Category> categoryList;
        public ObservableCollection<Category> CategoryList {
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
                OnPropertyChanged("IsEnable");
                ClearContributionCommand.RaiseCanExecuteChanged();
                DeleteContributionCommand.RaiseCanExecuteChanged();
                SaveContributionCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentContribution");
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
                //OnPropertyChanged("EnbalbeIsGuardian");
            }
        }



        private Category category;

        public Category Category {
            get { return category; }
            set {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        //private Category categoryDetail;
        //public Category CategoryDetail {
        //    get { return categoryDetail; }
        //    set {
        //        categoryDetail = value;
        //        OnPropertyChanged("CategoryId");
        //    }
        //}


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

        private String receiptNumber;

        public String ReceiptNumber {
            get { return receiptNumber; }
            set {
                receiptNumber = value;
                OnPropertyChanged("ReceiptNumber");
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

        private string date;

        public string Date {
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
        }

        private bool CanExecuteClearContribution() {
            return CurrentContribution != null;
        }

        private IEnumerable<string> searchableMembers;

        public IEnumerable<string> SearchableMembers {
            get { return searchableMembers; }
            set { searchableMembers = value; }
        }

        private void RefreshContribution() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                members = unitofWork.ResidenceMembers.GetAll();
                foreach(var member in members) {
                    Residence residence = unitofWork.Residences.Get(member.Residence_Id);
                    member.Job = residence.Name;
                    member.DOB = residence.Number;
                }

                ContributionList = new ObservableCollection<MahalluManager.Model.Contribution>(unitofWork.Contributions.GetAll());
                if(ContributionList != null && ContributionList.Count > 0) {
                    CurrentContribution = ContributionList[0];
                } else {
                    IsEnable = true;
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
                ReceiptNumber = currentContribution.ReceiptNo;
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
            } else {
                IsEnable = true;
                ClearContribution();
                ClearContributionsDetailsList();
            }
        }



        private void ClearContribution() {
            Category = null;
            TotalAmount = ReceiptNumber = string.Empty;
            CreatedOn = DateTime.Now;
        }

        private bool ValidateContribution() {
            if(Category == null ||
                String.IsNullOrEmpty(Category.Name) ||
                String.IsNullOrEmpty(TotalAmount)) {
                MessageBox.Show("Please enter Category and Total Amount");
                return false;
            }
            return true;
        }

        private MahalluManager.Model.Contribution GetContribution() {
            var contribution = new MahalluManager.Model.Contribution();
            contribution.ToatalAmount = Convert.ToDecimal(TotalAmount?.Trim());
            contribution.ReceiptNo = ReceiptNumber?.Trim();
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

            //contributionDetail.Amount = Amount.Trim();
            contributionDetail.CreatedOn = CreatedOn;
            contributionDetail.ReceiptNo = ReceiptNumber?.Trim();
            contributionDetail.CareOf = CareOf?.Trim();
            contributionDetail.Contribution_Id = 1;// currentContribution.Id;
            return contributionDetail;
        }

        private void CurrentContributionDetailChanged() {
            if(CurrentContributionDetail != null) {
                MemberName = CurrentContributionDetail.MemberName;
                Amount = CurrentContributionDetail.Amount.ToString();
                Date = CurrentContributionDetail.CreatedOn.ToString(); ;
                ReceiptNumber = CurrentContributionDetail.ReceiptNo;
                CareOf = CurrentContributionDetail.CareOf;
                if(CurrentContributionDetail.HouserNumber > 0) {
                    IsMember = true;
                } else {
                    IsMember = false;
                }
                DisableDetail = false;
            } else {
                IsMember = true;
                DisableDetail = true;
                ClearContributionDetails();
            }
        }

        private void ClearContributionDetails() {
            MemberName = Amount = Date = ReceiptNumber = CareOf = String.Empty;
        }
        private void ClearContributionsDetailsList() {
            ContributionDetailList.Clear();
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
            return true;
        }
    }
}
