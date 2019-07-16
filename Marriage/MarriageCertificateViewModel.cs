using iTextSharp.text;
using iTextSharp.text.pdf;
using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Marriage {
    public class MarriageCertificateViewModel : ViewModelBase {
        public MarriageCertificateViewModel() {
            InitializeDatePicker();
            GenerateCertificateCommand = new DelegateCommand(ExecuteGenerateCertificate, CanExecuteGenerateCertificate);
            SaveMarriageCommand = new DelegateCommand(ExecuteSaveMarriage);
            NewMarriageCommand = new DelegateCommand(ExecuteNewMarriage);
            DeleteMarriageCommand = new DelegateCommand(ExecuteDeleteMarriage, CanExecuteDeleteMarriage);
            ClearMarriageCommand = new DelegateCommand(ExecuteClearMarriage, CanExecuteClearMarriage);
            ClearSearchCommand = new DelegateCommand(ExecuteClearSearch);
            SearchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch);
            RefreshMarriages();
        }

        private MarriageCertificate currentMarriage;
        public MarriageCertificate CurrentMarriage {
            get { return currentMarriage; }
            set {
                currentMarriage = value;
                CurrentMarriageChanged();
                ClearMarriageCommand.RaiseCanExecuteChanged();
                DeleteMarriageCommand.RaiseCanExecuteChanged();
                SaveMarriageCommand.RaiseCanExecuteChanged();
                NewMarriageCommand.RaiseCanExecuteChanged();
                GenerateCertificateCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CurrentMarriage");
            }
        }

        private ObservableCollection<MarriageCertificate> marriageList;
        public ObservableCollection<MarriageCertificate> MarriageList {
            get { return marriageList; }
            set {
                marriageList = value;
                OnPropertyChanged("MarriageList");
            }
        }
        private List<MarriageCertificate> searchSource = null;

        private string regNo;
        public string RegNo {
            get { return regNo; }
            set {
                regNo = value;
                OnPropertyChanged("RegNo");
            }
        }

        private string masjidName;
        public string MasjidName {
            get { return masjidName + "Muhiyidheen Masjid, Iringal Moorad"; }
            set {
                masjidName = value;
                OnPropertyChanged("MasjidName");
            }
        }

        private String place;
        public String Place {
            get { return place + "Iringal, Moorad"; }
            set {
                place = value;
                OnPropertyChanged("Place");
            }
        }

        private DateTime dobEndDate;
        public DateTime DOBEndDate {
            get { return dobEndDate; }
            set {
                dobEndDate = value;
                OnPropertyChanged("DOBEndDate");
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

        private DateTime marriageDate;
        public DateTime MarriageDate {
            get { return marriageDate; }
            set {
                marriageDate = value;
                OnPropertyChanged("MarriageDate");
            }
        }

        private string marriagePlace;
        public string MarriagePlace {
            get { return marriagePlace; }
            set {
                marriagePlace = value;
                OnPropertyChanged("MarriagePlace");
            }
        }

        private DateTime groomDOB;
        public DateTime GroomDOB {
            get { return groomDOB; }
            set {
                groomDOB = value;
                OnPropertyChanged("GroomDOB");
            }
        }

        private DateTime brideDOB;
        public DateTime BrideDOB {
            get { return brideDOB; }
            set {
                brideDOB = value;
                OnPropertyChanged("BrideDOB");
            }
        }


        private BitmapImage bridePhoto;
        public BitmapImage BridePhoto {
            get {
                return bridePhoto;
            }
            set {
                bridePhoto = value;
                OnPropertyChanged("BridePhoto");
            }
        }

        private BitmapImage groomPhoto;
        public BitmapImage GroomPhoto {
            get {
                return groomPhoto;
            }
            set {
                groomPhoto = value;
                OnPropertyChanged("GroomPhoto");
            }
        }

        private BitmapImage bridePhoto1;
        public BitmapImage BridePhoto1 {
            get {
                return bridePhoto1;
            }
            set {
                bridePhoto1 = value;
                OnPropertyChanged("BridePhoto1");
            }
        }

        private BitmapImage groomPhoto1;
        public BitmapImage GroomPhoto1 {
            get {
                return groomPhoto1;
            }
            set {
                groomPhoto1 = value;
                OnPropertyChanged("GroomPhoto1");
            }
        }

        private string bridePhotoPath;
        public string BridePhotoPath {
            get { return bridePhotoPath; }
            set {
                bridePhotoPath = value;
                if(!String.IsNullOrEmpty(bridePhotoPath)) {
                    var uriSource = new Uri(bridePhotoPath, UriKind.Absolute);
                    BridePhoto = new BitmapImage(uriSource);
                    BridePhoto1 = new BitmapImage(new Uri(bridePhotoPath, UriKind.Relative));
                }
                OnPropertyChanged("BridePhotoPath");
            }
        }

        private string groomPhotoPath;
        public string GroomPhotoPath {
            get { return groomPhotoPath; }
            set {
                groomPhotoPath = value;
                if(!String.IsNullOrEmpty(groomPhotoPath)) {
                    var uriSource = new Uri(groomPhotoPath, UriKind.Absolute);
                    GroomPhoto = new BitmapImage(uriSource);
                    GroomPhoto1 = new BitmapImage(new Uri(groomPhotoPath, UriKind.Relative));
                }
                OnPropertyChanged("GroomPhotoPath");
            }
        }

        private String certificatePath;
        public String CertificatePath {
            get { return certificatePath; }
            set {
                certificatePath = value;
                OnPropertyChanged("CertificatePath");
            }
        }

        private string groomName;
        public string GroomName {
            get { return groomName; }
            set {
                groomName = value;
                OnPropertyChanged("GroomName");
            }
        }
        private string groomFatherName;
        public string GroomFatherName {
            get { return groomFatherName; }
            set {
                groomFatherName = value;
                OnPropertyChanged("GroomFatherName");
            }
        }
        private string groomHouseName;
        public string GroomHouseName {
            get { return groomHouseName; }
            set {
                groomHouseName = value;
                OnPropertyChanged("GroomHouseName");
            }
        }

        private string groomArea;
        public string GroomArea {
            get { return groomArea; }
            set {
                groomArea = value;
                OnPropertyChanged("GroomArea");
            }
        }

        private String groomPincode;
        public String GroomPincode {
            get { return groomPincode; }
            set {
                groomPincode = value;
                OnPropertyChanged("GroomPincode");
            }
        }
        private String groomPostOffice;
        public String GroomPostOffice {
            get { return groomPostOffice; }
            set {
                groomPostOffice = value;
                OnPropertyChanged("GroomPostOffice");
            }
        }

        private string groomDistrict;
        public string GroomDistrict {
            get { return groomDistrict; }
            set {
                groomDistrict = value;
                OnPropertyChanged("GroomDistrict");
            }
        }

        private string groomState;
        public string GroomState {
            get { return groomState; }
            set {
                groomState = value;
                OnPropertyChanged("GroomState");
            }
        }

        private string groomCountry;
        public string GroomCountry {
            get { return groomCountry; }
            set {
                groomCountry = value;
                OnPropertyChanged("GroomCountry");
            }
        }

        private string brideName;
        public string BrideName {
            get { return brideName; }
            set {
                brideName = value;
                OnPropertyChanged("BrideName");
            }
        }

        private string brideFatherName;
        public string BrideFatherName {
            get { return brideFatherName; }
            set {
                brideFatherName = value;
                OnPropertyChanged("BrideFatherName");
            }
        }

        private string brideHouseName;
        public string BrideHouseName {
            get { return brideHouseName; }
            set {
                brideHouseName = value;
                OnPropertyChanged("BrideHouseName");
            }
        }

        private string brideArea;
        public string BrideArea {
            get { return brideArea; }
            set {
                brideArea = value;
                OnPropertyChanged("BrideArea");
            }
        }
        private String bridePincode;
        public String BridePincode {
            get { return bridePincode; }
            set {
                bridePincode = value;
                OnPropertyChanged("BridePincode");
            }
        }

        private string bridePostOffice;
        public string BridePostOffice {
            get { return bridePostOffice; }
            set {
                bridePostOffice = value;
                OnPropertyChanged("BridePostOffice");
            }
        }

        private string brideDistrict;
        public string BrideDistrict {
            get { return brideDistrict; }
            set {
                brideDistrict = value;
                OnPropertyChanged("BrideDistrict");
            }
        }

        private string brideState;
        public string BrideState {
            get { return brideState; }
            set {
                brideState = value;
                OnPropertyChanged("BrideState");
            }
        }

        private String brideCountry;
        public String BrideCountry {
            get { return brideCountry; }
            set {
                brideCountry = value;
                OnPropertyChanged("BrideCountry");
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

        private DelegateCommand saveMarriageCommand;
        public DelegateCommand SaveMarriageCommand {
            get { return saveMarriageCommand; }
            set { saveMarriageCommand = value; }
        }
        private void ExecuteSaveMarriage() {
            using(var unitOfWork = new UnitOfWork(new MahalluDBContext())) {
                if(ValidateFields()) {
                    MarriageCertificate marriageCertificate = GetMarriageDetails();
                    if(CurrentMarriage != null) {
                        CurrentMarriage.GroomName = marriageCertificate.GroomName;
                        CurrentMarriage.GroomDOB = marriageCertificate.GroomDOB;
                        CurrentMarriage.GroomFatherName = marriageCertificate.GroomFatherName;
                        CurrentMarriage.GroomHouseName = marriageCertificate.GroomHouseName;
                        CurrentMarriage.GroomArea = marriageCertificate.GroomArea;
                        CurrentMarriage.GroomPincode = marriageCertificate.GroomPincode;
                        CurrentMarriage.GroomPostOffice = marriageCertificate.GroomPostOffice;
                        CurrentMarriage.GroomDistrict = marriageCertificate.GroomDistrict;
                        CurrentMarriage.GroomState = marriageCertificate.GroomState;
                        CurrentMarriage.GroomCountry = marriageCertificate.GroomCountry;
                        if(marriageCertificate.GroomPhoto != null) {
                            CurrentMarriage.GroomPhoto = marriageCertificate.GroomPhoto;
                        }

                        CurrentMarriage.BrideName = marriageCertificate.BrideName;
                        CurrentMarriage.BrideDOB = marriageCertificate.BrideDOB;
                        CurrentMarriage.BrideFatherName = marriageCertificate.BrideFatherName;
                        CurrentMarriage.BrideHouseName = marriageCertificate.BrideHouseName;
                        CurrentMarriage.BrideArea = marriageCertificate.BrideArea;
                        CurrentMarriage.BridePincode = marriageCertificate.BridePincode;
                        CurrentMarriage.BridePostOffice = marriageCertificate.BridePostOffice;
                        CurrentMarriage.BrideDistrict = marriageCertificate.BrideDistrict;
                        CurrentMarriage.BrideState = marriageCertificate.BrideState;
                        CurrentMarriage.BrideCountry = marriageCertificate.BrideCountry;
                        if(marriageCertificate.BridePhoto != null) {
                            CurrentMarriage.BridePhoto = marriageCertificate.BridePhoto;
                        }
                        CurrentMarriage.MarriageDate = marriageCertificate.MarriageDate;
                        CurrentMarriage.MarriagePlace = marriageCertificate.MarriagePlace;

                        unitOfWork.MarriageCertificates.Update(CurrentMarriage);
                        MessageBox.Show(GroomName + "'s marriage updated successfully !", "New Marriage", MessageBoxButton.OK, MessageBoxImage.Information);
                    } else {
                        unitOfWork.MarriageCertificates.Add(marriageCertificate);
                        MarriageList.Add(marriageCertificate);
                        CurrentMarriage = marriageCertificate;
                        MessageBox.Show("Marriage details added successfully !", "New Marriage", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    unitOfWork.Complete();
                    BridePhotoPath = GroomPhotoPath = string.Empty;
                    //ClearMarriage();
                    //ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                    //eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);
                }
            }
        }

        private DelegateCommand generateCertificateCommand;
        public DelegateCommand GenerateCertificateCommand {
            get { return generateCertificateCommand; }
            set { generateCertificateCommand = value; }
        }
        private void ExecuteGenerateCertificate1() {
            CertificatePath = String.Empty;
            Thread.Sleep(400);
            string certificateName = "Certificate.pdf";
            Document document = new Document();
            FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            PdfContentByte pdfContentByte = writer.DirectContent;
            int textDown = 0;
            if(String.IsNullOrEmpty(GroomPhotoPath) && String.IsNullOrEmpty(GroomPhotoPath)) {
                textDown = 70;
            }

            Paragraph title = new Paragraph();
            title.Alignment = Element.ALIGN_CENTER;
            title.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, Font.UNDERLINE, BaseColor.BLACK);
            title.Add("MARRIAGE CERTIFICATE");
            ColumnText ct = new ColumnText(pdfContentByte);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 520 - textDown, 560, 30));
            ct.AddElement(title);
            ct.Go();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            int column1LeftMargin = 120;
            int column2LeftMargin = 370;
            if(!String.IsNullOrEmpty(GroomPhotoPath) && !String.IsNullOrEmpty(GroomPhotoPath)) {
                Image i = Image.GetInstance(GroomPhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(94f, 144f);
                i.SetAbsolutePosition(200, 360);
                pdfContentByte.AddImage(i);
                i = Image.GetInstance(BridePhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(94f, 144f);
                i.SetAbsolutePosition(310, 360);
                pdfContentByte.AddImage(i);
            }

            GroomName = "Rashid MV";
            GroomFatherName = "Khader KM";
            GroomHouseName = "Rashid Manzil";
            GroomArea = "Kottakunnummal";
            GroomPincode = "673521";
            GroomPostOffice = "Iringal";
            GroomDistrict = "Kozhikkode";
            GroomState = "Kerala";
            GroomCountry = "India";

            BrideName = "Hiba Fahisa AV";
            BrideFatherName = "Asharaf AV";
            BrideHouseName = "Kulangarath Kuniyil";
            BrideArea = "Arakkilad";
            BridePincode = "675302";
            BridePostOffice = "Nadakkuthazhe";
            BrideDistrict = "Kozhikkode";
            BrideState = "Kerala";
            BrideCountry = "India";
            MarriagePlace = "Moorad";

            Paragraph groomName = new Paragraph(GroomName);
            groomName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(column1LeftMargin, 356, 540, 30));
            ct.AddElement(groomName);
            ct.Go();
            Paragraph brideName = new Paragraph(BrideName);
            brideName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(column2LeftMargin, 356, 540, 30));
            ct.AddElement(brideName);
            ct.Go();

            pdfContentByte.SetColorFill(BaseColor.DARK_GRAY);
            pdfContentByte.SetFontAndSize(bf, 12);

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "S/o. " + GroomFatherName, column1LeftMargin, 324, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "D/o. " + BrideFatherName, column2LeftMargin, 324, 0);
            pdfContentByte.EndText();


            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "Date of Birth: " + GroomDOB.ToShortDateString(), column1LeftMargin, 308, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "Date of Birth: " + BrideDOB.ToShortDateString(), column2LeftMargin, 308, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomHouseName + " [House],", column1LeftMargin, 292, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideHouseName + " [House],", column2LeftMargin, 292, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomArea + ",", column1LeftMargin, 276, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideArea + ",", column2LeftMargin, 276, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomPostOffice + " [Post],", column1LeftMargin, 260, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BridePostOffice + " [Post],", column2LeftMargin, 260, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomDistrict + " - " + GroomPincode + ",", column1LeftMargin, 244, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideDistrict + " - " + BridePincode + ",", column2LeftMargin, 244, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomState + ", " + GroomCountry, column1LeftMargin, 228, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideState + ", " + BrideCountry, column2LeftMargin, 228, 0);
            pdfContentByte.EndText();

            Chunk dateOfMarriage = new Chunk("'" + MarriageDate.ToShortDateString() + "'");
            dateOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            Chunk placeOfMarriage = new Chunk("'" + MarriagePlace + "'");
            placeOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            Chunk masjidName = new Chunk("'" + MasjidName + "'");
            masjidName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 12f, BaseColor.BLACK);

            Paragraph content = new Paragraph();
            content.SpacingBefore = 10;
            content.SpacingAfter = 10;
            content.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.BLACK);
            string contentText1 = "             Certify that the Marriage between the above mentioned persons was solemnised by Mahallu Khazi of ";
            string contentText2 = " in the presence of their friends and relatives on ";
            string contentText3 = " at ";
            string contentText4 = " according to the Islamic Shareeath Law.";
            content.Add(contentText1);
            content.Add(masjidName);
            content.Add(contentText2);
            content.Add(dateOfMarriage);
            content.Add(contentText3);
            content.Add(placeOfMarriage);
            content.Add(contentText4);
            content.Alignment = Element.ALIGN_JUSTIFIED;
            ct.SetSimpleColumn(new Rectangle(30, 200, 560, 50));
            ct.AddElement(content);
            ct.Go();

            Paragraph secretary = new Paragraph("SECRETARY");
            secretary.Alignment = Element.ALIGN_RIGHT;
            secretary.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 120, 540, 30));
            ct.AddElement(secretary);
            ct.Go();

            Paragraph place = new Paragraph("Place : " + Place + "\nDate  : " + DateTime.Now.ToShortDateString());
            place.Alignment = Element.ALIGN_LEFT;
            place.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(40, 120, 540, 30));
            ct.AddElement(place);
            ct.Go();

            document.Close();
            fs.Close();
            writer.Close();

            String currentFolder = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, "Certificate.pdf");
        }
        private void ExecuteGenerateCertificate() {
            if(!ValidateFields()) {
                MessageBox.Show("Please Enter all the required fields, then generate certificate");
                return;
            }
            CertificatePath = String.Empty;
            Thread.Sleep(400);
            string certificateName = "Certificate.pdf";
            Document document = new Document();
            FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            PdfContentByte pdfContentByte = writer.DirectContent;
            int textDown = 0;
            if(String.IsNullOrEmpty(GroomPhotoPath) && String.IsNullOrEmpty(GroomPhotoPath)) {
                textDown = 100;
            }

            Paragraph title = new Paragraph();
            title.Alignment = Element.ALIGN_CENTER;
            title.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 18f, Font.UNDERLINE, BaseColor.BLACK);
            title.Add("MARRIAGE CERTIFICATE");
            ColumnText ct = new ColumnText(pdfContentByte);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 520 - textDown, 560, 30));
            ct.AddElement(title);
            ct.Go();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            int column1LeftMargin = 120;
            int column2LeftMargin = 260;
            if(!String.IsNullOrEmpty(GroomPhotoPath) && !String.IsNullOrEmpty(GroomPhotoPath)) {
                Image i = Image.GetInstance(GroomPhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(94f, 144f);
                i.SetAbsolutePosition(200, 370);
                pdfContentByte.AddImage(i);
                i = Image.GetInstance(BridePhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(94f, 144f);
                i.SetAbsolutePosition(310, 370);
                pdfContentByte.AddImage(i);
            }

            //GroomName = "Rashid MV";
            //GroomFatherName = "Khader KM";
            //GroomHouseName = "Rashid Manzil";
            //GroomArea = "Kottakunnummal";
            //GroomPincode = "673521";
            //GroomPostOffice = "Iringal";
            //GroomDistrict = "Kozhikkode";
            //GroomState = "Kerala";
            //GroomCountry = "India";

            //BrideName = "Hiba Fahisa AV";
            //BrideFatherName = "Asharaf AV";
            //BrideHouseName = "Kulangarath Kuniyil";
            //BrideArea = "Arakkilad";
            //BridePincode = "675302";
            //BridePostOffice = "Nadakkuthazhe";
            //BrideDistrict = "Kozhikkode";
            //BrideState = "Kerala";
            //BrideCountry = "India";
            //MarriagePlace = "Moorad";


            Paragraph registation = new Paragraph("Reg.No : " + RegNo);
            registation.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 11f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(column1LeftMargin - 30, 386, 416, 30));
            ct.AddElement(registation);
            ct.Go();

            Paragraph groomDetailsLabels = new Paragraph("Name of Bridegroom   :\n" +
                                                         "Date of Birth                :\n" +
                                                         "Name of father            :\n" +
                                                         "Permenant Address    :");
            groomDetailsLabels.Leading = 12;
            groomDetailsLabels.Font = FontFactory.GetFont(FontFactory.HELVETICA, 11f, BaseColor.BLACK);
            ct.SetSimpleColumn(new Rectangle(column1LeftMargin, 366, 416, 30));
            ct.AddElement(groomDetailsLabels);
            ct.Go();
            Chunk groomName = new Chunk(GroomName);
            groomName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, BaseColor.BLACK);
            Paragraph groomDetails = new Paragraph();
            groomDetails.Font = FontFactory.GetFont(FontFactory.HELVETICA, 11f, BaseColor.BLACK);
            groomDetails.Add(groomName);

            String details = "\n" + GroomDOB.ToShortDateString() + "\n" + GroomFatherName + "\n"
                + GroomHouseName + " [House],\n" + GroomArea + ", " + GroomPostOffice + ",\n"
                + GroomDistrict + ", " + GroomState + ",\n" + GroomCountry + " - " + GroomPincode;
            groomDetails.Add(details);
            groomDetails.Leading = 12;
            ct.SetSimpleColumn(new Rectangle(column2LeftMargin, 366, 416, 30));
            ct.AddElement(groomDetails);
            ct.Go();

            Paragraph brideDetailsLabels = new Paragraph("Name of Bride             :\n" +
                                                         "Date of Birth                :\n" +
                                                         "Name of father            :\n" +
                                                         "Permenant Address    :");
            brideDetailsLabels.Leading = 12;
            brideDetailsLabels.Font = FontFactory.GetFont(FontFactory.HELVETICA, 11f, BaseColor.BLACK);
            ct.SetSimpleColumn(new Rectangle(column1LeftMargin, 276, 416, 30));
            ct.AddElement(brideDetailsLabels);
            ct.Go();
            Chunk brideName = new Chunk(BrideName);
            brideName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, BaseColor.BLACK);
            Paragraph brideDetails = new Paragraph();
            brideDetails.Font = FontFactory.GetFont(FontFactory.HELVETICA, 11f, BaseColor.BLACK);
            brideDetails.Add(brideName);

            details = "\n" + BrideDOB.ToShortDateString() + "\n" + BrideFatherName + "\n"
                + BrideHouseName + " [House],\n" + BrideArea + ", " + BridePostOffice + ",\n"
                + BrideDistrict + ", " + BrideState + ",\n" + BrideCountry + " - " + BridePincode;
            brideDetails.Add(details);
            brideDetails.Leading = 12;
            ct.SetSimpleColumn(new Rectangle(column2LeftMargin, 276, 416, 30));
            ct.AddElement(brideDetails);
            ct.Go();

            //pdfContentByte.SetColorFill(BaseColor.BLACK);
            //pdfContentByte.SetFontAndSize(bf, 12);

            Chunk dateOfMarriage = new Chunk("'" + MarriageDate.ToShortDateString() + "'");
            dateOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            Chunk placeOfMarriage = new Chunk("'" + MarriagePlace + "'");
            placeOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            Chunk masjidName = new Chunk("'" + MasjidName + "'");
            masjidName.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLDOBLIQUE, 12f, BaseColor.BLACK);

            Paragraph content = new Paragraph();
            content.SpacingBefore = 10;
            content.SpacingAfter = 10;
            content.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.BLACK);
            string contentText1 = "             Certify that the Marriage between the above mentioned persons was solemnised by Mahallu Khazi of ";
            string contentText2 = " in the presence of their friends and relatives on ";
            string contentText3 = " at ";
            string contentText4 = " according to the Islamic Shareeath Law.";
            content.Add(contentText1);
            content.Add(masjidName);
            content.Add(contentText2);
            content.Add(dateOfMarriage);
            content.Add(contentText3);
            content.Add(placeOfMarriage);
            content.Add(contentText4);
            content.Alignment = Element.ALIGN_JUSTIFIED;
            ct.SetSimpleColumn(new Rectangle(30, 180, 540, 50));
            ct.AddElement(content);
            ct.Go();

            Paragraph secretary = new Paragraph("SECRETARY");
            secretary.Alignment = Element.ALIGN_RIGHT;
            secretary.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 120, 540, 30));
            ct.AddElement(secretary);
            ct.Go();

            Paragraph place = new Paragraph("Place : " + Place + "\nDate  : " + DateTime.Now.ToShortDateString());
            place.Alignment = Element.ALIGN_LEFT;
            place.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(40, 120, 540, 30));
            ct.AddElement(place);
            ct.Go();

            document.Close();
            fs.Close();
            writer.Close();

            String currentFolder = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, "Certificate.pdf");
        }
        private bool CanExecuteGenerateCertificate() {
            return CurrentMarriage != null;
        }

        private DelegateCommand newMarriageCommand;

        public DelegateCommand NewMarriageCommand {
            get { return newMarriageCommand; }
            set { newMarriageCommand = value; }
        }
        private void ExecuteNewMarriage() {
            CurrentMarriage = null;
        }

        private DelegateCommand deleteMarriageCommand;
        public DelegateCommand DeleteMarriageCommand {
            get { return deleteMarriageCommand; }
            set { deleteMarriageCommand = value; }
        }

        private bool CanExecuteDeleteMarriage() {
            return CurrentMarriage != null;
        }

        private void ExecuteDeleteMarriage() {
            MessageBoxResult result = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes) {
                if(CurrentMarriage != null) {
                    using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                        MarriageCertificate marriageCertificate = unitofWork.MarriageCertificates.Get(CurrentMarriage.Id);
                        unitofWork.MarriageCertificates.Remove(marriageCertificate);
                        unitofWork.Complete();

                        //ExpenseType totatExpenseType = new ExpenseType() { Expense = CurrentExpense };
                        //eventAggregator.GetEvent<PubSubEvent<ExpenseType>>().Publish(totatExpenseType);

                        MarriageList.Remove(CurrentMarriage);
                        CurrentMarriage = null;
                    }
                }
            }
        }

        private DelegateCommand clearMarriageCommand;
        public DelegateCommand ClearMarriageCommand {
            get { return clearMarriageCommand; }
            set { clearMarriageCommand = value; }
        }

        private bool CanExecuteClearMarriage() {
            return CurrentMarriage != null;
        }

        private void ExecuteClearMarriage() {
            CurrentMarriage = null;
        }

        private DelegateCommand clearSearchCommand;
        public DelegateCommand ClearSearchCommand {
            get { return clearSearchCommand; }
            set { clearSearchCommand = value; }
        }
        private void ExecuteClearSearch() {
            if(MarriageList != null) {
                MarriageList.Clear();
                SearchText = String.Empty;
            }
            RefreshMarriages();
        }

        private DelegateCommand searchCommand;
        public DelegateCommand SearchCommand {
            get { return searchCommand; }
            set { searchCommand = value; }
        }
        private void ExecuteSearch() {
            RefreshMarriages();
            searchSource = MarriageList.ToList(); ;
            MarriageList = new ObservableCollection<MarriageCertificate>(searchSource.FindAll((x) => x.BrideName.Contains(SearchText.Trim())
                                                                            || x.BrideFatherName.Contains(SearchText.Trim())
                                                                            || x.GroomFatherName.Contains(SearchText.Trim())
                                                                            || x.Id.ToString() == (SearchText.Trim())
                                                                            || x.GroomName.Contains(SearchText.Trim())));
            if(MarriageList != null && MarriageList.Count == 0) {

                MessageBox.Show("No Marriage details found with " + SearchText);
            }
        }
        private bool CanExecuteSearch() {
            return MarriageList != null && !String.IsNullOrEmpty(SearchText);
        }


        private void InitializeDatePicker() {
            DOBEndDate = DateTime.Now.AddYears(-18);
            MarriageDate = EndDate = DateTime.Now;
            GroomDOB = BrideDOB = DOBEndDate;
        }

        private bool ValidateFields() {
            if(string.IsNullOrEmpty(GroomName)) {
                MessageBox.Show("Please Enter Groom Name");
                return false;
            } else if(string.IsNullOrEmpty(BrideName)) {
                MessageBox.Show("Please Enter Bride Name");
                return false;
            } else if(GroomDOB == null) {
                MessageBox.Show("Please Enter Groom Date of birth");
                return false;
            } else if(BrideDOB == null) {
                MessageBox.Show("Please Enter Bride Date of birth");
                return false;
            } else if(string.IsNullOrEmpty(GroomFatherName)) {
                MessageBox.Show("Please Enter Groom Father Name");
                return false;
            } else if(string.IsNullOrEmpty(BrideFatherName)) {
                MessageBox.Show("Please Enter Bride Father Name");
                return false;
            } else if(string.IsNullOrEmpty(GroomHouseName)) {
                MessageBox.Show("Please Enter Groom House Name");
                return false;
            } else if(string.IsNullOrEmpty(BrideHouseName)) {
                MessageBox.Show("Please Enter Bride House Name");
                return false;
            } else if(string.IsNullOrEmpty(GroomArea)) {
                MessageBox.Show("Please Enter Groom Area");
                return false;
            } else if(string.IsNullOrEmpty(BrideArea)) {
                MessageBox.Show("Please Enter Bride Area");
                return false;
            } else if(string.IsNullOrEmpty(GroomPincode)) {
                MessageBox.Show("Please Enter Groom Pincode");
                return false;
            } else if(string.IsNullOrEmpty(BridePincode)) {
                MessageBox.Show("Please Enter Bride Pincode");
                return false;
            } else if(string.IsNullOrEmpty(GroomPostOffice)) {
                MessageBox.Show("Please Enter Groom Post Office");
                return false;
            } else if(string.IsNullOrEmpty(BridePostOffice)) {
                MessageBox.Show("Please Enter Bride Post Office");
                return false;
            } else if(string.IsNullOrEmpty(GroomDistrict)) {
                MessageBox.Show("Please Enter Groom District");
                return false;
            } else if(string.IsNullOrEmpty(BrideDistrict)) {
                MessageBox.Show("Please Enter Bride District");
                return false;
            } else if(string.IsNullOrEmpty(GroomState)) {
                MessageBox.Show("Please Enter Groom State");
                return false;
            } else if(string.IsNullOrEmpty(BrideState)) {
                MessageBox.Show("Please Enter Bride State");
                return false;
            } else if(string.IsNullOrEmpty(GroomCountry)) {
                MessageBox.Show("Please Enter Groom Country");
                return false;
            } else if(string.IsNullOrEmpty(BrideCountry)) {
                MessageBox.Show("Please Enter Bride Country");
                return false;
            } else if(MarriageDate == null) {
                MessageBox.Show("Please Enter Marriage Date");
                return false;
            } else if(MarriagePlace == null) {
                MessageBox.Show("Please Enter Marriage Place");
                return false;
            }

            return true;
        }

        private MarriageCertificate GetMarriageDetails() {
            var marriageDetails = new MarriageCertificate();
            marriageDetails.GroomName = GroomName.Trim();
            marriageDetails.GroomDOB = GroomDOB;
            marriageDetails.GroomFatherName = GroomFatherName.Trim();
            marriageDetails.GroomHouseName = GroomHouseName.Trim();
            marriageDetails.GroomArea = GroomArea.Trim();
            marriageDetails.GroomPincode = GroomPincode.Trim();
            marriageDetails.GroomPostOffice = GroomPostOffice.Trim();
            marriageDetails.GroomDistrict = GroomDistrict.Trim();
            marriageDetails.GroomState = GroomState.Trim();
            marriageDetails.GroomCountry = GroomCountry.Trim();
            if(GroomPhoto1 != null && !String.IsNullOrEmpty(GroomPhotoPath)) {
                marriageDetails.GroomPhoto = BufferFromImage(GroomPhoto1);
            }

            marriageDetails.BrideName = BrideName.Trim();
            marriageDetails.BrideDOB = BrideDOB;
            marriageDetails.BrideFatherName = BrideFatherName.Trim();
            marriageDetails.BrideHouseName = BrideHouseName.Trim();
            marriageDetails.BrideArea = BrideArea.Trim();
            marriageDetails.BridePincode = BridePincode.Trim();
            marriageDetails.BridePostOffice = BridePostOffice.Trim();
            marriageDetails.BrideDistrict = BrideDistrict.Trim();
            marriageDetails.BrideState = BrideState.Trim();
            marriageDetails.BrideCountry = BrideCountry.Trim();
            if(BridePhoto1 != null && !String.IsNullOrEmpty(BridePhotoPath)) {
                marriageDetails.BridePhoto = BufferFromImage(BridePhoto1);
            }
            marriageDetails.MarriageDate = MarriageDate;
            marriageDetails.MarriagePlace = MarriagePlace.Trim();
            return marriageDetails;
        }

        private void CurrentMarriageChanged() {
            if(CurrentMarriage != null) {
                GroomName = CurrentMarriage.GroomName;
                GroomDOB = CurrentMarriage.GroomDOB;
                GroomFatherName = CurrentMarriage.GroomFatherName;
                GroomHouseName = CurrentMarriage.GroomHouseName;
                GroomArea = CurrentMarriage.GroomArea;
                GroomPincode = CurrentMarriage.GroomPincode;
                GroomPostOffice = CurrentMarriage.GroomPostOffice;
                GroomDistrict = CurrentMarriage.GroomDistrict;
                GroomState = CurrentMarriage.GroomState;
                GroomCountry = CurrentMarriage.GroomCountry;
                if(CurrentMarriage.GroomPhoto != null) {
                    GroomPhoto = ImageFromBuffer(CurrentMarriage.GroomPhoto);
                    GroomPhoto1 = ImageFromBuffer(CurrentMarriage.GroomPhoto);
                } else {
                    GroomPhoto = null;
                }
                BrideName = CurrentMarriage.BrideName;
                BrideDOB = CurrentMarriage.BrideDOB;
                BrideFatherName = CurrentMarriage.BrideFatherName;
                BrideHouseName = CurrentMarriage.BrideHouseName;
                BrideArea = CurrentMarriage.BrideArea;
                BridePincode = CurrentMarriage.BridePincode;
                BridePostOffice = CurrentMarriage.BridePostOffice;
                BrideDistrict = CurrentMarriage.BrideDistrict;
                BrideState = CurrentMarriage.BrideState;
                BrideCountry = CurrentMarriage.BrideCountry;
                if(CurrentMarriage.BridePhoto != null) {
                    BridePhoto = ImageFromBuffer(CurrentMarriage.BridePhoto);
                    BridePhoto1 = ImageFromBuffer(CurrentMarriage.BridePhoto);
                } else {
                    BridePhoto = null;
                }
                MarriageDate = CurrentMarriage.MarriageDate;
                MarriagePlace = CurrentMarriage.MarriagePlace;
                //IsEnable = false;
            } else {
                //IsEnable = true;
                ClearMarriage();
            }
        }

        private void ClearMarriage() {
            GroomName = GroomFatherName = GroomHouseName = GroomArea = String.Empty;
            GroomPincode = GroomPostOffice = GroomDistrict = GroomState = GroomCountry = GroomPhotoPath = String.Empty;
            GroomDOB = BrideDOB = DOBEndDate;
            BrideName = BrideFatherName = BrideHouseName = BrideArea = String.Empty;
            BridePincode = BridePostOffice = BrideDistrict = BrideState = BrideCountry = BridePhotoPath = String.Empty;
            MarriageDate = DateTime.Now;
            MarriagePlace = String.Empty;
            GroomPhoto = null;
            GroomPhoto1 = null;
            BridePhoto = null;
            BridePhoto1 = null;
        }

        private void RefreshMarriages() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                MarriageList = new ObservableCollection<MarriageCertificate>(unitofWork.MarriageCertificates.GetAll());
                if(MarriageList != null && MarriageList.Count > 0) {
                    CurrentMarriage = MarriageList[0];
                }
            }
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes) {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public Byte[] BufferFromImage(BitmapImage imageSource) {
            Uri uri = imageSource.UriSource;
            Stream stream = Application.GetRemoteStream(uri).Stream;
            Byte[] buffer = null;
            if(stream != null && stream.Length > 0) {
                using(BinaryReader br = new BinaryReader(stream)) {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }
            return buffer;
        }
    }
}
