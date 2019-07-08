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

namespace Resident {
    public class NewResidentViewModel : ViewModelBase {
        public NewResidentViewModel() {
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Subscribe((e) => {
                AreaList = e;
            });

            SaveResidenceCommand = new DelegateCommand(ExecuteSaveResidence, CanExecuteSaveResidence);
            ClearResidenceCommand = new DelegateCommand(ExecuteClearResidence, CanExecuteClearResidence);
            SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
            ClearSearchCommand = new DelegateCommand(ExecuteClearSearch);
            SaveMemberCommand = new DelegateCommand(ExecuteSaveMember, CanExecuteSaveMember);
            ClearMemberCommand = new DelegateCommand(ExecuteClearMember, CanExecuteClearMember);
            NewResidenceCommand = new DelegateCommand(ExecuteNewResidence);
            DeleteResidenceCommand = new DelegateCommand(ExecuteDeleteResidence, CanExecuteDeleteResidence);

            NewMemberCommand = new DelegateCommand(ExecuteNewMember);
            DeleteMemberCommand = new DelegateCommand(ExecuteDeleteMember, CanExecuteDeleteMember);
            RefreshResidence();
            SearchByHouseName = true;
            InitializeDatePicker();
        }

        public List<String> MarriageStatuses {
            get {
                return new List<String>() { "Married", "Batchelor", "Spinster", "Widower", "Widow", "Divorcee" };
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

        private List<Residence> searchSource = null;

        private ObservableCollection<Area> areaList;
        public ObservableCollection<Area> AreaList {
            get { return areaList; }
            set {
                areaList = value;
                OnPropertyChanged("AreaList");
            }
        }

        private Residence currentResidence;
        public Residence CurrentResidence {
            get { return currentResidence; }
            set {
                currentResidence = value;
                CurrentResidenceChanged();
                OnPropertyChanged("IsEdit");
                ClearResidenceCommand.RaiseCanExecuteChanged();
                DeleteResidenceCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentResidence");
            }
        }

        private ObservableCollection<Residence> residenceList;
        public ObservableCollection<Residence> ResidenceList {
            get { return residenceList; }
            set {
                residenceList = value;
                OnPropertyChanged("ResidenceList");
            }
        }

        private ResidenceMember currentMember;
        public ResidenceMember CurrentMember {
            get { return currentMember; }
            set {
                currentMember = value;
                CurrentMemberChanged();
                ClearMemberCommand.RaiseCanExecuteChanged();
                DeleteMemberCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentMember");
                OnPropertyChanged("EnbalbeIsGuardian");
            }
        }

        private ObservableCollection<ResidenceMember> memberList;
        public ObservableCollection<ResidenceMember> MemberList {
            get { return memberList; }
            set {
                memberList = value;
                OnPropertyChanged("MemberList");
            }
        }

        private DelegateCommand saveResidenceCommand;
        public DelegateCommand SaveResidenceCommand {
            get { return saveResidenceCommand; }
            set { saveResidenceCommand = value; }
        }
        private void ExecuteSaveResidence() {
            using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                if(ValidateResidence()) {
                    Residence residence = GetResidence();
                    if(CurrentResidence != null) {
                        CurrentResidence.Name = residence.Name;
                        CurrentResidence.Area = residence.Area;
                        unitOfWork.Residences.Update(CurrentResidence);
                        MessageBox.Show(CurrentResidence.Name + " updated successfully !", "New Residence", MessageBoxButton.OK, MessageBoxImage.Information);

                    } else {
                        if(CurrentResidence == null && IsHouserNumberExists(unitOfWork)) {
                            return;
                        }
                        unitOfWork.Residences.Add(residence);
                        ResidenceList.Add(residence);
                        CurrentResidence = residence;
                        MessageBox.Show(CurrentResidence.Name + " added successfully !", "New Residence", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    unitOfWork.Complete();
                    ResidenceType residenceType = new ResidenceType { Residence = CurrentResidence, Operation = MahalluManager.Model.Common.Operation.Add };
                    eventAggregator.GetEvent<PubSubEvent<ResidenceType>>().Publish(residenceType);
                }
            }
        }
        private bool CanExecuteSaveResidence() {
            return true;
        }

        private DelegateCommand clearResidenceCommand;
        public DelegateCommand ClearResidenceCommand {
            get { return clearResidenceCommand; }
            set { clearResidenceCommand = value; }
        }
        private void ExecuteClearResidence() {
            CurrentResidence = null;
        }
        private bool CanExecuteClearResidence() {
            return CurrentResidence != null;
        }
        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand {
            get { return searchCommand; }
            set { searchCommand = value; }
        }
        private void ExecuteSearch() {
            RefreshResidence();
            searchSource = ResidenceList.ToList(); ;
            if(SearchByHouseNumber) {
                ResidenceList = new ObservableCollection<Residence>(searchSource.FindAll((x) => x.Number == SearchText.Trim()));//.ToList());
                if(ResidenceList != null && residenceList.Count == 0) {

                    MessageBox.Show("No Residence Found with House Number " + SearchText);
                }
            } else if(SearchByHouseName) {
                ResidenceList = new ObservableCollection<Residence>(searchSource.FindAll((x) => x.Name.ToLower().Contains(SearchText.Trim().ToLower())));//.ToList());
                if(ResidenceList != null && residenceList.Count == 0) {
                    MessageBox.Show("No Residence Found with House Name " + SearchText);
                }
            } else if(SearchByMemberName) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    List<ResidenceMember> tempResidenceMembers = unitofWork.ResidenceMembers.Find((x) => x.MemberName.ToLower().Contains(SearchText.Trim().ToLower())).ToList();
                    if(tempResidenceMembers != null && tempResidenceMembers.Count == 0) {
                        MessageBox.Show("No Residence Found with Member Name " + SearchText);
                    } else {
                        ResidenceList.Clear();
                        foreach(var item in tempResidenceMembers) {
                            Residence residence = searchSource.Find((x) => x.Id == item.Residence_Id);
                            if(residence != null && !ResidenceList.Contains(residence)) {
                                ResidenceList.Add(residence);
                            }
                        }
                    }
                }
            }
        }
        private bool CanExecuteSearch() {
            return ResidenceList != null && !String.IsNullOrEmpty(SearchText);
        }

        private DelegateCommand clearSearchCommand;
        public DelegateCommand ClearSearchCommand {
            get { return clearSearchCommand; }
            set { clearSearchCommand = value; }
        }
        private void ExecuteClearSearch() {
            if(ResidenceList != null) {
                ResidenceList.Clear();
                SearchText = String.Empty;
            }
            RefreshResidence();
        }

        private void RefreshResidence() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                ResidenceList = new ObservableCollection<Residence>(unitofWork.Residences.GetAll());

                if(ResidenceList != null && residenceList.Count > 0) {
                    CurrentResidence = ResidenceList[0];
                }
            }
        }

        private DelegateCommand saveMemberCommand;
        public DelegateCommand SaveMemberCommand {
            get { return saveMemberCommand; }
            set { saveMemberCommand = value; }
        }
        private void ExecuteSaveMember() {
            using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                if(ValidateResidenceMember(unitOfWork)) {
                    ResidenceMember residenceMember = GetResidenceMember();
                    if(CurrentMember != null) {
                        CurrentMember.MemberName = residenceMember.MemberName;
                        CurrentMember.DOB = residenceMember.DOB;
                        CurrentMember.Job = residenceMember.Job;
                        CurrentMember.Mobile = residenceMember.Mobile;
                        CurrentMember.Abroad = residenceMember.Abroad;
                        CurrentMember.Country = residenceMember.Country;
                        CurrentMember.IsGuardian = residenceMember.IsGuardian;
                        CurrentMember.MarriageStatus = residenceMember.MarriageStatus;
                        CurrentMember.Qualification = residenceMember.Qualification;
                        CurrentMember.Gender = residenceMember.Gender;
                        unitOfWork.ResidenceMembers.Update(CurrentMember);
                        MessageBox.Show(CurrentMember.MemberName + " updated successfully !", "New Member", MessageBoxButton.OK, MessageBoxImage.Information);
                    } else {
                        unitOfWork.ResidenceMembers.Add(residenceMember);
                        MemberList.Add(residenceMember);
                        CurrentMember = residenceMember;
                        MessageBox.Show(CurrentMember.MemberName + " added successfully !", "New Member", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    unitOfWork.Complete();

                    ResidenceMemberType residenceMemberType = new ResidenceMemberType {
                        ResidenceMember = CurrentMember,
                        Operation = MahalluManager.Model.Common.Operation.Add
                    };
                    eventAggregator.GetEvent<PubSubEvent<ResidenceMemberType>>().Publish(residenceMemberType);
                }
            }
        }

        private bool CanExecuteSaveMember() {
            return true;
        }
        private DelegateCommand newMemberCommand;

        public DelegateCommand NewMemberCommand {
            get { return newMemberCommand; }
            set { newMemberCommand = value; }
        }
        private void ExecuteNewMember() {
            CurrentMember = null;
        }
        private DelegateCommand deleteMemberCommand;

        public DelegateCommand DeleteMemberCommand {
            get { return deleteMemberCommand; }
            set { deleteMemberCommand = value; }
        }
        private void ExecuteDeleteMember() {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete " + currentMember.MemberName, "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(currentMember != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        ResidenceMember member = unitofWork.ResidenceMembers.Get(CurrentMember.Id);
                        if(member != null) {
                            unitofWork.ResidenceMembers.Remove(member);
                            unitofWork.Complete();

                            ResidenceMemberType residenceMemberType = new ResidenceMemberType {
                                ResidenceMember = CurrentMember,
                                Operation = MahalluManager.Model.Common.Operation.Delete
                            };
                            eventAggregator.GetEvent<PubSubEvent<ResidenceMemberType>>().Publish(residenceMemberType);

                            MemberList.Remove(CurrentMember);
                            CurrentMember = null;
                        }
                    }
                }
            }
        }
        private bool CanExecuteDeleteMember() {
            return CurrentMember != null;
        }

        private DelegateCommand newResidenceCommand;
        public DelegateCommand NewResidenceCommand {
            get { return newResidenceCommand; }
            set { newResidenceCommand = value; }
        }
        private void ExecuteNewResidence() {
            CurrentResidence = null;
        }
        private DelegateCommand deleteResidenceCommand;
        public DelegateCommand DeleteResidenceCommand {
            get { return deleteResidenceCommand; }
            set { deleteResidenceCommand = value; }
        }
        private void ExecuteDeleteResidence() {
            MessageBoxResult result = MessageBox.Show("Deleting Residence will delete all of the members also, \nAre you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentResidence != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        List<ResidenceMember> members = new List<ResidenceMember>(unitofWork.ResidenceMembers.Find((x) => x.Residence_Id == CurrentResidence.Id));
                        unitofWork.ResidenceMembers.RemoveRange(members);
                        Residence residence = unitofWork.Residences.Get(CurrentResidence.Id);
                        unitofWork.Residences.Remove(residence);
                        unitofWork.Complete();

                        ResidenceType residenceType = new ResidenceType {
                            Residence = CurrentResidence,
                            Operation = MahalluManager.Model.Common.Operation.Delete
                        };
                        eventAggregator.GetEvent<PubSubEvent<ResidenceType>>().Publish(residenceType);

                        ResidenceList.Remove(CurrentResidence);
                        CurrentResidence = null;
                    }
                }
            }
        }
        private bool CanExecuteDeleteResidence() {
            return CurrentResidence != null;
        }
        private void ExecuteEditResidence() {
        }
        private bool CanExecuteEditResidence() {
            return CurrentResidence?.Id != 0;
        }


        private DelegateCommand clearMemberCommand;
        public DelegateCommand ClearMemberCommand {
            get { return clearMemberCommand; }
            set { clearMemberCommand = value; }
        }
        private void ExecuteClearMember() {
            CurrentMember = null;
        }
        private bool CanExecuteClearMember() {
            return CurrentMember != null;
        }

        private string number;
        public string Number {
            get { return number; }
            set {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        private string guardian;
        public string Guardian {
            get { return guardian; }
            set {
                guardian = value;
                OnPropertyChanged("Guardian");
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

        private string area;
        public string Area {
            get { return area; }
            set {
                area = value;
                OnPropertyChanged("Area");
            }
        }

        private bool searchByHouseNumber;
        public bool SearchByHouseNumber {
            get { return searchByHouseNumber; }
            set {
                searchByHouseNumber = value;
                OnPropertyChanged("SearchByHouseNumber");
            }
        }

        private bool searchByHouseName;
        public bool SearchByHouseName {
            get { return searchByHouseName; }
            set {
                searchByHouseName = value;
                OnPropertyChanged("SearchByHouseName");
            }
        }

        private bool searchByMemberName;
        public bool SearchByMemberName {
            get { return searchByMemberName; }
            set {
                searchByMemberName = value;
                OnPropertyChanged("SearchByMemberName");
            }
        }

        private string searchText;
        public string SearchText {
            get { return searchText; }
            set {
                searchText = value;
                OnPropertyChanged("SearchText");
                SearchCommand.RaiseCanExecuteChanged();
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

        private DateTime dob;
        public DateTime DOB {
            get {
                if(dob == DateTime.MinValue)
                    return DateTime.Now;
                return dob;
            }
            set {
                dob = value;
                OnPropertyChanged("DOB");
                OnPropertyChanged("Age");
            }
        }

        private string job;
        public string Job {
            get { return job; }
            set {
                job = value;
                OnPropertyChanged("Job");
            }
        }

        private string mobile;
        public string Mobile {
            get { return mobile; }
            set {
                mobile = value;
                OnPropertyChanged("Mobile");
            }
        }

        private string country;
        public string Country {
            get { return country; }
            set {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        private bool isAbroad;
        public bool IsAbroad {
            get { return isAbroad; }
            set {
                isAbroad = value;
                Country = string.Empty;
                OnPropertyChanged("IsAbroad");
            }
        }
        private bool isGuardian;
        public bool IsGuardian {
            get { return isGuardian; }
            set {
                isGuardian = value;
                OnPropertyChanged("IsGuardian");
            }
        }

        private bool male;
        public bool Male {
            get { return male; }
            set {
                male = value;
                if(male == true) {
                    Female = Other = false;
                }
                OnPropertyChanged("Male");
            }
        }

        private bool female;
        public bool Female {
            get { return female; }
            set {
                female = value;
                if(female == true) {
                    Male = Other = false;
                }
                OnPropertyChanged("Female");
            }
        }

        private bool other;

        public bool Other {
            get { return other; }
            set {
                other = value;
                if(Other == true) {
                    Male = Female = false;
                }
                OnPropertyChanged("Other");
            }
        }

        private String age;
        public String Age {
            get {
                int year = DateTime.Now.Year - DOB.Year;
                if(year > 0) {
                    return year.ToString();
                } else {
                    return (DateTime.Now.Month - DOB.Month).ToString() + " Months";
                }
            }
            set {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        private string marriageStatus;
        public string MarriageStatus {
            get { return marriageStatus; }
            set {
                marriageStatus = value;
                OnPropertyChanged("MarriageStatus");
            }
        }

        private String qualification;
        public String Qualification {
            get { return qualification; }
            set {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }



        private bool enbalbeIsGuardian;
        public bool EnbalbeIsGuardian {
            get {
                bool guardianSet = false;
                if(MemberList != null) {

                    foreach(var member in MemberList) {
                        if(member.IsGuardian) {
                            guardianSet = true;
                            if(CurrentMember != null && CurrentMember.Id == member.Id) {
                                return true;
                            }
                        }
                    }
                    if(guardianSet) {
                        return false;
                    }
                }
                return true;
            }
            set {
                enbalbeIsGuardian = value;
                OnPropertyChanged("EnbalbeIsGuardian");
            }
        }
        private bool disbalbeAllResidenceFields;
        public bool DisbalbeAllResidenceFields {
            get { return disbalbeAllResidenceFields; }
            set {
                disbalbeAllResidenceFields = value;
                OnPropertyChanged("DisbalbeAllResidenceFields");
            }
        }
        private bool isEdit;

        public bool IsEdit {
            get { return CurrentResidence == null; }
            set {
                isEdit = value;
                OnPropertyChanged("IsEdit");
            }
        }



        private void CurrentResidenceChanged() {
            if(CurrentResidence != null) {
                Number = CurrentResidence.Number;
                Name = CurrentResidence.Name;
                Area = currentResidence.Area;
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    MemberList = new ObservableCollection<ResidenceMember>(unitofWork.ResidenceMembers.Find((x) => x.Residence_Id == CurrentResidence.Id));
                    if(MemberList != null && MemberList.Count > 0) {
                        CurrentMember = MemberList[0];
                    } else {
                        ClearResidenceDetails();
                    }
                }
                SetGuardian();
            } else {
                ClearResidence();
                ClearResidenceDetails();
            }
        }
        private void SetGuardian() {
            if(MemberList != null && MemberList.Count > 0) {
                foreach(var member in MemberList) {
                    if(member.IsGuardian) {
                        Guardian = member.MemberName;
                        return;
                    }
                }

            }
            Guardian = string.Empty;
        }
        private void ClearResidenceDetails() {
            MemberList.Clear();
            CurrentMember = null;
        }
        private void ClearResidence() {
            Number = Name = Guardian = Area = string.Empty;
        }
        private void CurrentMemberChanged() {
            if(CurrentMember != null) {
                MemberName = CurrentMember.MemberName;
                DOB = CurrentMember.DOB;
                Job = CurrentMember.Job;
                Mobile = currentMember.Mobile;
                IsAbroad = CurrentMember.Abroad;
                Country = CurrentMember.Country;
                IsGuardian = CurrentMember.IsGuardian;
                if(CurrentMember.Gender.Equals("Male"))
                    Male = true;
                else if(CurrentMember.Gender.Equals("Female"))
                    Female = true;
                else
                    Other = true;
                MarriageStatus = CurrentMember.MarriageStatus;
                Qualification = CurrentMember.Qualification;
                SetGuardian();
            } else {
                ClearMemberDetails();
            }
        }

        private void ClearMemberDetails() {
            MemberName = String.Empty;
            DOB = DateTime.Now;
            Job = String.Empty;
            Mobile = String.Empty;
            IsAbroad = false;
            IsGuardian = false;
            Country = MarriageStatus = qualification = String.Empty;

        }

        private Residence GetResidence() {
            var residence = new Residence();
            residence.Name = Name?.Trim();
            residence.Number = Number?.Trim();
            residence.Area = Area?.Trim();
            return residence;
        }
        private ResidenceMember GetResidenceMember() {
            var residenceMember = new ResidenceMember();
            residenceMember.MemberName = MemberName.Trim();
            residenceMember.DOB = DOB;
            residenceMember.Job = Job?.Trim();
            residenceMember.Mobile = Mobile?.Trim();
            residenceMember.Abroad = IsAbroad;
            if(IsAbroad) {
                residenceMember.Country = Country?.Trim();
            }
            residenceMember.IsGuardian = IsGuardian;
            String gender = String.Empty;
            if(Male) {
                gender = "Male";
            } else if(Female) {
                gender = "Female";
            } else {
                gender = "Other";
            }
            residenceMember.Gender = gender;
            residenceMember.MarriageStatus = MarriageStatus;
            residenceMember.Qualification = Qualification;
            residenceMember.Residence_Id = CurrentResidence.Id;
            return residenceMember;
        }

        private bool ValidateResidence() {
            if(String.IsNullOrEmpty(Number) ||
                String.IsNullOrEmpty(Name)) {
                MessageBox.Show("Please enter house number and name");
                return false;
            }
            return true;
        }

        private bool IsHouserNumberExists(UnitOfWork unitOfWork) {
            Residence residence = unitOfWork.Residences.Find((x) => x.Number == Number).FirstOrDefault();
            if(residence != null) {
                MessageBox.Show("House Number already exists for " + residence.Name);
                return true;
            }
            return false;
        }
        private bool ValidateResidenceMember(UnitOfWork unitOfWork) {
            if(CurrentResidence == null) {
                MessageBox.Show("Please select residence first to add a member");
                return false;
            }
            if(String.IsNullOrEmpty(MemberName)) {
                MessageBox.Show("Please enter member name");
                return false;
            }
            if(!Male && !Female && !Other) {
                MessageBox.Show("Please enter gender");
                return false;
            }
            if(String.IsNullOrEmpty(MarriageStatus)) {
                MessageBox.Show("Please enter marriage status");
                return false;
            }
            if(CurrentMember == null) {
                ResidenceMember residenceMember = unitOfWork.ResidenceMembers.Find((x) => x.MemberName == MemberName
                                                    && x.Residence_Id == CurrentResidence.Id).FirstOrDefault();
                if(residenceMember != null) {
                    MessageBox.Show("Member already exists");
                    return false;
                }
            }
            return true;
        }
        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddYears(-110);
            EndDate = DateTime.Now;
        }

    }
}
