using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
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
            ResidenceColumns = new ObservableCollection<string>() { "Name", "Area", "DOB1", "MarriageStatus", "Gender" };
            SelectedResidenceColumns = new ObservableCollection<string>();
        }

        private string headerText;
        public string HeaderText {
            get { return headerText; }
            set {
                headerText = value;
                OnPropertyChanged("HeaderText");
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
                result.PageWidth = 740;
                result.TextAlignment = System.Windows.TextAlignment.Center;
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
            Result = new FlowDocument();
            List<Residence> input = null;
            using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                input = unitOfWork.Residences.GetAll().ToList();
            }

            List<ResidenceMember> members = null;
            using(UnitOfWork unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                members = unitOfWork.ResidenceMembers.GetAll().ToList();
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
            PrintDialog printDlg = new PrintDialog();
            printDlg.ShowDialog();
            Result.PageWidth = printDlg.PrintableAreaWidth;
            Result.MinPageWidth = printDlg.PrintableAreaWidth;

            IDocumentPaginatorSource idpSource = Result;

            printDlg.PrintDocument(idpSource.DocumentPaginator, "Printing.");
        }

        private void BuildReport(List<Residence> residences, List<ResidenceMember> members) {
            var x = residences.Join(members, residence => residence.Id, memer => memer.Residence_Id, (residence, memer) =>
            new { residence.Name, residence.Area, DOB1 = memer.DOB.ToShortDateString(), memer.MarriageStatus, memer.Gender });
            
            for(int i = 0; i < x.Count(); i += 25) {
                var list = (from c in x
                            select c).Skip(i).Take(25);

                ListView l = new ListView();
                l.ItemsSource = list;
                GridView g = new GridView();
                l.Width = 700;
                if(SelectedResidenceColumns.Contains("Name")) {
                    GridViewColumn residenceName = new GridViewColumn() {
                        Header = "Name", DisplayMemberBinding = new Binding("Name"), Width = 160
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
                //4*180
                if(SelectedResidenceColumns.Contains("DOB1")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "DOB1", DisplayMemberBinding = new Binding("DOB1"), Width = 96
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("Gender")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "Gender", DisplayMemberBinding = new Binding("Gender"), Width = 64
                    };
                    g.Columns.Add(dateOfBirth);
                }

                if(SelectedResidenceColumns.Contains("MarriageStatus")) {
                    var dateOfBirth = new GridViewColumn() {
                        Header = "MarriageStatus", DisplayMemberBinding = new Binding("MarriageStatus"), Width = 170
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
