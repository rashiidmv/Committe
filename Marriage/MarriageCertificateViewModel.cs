using iTextSharp.text;
using iTextSharp.text.pdf;
using MahalluManager.Infra;
using Microsoft.Practices.Prism.Commands;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Marriage {
    public class MarriageCertificateViewModel : ViewModelBase {
        public MarriageCertificateViewModel() {
            InitializeDatePicker();
            GenerateCertificateCommand = new DelegateCommand(ExecuteGenerateCertificate);
            SaveMarriageCommand = new DelegateCommand(ExecuteSaveMarriage);
        }

        private void ExecuteSaveMarriage() {
            String currentFolder = Path.GetDirectoryName(Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, "Certificate.pdf");
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
                if(!String.IsNullOrEmpty(bridePhotoPath)) {
                    var uriSource = new Uri(bridePhotoPath);
                    bridePhoto = new BitmapImage(uriSource);
                }
                return bridePhoto;
            }
        }

        private BitmapImage groomPhoto;
        public BitmapImage GroomPhoto {
            get {
                if(!String.IsNullOrEmpty(groomPhotoPath)) {
                    var uriSource = new Uri(groomPhotoPath);
                    groomPhoto = new BitmapImage(uriSource);
                }
                return groomPhoto;
            }
        }

        private string bridePhotoPath;
        public string BridePhotoPath {
            get { return bridePhotoPath; }
            set {
                bridePhotoPath = value;
                OnPropertyChanged("BridePhotoPath");
                OnPropertyChanged("BridePhoto");
            }
        }

        private string groomPhotoPath;
        public string GroomPhotoPath {
            get { return groomPhotoPath; }
            set {
                groomPhotoPath = value;
                OnPropertyChanged("GroomPhotoPath");
                OnPropertyChanged("GroomPhoto");
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

        private int groomPincode;
        public int GroomPincode {
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
        private int bridePincode;
        public int BridePincode {
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

        private DelegateCommand saveMarriageCommand;
        public DelegateCommand SaveMarriageCommand {
            get { return saveMarriageCommand; }
            set { saveMarriageCommand = value; }
        }
        private void ExecuteSaveMarriageCommand() {
        }

        private DelegateCommand generateCertificateCommand;
        public DelegateCommand GenerateCertificateCommand {
            get { return generateCertificateCommand; }
            set { generateCertificateCommand = value; }
        }
        private void ExecuteGenerateCertificate() {
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
            GroomPincode = 673521;
            GroomPostOffice = "Iringal";
            GroomDistrict = "Kozhikkode";
            GroomState = "Kerala";
            GroomCountry = "India";

            BrideName = "Hiba Fahisa AV";
            BrideFatherName = "Asharaf AV";
            BrideHouseName = "Kulangarath Kuniyil";
            BrideArea = "Arakkilad";
            BridePincode = 675302;
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

            Paragraph place = new Paragraph("Place : " + Place + "\nDate : " + DateTime.Now.ToShortDateString());
            place.Alignment = Element.ALIGN_LEFT;
            place.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f, BaseColor.BLACK);
            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(40, 120, 540, 30));
            ct.AddElement(place);
            ct.Go();

            document.Close();
            fs.Close();
            writer.Close();

            String currentFolder = Path.GetDirectoryName(Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, "Certificate.pdf");
        }

        private void InitializeDatePicker() {
            DOBEndDate = DateTime.Now.AddYears(-18);
            MarriageDate = EndDate = DateTime.Now;
            GroomDOB = BrideDOB = DOBEndDate;
        }
    }
}
