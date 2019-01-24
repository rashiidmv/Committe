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

namespace Administrator {
    public class NewResidentViewModel : ViewModelBase {
        public NewResidentViewModel() {
            eventAggregator.GetEvent<PubSubEvent<Area>>().Subscribe((e) => {
                AreaList.Add(e);
            });
            eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Subscribe((e) => {
                AreaList = e;
            });
            SaveResidenceCommand = new DelegateCommand(ExecuteSaveResidence, CanExecuteSaveResidence);
            SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
            ClearSearchCommand = new DelegateCommand(ExecuteClearSearch);
            SaveMemberCommand = new DelegateCommand(ExecuteSaveMember, CanExecuteSaveMember);
            CurrentResidence = new Residence();
            CurrentMember = new ResidenceMember();
            SearchByHouseNumber = true;
        }

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
                if(CurrentResidence != null) {
                    Number = CurrentResidence.Number;
                    Guardian = CurrentResidence.Guardian;
                    Name = CurrentResidence.Name;
                    Area = currentResidence.Area;
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        MemberList = new ObservableCollection<ResidenceMember>(unitofWork.ResidenceMembers.Find((x) => x.Residence_Id == CurrentResidence.Id).ToList());
                    }
                } else {
                    ClearResidence();
                }
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
                if(CurrentMember != null) {
                    MemberName = CurrentMember.Name;
                    DOB = CurrentMember.DOB;
                    Job = CurrentMember.Job;
                    Mobile = CurrentMember.Mobile;
                    IsAbroad = CurrentMember.Abroad;
                    Country = CurrentMember.Country;
                }
                OnPropertyChanged("CurrentMember");
            }
        }

        private ObservableCollection<ResidenceMember> memberList;
        public ObservableCollection<ResidenceMember> MemberList {
            get { return memberList; }
            set {
                memberList = value;
                if(MemberList != null && MemberList.Count > 0) {
                    CurrentMember = MemberList[0];
                }
                OnPropertyChanged("MemberList");
            }
        }

        private DelegateCommand saveResidenceCommand;
        public DelegateCommand SaveResidenceCommand {
            get { return saveResidenceCommand; }
            set { saveResidenceCommand = value; }
        }
        private void ExecuteSaveResidence() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                FillResidence();
                if(ValidateResidence(unitofWork)) {
                    unitofWork.Residences.Add(CurrentResidence);
                    unitofWork.Complete();
                    ClearResidence();
                }
            }
        }
        private bool CanExecuteSaveResidence() {
            return (Name != null && Name != String.Empty)
                   && (Number != null && Number != String.Empty);
        }

        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand {
            get { return searchCommand; }
            set { searchCommand = value; }
        }
        private void ExecuteSearch() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                if(SearchByHouseNumber) {
                    ResidenceList = new ObservableCollection<Residence>(unitofWork.Residences.Find((x) => x.Number == SearchText.Trim()).ToList());
                    if(ResidenceList != null && residenceList.Count == 0) {
                        MessageBox.Show("No Residence Found with House Number " + SearchText);
                    }
                } else if(SearchByHouseName) {
                    ResidenceList = new ObservableCollection<Residence>(unitofWork.Residences.Find((x) => x.Name.Contains(SearchText.Trim())).ToList());
                    if(ResidenceList != null && residenceList.Count == 0) {
                        MessageBox.Show("No Residence Found with House Name " + SearchText);
                    }
                } else if(SearchByMemberName) {
                    //  ResidenceList = unitofWork.Residences.Find((x) => x.Id.ToString().Contains(SearchText)).ToList();
                    //if(ResidenceList != null && residenceList.Count == 0) {
                    //    MessageBox.Show("No Residence Found with Member Name " + SearchText);
                    //}
                }
            }
        }
        private bool CanExecuteSearch() {
            return (SearchText != null && SearchText != String.Empty);
        }

        private DelegateCommand clearSearchCommand;
        public DelegateCommand ClearSearchCommand {
            get { return clearSearchCommand; }
            set { clearSearchCommand = value; }
        }
        private void ExecuteClearSearch() {
            ResidenceList.Clear();
            MemberList.Clear();
            SearchText = Number = Name = Guardian = MemberName = Area = DOB = Job = Mobile = Country = String.Empty;
            IsAbroad = false;
        }

        private DelegateCommand saveMemberCommand;
        public DelegateCommand SaveMemberCommand {
            get { return saveMemberCommand; }
            set { saveMemberCommand = value; }
        }
        private void ExecuteSaveMember() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                FillMember();
                unitofWork.ResidenceMembers.Add(CurrentMember);
                unitofWork.Complete();
                ClearMember();
            }
        }


        private bool CanExecuteSaveMember() {
            return (MemberName != null && MemberName != String.Empty);
        }



        private string number;
        public string Number {
            get { return number; }
            set {
                number = value;
                SaveResidenceCommand.RaiseCanExecuteChanged();
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
                SaveResidenceCommand.RaiseCanExecuteChanged();
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
                SearchCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SearchText");
            }
        }

        private string memberName;

        public string MemberName {
            get { return memberName; }
            set {
                memberName = value;
                SaveMemberCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("MemberName");
            }
        }

        private string dob;
        public string DOB {
            get { return dob; }
            set {
                dob = value;
                OnPropertyChanged("DOB");
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
                if(!String.IsNullOrEmpty(Country)) {
                    Country = string.Empty;
                }
                OnPropertyChanged("IsAbroad");
            }
        }
        private bool isGuardian;
        public bool IsGuardian {
            get { return isGuardian; }
            set {
                isGuardian = value;
                //if(!String.IsNullOrEmpty(Country)) {
                //    Country = string.Empty;
                //}
                OnPropertyChanged("IsGuardian");
            }
        }
        
        private void FillResidence() {
            CurrentResidence.Name = Name.Trim();
            CurrentResidence.Number = Number.Trim();
            CurrentResidence.Area = Area.Trim();
            CurrentResidence.Guardian = Guardian.Trim();
        }
        private bool ValidateResidence(UnitOfWork unitOfWork) {
            Residence residence = unitOfWork.Residences.Find((x) => x.Number == Number).FirstOrDefault();
            if(residence != null) {
                MessageBox.Show("House Number already exists");
                return false;
            }
            return true;
        }
        private void ClearResidence() {
            Number = Name = Guardian = Area = string.Empty;
        }


        private void FillMember() {
            if(CurrentResidence != null && CurrentResidence.Id > 0) {
                CurrentMember.Name = MemberName.Trim();
                CurrentMember.DOB = DOB.Trim();
                CurrentMember.Job = Job.Trim();
                CurrentMember.Mobile = Mobile.Trim();
                CurrentMember.Abroad = IsAbroad;
                if(IsAbroad) {
                    CurrentMember.Country = Country.Trim();
                }
                CurrentMember.Residence_Id = CurrentResidence.Id;
            }
        }
        private void ClearMember() {
            MemberName = DOB = Job = Mobile = Country = string.Empty;
            IsAbroad = false;
        }
    }
}
