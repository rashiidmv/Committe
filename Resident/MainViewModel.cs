using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.Common;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Resident {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        public MainViewModel() {
            ShowReportCommand = new DelegateCommand(ExecuteShowReport);
            PrintReportCommand = new DelegateCommand(ExecutePrintReport);
            ResidenceColumns = new ObservableCollection<string>() { "House Number","House Name", "Area",
                "Name","Date of Birth", "Marriage Status", "Gender",
                "Job", "Mobile","Country","Guardian","Qualification"
            };

            ClearCountry = new DelegateCommand(ExecuteClearCountry, CanExecuteClearCountry);
            ClearMarriageStatus = new DelegateCommand(ExecuteClearMarriageStatus, CanExecuteClearMarriageStatus);
            ClearArea = new DelegateCommand(ExecuteClearArea, CanExecuteClearArea);
            ClearHouseName = new DelegateCommand(ExecuteClearHouseName, CanExecuteClearHouseName);
            ClearHouseNumber = new DelegateCommand(ExecuteClearHouseNumber, CanExecuteClearHouseNumber);
            ClearQualification = new DelegateCommand(ExecuteClearQualification, CanExecuteClearQualification);
            ClearGender = new DelegateCommand(ExecuteClearGender, CanExecuteClearGender);

            SelectedResidenceColumns = new ObservableCollection<string>();
        }

        private DelegateCommand clearCountry;
        public DelegateCommand ClearCountry {
            get { return clearCountry; }
            set { clearCountry = value; }
        }
        private bool CanExecuteClearCountry() {
            return !String.IsNullOrEmpty(Country);
        }
        private void ExecuteClearCountry() {
            Country = String.Empty;
        }

        private DelegateCommand clearMarriageStatus;
        public DelegateCommand ClearMarriageStatus {
            get { return clearMarriageStatus; }
            set { clearMarriageStatus = value; }
        }
        private bool CanExecuteClearMarriageStatus() {
            return !String.IsNullOrEmpty(MarriageStatus);
        }
        private void ExecuteClearMarriageStatus() {
            MarriageStatus = String.Empty;
        }

        private DelegateCommand clearArea;
        public DelegateCommand ClearArea {
            get { return clearArea; }
            set { clearArea = value; }
        }
        private void ExecuteClearArea() {
            Area = String.Empty;
        }
        private bool CanExecuteClearArea() {
            return !String.IsNullOrEmpty(Area);
        }
        private DelegateCommand clearHouseName;
        public DelegateCommand ClearHouseName {
            get { return clearHouseName; }
            set { clearHouseName = value; }
        }
        private void ExecuteClearHouseName() {
            HouseName = String.Empty;
        }
        private bool CanExecuteClearHouseName() {
            return !String.IsNullOrEmpty(HouseName);
        }
        private DelegateCommand clearHouseNumber;
        public DelegateCommand ClearHouseNumber {
            get { return clearHouseNumber; }
            set { clearHouseNumber = value; }
        }
        private void ExecuteClearHouseNumber() {
            HouseNumber = String.Empty;
        }
        private bool CanExecuteClearHouseNumber() {
            return !String.IsNullOrEmpty(HouseNumber);
        }
        private DelegateCommand clearQualification;
        public DelegateCommand ClearQualification {
            get { return clearQualification; }
            set { clearQualification = value; }
        }
        private void ExecuteClearQualification() {
            Qualification = String.Empty;
        }
        private bool CanExecuteClearQualification() {
            return !String.IsNullOrEmpty(Qualification);
        }
        private DelegateCommand clearGender;
        public DelegateCommand ClearGender {
            get { return clearGender; }
            set { clearGender = value; }
        }
        private void ExecuteClearGender() {
            Gender = String.Empty;
        }
        private bool CanExecuteClearGender() {
            return !String.IsNullOrEmpty(Gender);
        }

        private String country;

        public String Country {
            get { return country; }
            set {
                country = value;
                OnPropertyChanged("Country");
                ClearCountry.RaiseCanExecuteChanged();
            }
        }

        private string marriageStatus;
        public string MarriageStatus {
            get { return marriageStatus; }
            set {
                marriageStatus = value;
                OnPropertyChanged("MarriageStatus");
                ClearMarriageStatus.RaiseCanExecuteChanged();
            }
        }
        private string houseName;
        public string HouseName {
            get { return houseName; }
            set {
                houseName = value;
                OnPropertyChanged("HouseName");
                ClearHouseName.RaiseCanExecuteChanged();
            }
        }
        private string houseNumber;
        public string HouseNumber {
            get { return houseNumber; }
            set {
                houseNumber = value;
                OnPropertyChanged("HouseNumber");
                ClearHouseNumber.RaiseCanExecuteChanged();
            }
        }
        private string qualification;
        public string Qualification {
            get { return qualification; }
            set {
                qualification = value;
                OnPropertyChanged("Qualification");
                ClearQualification.RaiseCanExecuteChanged();
            }
        }
        private string gender;
        public string Gender {
            get { return gender; }
            set {
                gender = value;
                OnPropertyChanged("Gender");
                ClearGender.RaiseCanExecuteChanged();
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


        private bool showHeader;
        public bool ShowHeader {
            get { return showHeader; }
            set {
                showHeader = value;
                OnPropertyChanged("ShowHeader");
            }
        }


        private string title;
        public string Title {
            get { return "Resident"; }
            set { title = value; }
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

        private string selectedInputColumn;
        public string SelectedInputColumn {
            get { return selectedInputColumn; }
            set {
                selectedInputColumn = value;
                OnPropertyChanged("SelectedInputColumn");
            }
        }

        private string selectedOutputColumn;
        public string SelectedOutputColumn {
            get { return selectedOutputColumn; }
            set {
                selectedOutputColumn = value;
                OnPropertyChanged("SelectedOutputColumn");
            }
        }

        private string area;
        public string Area {
            get { return area; }
            set {
                area = value;
                OnPropertyChanged("Area");
                ClearArea.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<string> residenceColumns;
        public ObservableCollection<string> ResidenceColumns {
            get { return residenceColumns; }
            set { residenceColumns = value; }
        }

        private ObservableCollection<string> selectedResidenceColumns;
        public ObservableCollection<string> SelectedResidenceColumns {
            get { return selectedResidenceColumns; }
            set { selectedResidenceColumns = value; }
        }

        private DelegateCommand showReportCommand;
        public DelegateCommand ShowReportCommand {
            get { return showReportCommand; }
            set { showReportCommand = value; }
        }
        private void ExecuteShowReport() {
            if(SelectedResidenceColumns.Count == 0) {
                MessageBox.Show("No Columns selected");
                Result = null;
                SearchStatus = String.Empty;
                return;
            }
            Result = new FlowDocument();
            List<Residence> input = null;
            using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                input = unitOfWork.Residences.GetAll().ToList();
                String[] temp = null;
                if(!String.IsNullOrEmpty(Area)) {
                    if(Area.Contains(";")) {
                        temp = Area.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = Area;
                    }
                    if(temp != null) {
                        input = input.Where(x => temp.Contains(x.Area, new ContainsComparer())).ToList();
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
                        input = input.Where(x => temp.Contains(x.Name, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(HouseNumber)) {
                    temp = null;
                    if(HouseNumber.Contains(";")) {
                        temp = HouseNumber.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = HouseNumber;
                    }
                    if(temp != null) {
                        input = input.Where(x => temp.Contains(x.Number, new ContainsComparer())).ToList();
                    }
                }
            }

            List<ResidenceMember> members = null;
            using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                members = unitOfWork.ResidenceMembers.GetAll().ToList();
                String[] temp = null;
                if(!String.IsNullOrEmpty(MarriageStatus)) {
                    if(MarriageStatus.Contains(";")) {
                        temp = MarriageStatus.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = MarriageStatus;
                    }
                    if(temp != null) {
                        members = members.Where(x => temp.Contains(x.MarriageStatus, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(Country)) {
                    temp = null;
                    if(Country.Contains(";")) {
                        temp = Country.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = Country;
                    }
                    if(temp != null) {
                        members = members.Where(x => temp.Contains(x.Country, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(Qualification)) {
                    temp = null;
                    if(Qualification.Contains(";")) {
                        temp = Qualification.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = Qualification;
                    }
                    if(temp != null) {
                        members = members.Where(x => temp.Contains(x.Qualification, new ContainsComparer())).ToList();
                    }
                }
                if(!String.IsNullOrEmpty(Gender)) {
                    temp = null;
                    if(Gender.Contains(";")) {
                        temp = Gender.Split(';');
                    } else {
                        temp = new string[1];
                        temp[0] = Gender;
                    }
                    if(temp != null) {
                        members = members.Where(x => temp.Contains(x.Gender, new ContainsComparer())).ToList();
                    }
                }
            }

            if(ValidateReportParameters()) {
                BuildReport(input, members);
            }
        }

        private DelegateCommand printReportCommand;
        public DelegateCommand PrintReportCommand {
            get { return printReportCommand; }
            set { printReportCommand = value; }
        }
        private void ExecutePrintReport() {
            MessageBox.Show("Press Ctrl + P to print");
            //PrintDialog printDlg = new PrintDialog();
            //printDlg.ShowDialog();
            //Result.PageWidth = 740;// printDlg.PrintableAreaWidth;
            ////Result.MinPageWidth = printDlg.PrintableAreaWidth;

            //IDocumentPaginatorSource idpSource = Result;
            //idpSource.DocumentPaginator.PageSize = new Size(printDlg.PrintableAreaWidth,printDlg.PrintableAreaHeight);
            //printDlg.PrintDocument(idpSource.DocumentPaginator, "Printing.");
        }

        private void BuildReport(List<Residence> residences, List<ResidenceMember> members) {
            var x = residences.Join(members, residence => residence.Id, member => member.Residence_Id, (residence, member) =>
            new {
                residence.Name, residence.Area,residence.Number,
                MemberName = member.MemberName,
                DOB = member.DOB.ToShortDateString(), member.MarriageStatus, member.Gender,
                Mobile = member.Mobile,
                Job = member.Job,
                IsGuardian = member.IsGuardian,
                Qualification = member.Qualification,
                Country = member.Country
            });
            if(x != null && x.Count() > 0) {
                SearchStatus = x.Count() + " Found";
            } else {
                SearchStatus = "No Result Found";
            }
            for(int i = 0; i < 200; i++) {

            }
            for(int i = 0; i < x.Count(); i += 38) {
                var list = (from c in x
                            select c).Skip(i).Take(38);

                ListView l = new ListView();
                l.ItemsSource = list;
                GridView g = new GridView();
                l.Width = 700; //690
                l.MaxWidth = 700;
                if(SelectedResidenceColumns.Contains("House Number")) {
                    GridViewColumn residenceNumber = new GridViewColumn() {
                        Header = "House#", DisplayMemberBinding = new Binding("Number"), Width = 60
                    };
                    g.Columns.Add(residenceNumber);
                }

                if(SelectedResidenceColumns.Contains("House Name")) {
                    GridViewColumn residenceName = new GridViewColumn() {
                        Header = "House Name", DisplayMemberBinding = new Binding("Name"), Width = 180
                    };
                    g.Columns.Add(residenceName);
                }

                if(SelectedResidenceColumns.Contains("Area")) {
                    var area = new GridViewColumn() {
                        Header = "Area",
                        DisplayMemberBinding = new Binding("Area"),
                        Width = 180
                    };
                    g.Columns.Add(area);
                }

                if(SelectedResidenceColumns.Contains("Name")) {
                    GridViewColumn memberName = new GridViewColumn() {
                        Header = "Name", DisplayMemberBinding = new Binding("MemberName"), Width = 160
                    };
                    g.Columns.Add(memberName);
                }
                //4*180
                if(SelectedResidenceColumns.Contains("Date of Birth")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Date of Birth", DisplayMemberBinding = new Binding("DOB"), Width = 100
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Gender")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Gender", DisplayMemberBinding = new Binding("Gender"), Width = 64
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Marriage Status")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Marriage Status", DisplayMemberBinding = new Binding("MarriageStatus"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Job")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Job", DisplayMemberBinding = new Binding("Job"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Mobile")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Mobile", DisplayMemberBinding = new Binding("Mobile"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Country")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Country", DisplayMemberBinding = new Binding("Country"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
                }
                if(SelectedResidenceColumns.Contains("Guardian")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Guardian", DisplayMemberBinding = new Binding("IsGuardian"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
                }
                if(SelectedResidenceColumns.Contains("Qualification")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Qualification", DisplayMemberBinding = new Binding("Qualification"), Width = 166
                    };
                    g.Columns.Add(dateOfBirth);
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
