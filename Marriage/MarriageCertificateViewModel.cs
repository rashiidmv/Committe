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

        private string placeOfMarriage;

        public string PlaceOfMarriage {
            get { return placeOfMarriage + "Moorad"; }
            set {
                placeOfMarriage = value;
                OnPropertyChanged("PlaceOfMarriage");
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
            //FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write);
            FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write, FileShare.Delete);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            Paragraph title = new Paragraph();
            title.Alignment = Element.ALIGN_CENTER;
            title.Font = FontFactory.GetFont(FontFactory.HELVETICA, 20f, BaseColor.GREEN);
            title.Add("MARRIAGE CERTIFICATE");
            document.Add(title);

            Phrase phrase = new Phrase("This is from Phrase.");
            document.Add(phrase);

            PdfContentByte pdfContentByte = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            pdfContentByte.SetColorFill(BaseColor.DARK_GRAY);
            pdfContentByte.SetFontAndSize(bf, 14);

            int column1LeftMargin = 160;
            int column2LeftMargin = 350;

            if(!String.IsNullOrEmpty(GroomPhotoPath) && !String.IsNullOrEmpty(GroomPhotoPath)) {
                Image i = Image.GetInstance(GroomPhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(100f, 160f);
                i.SetAbsolutePosition(column1LeftMargin, 360);
                pdfContentByte.AddImage(i);
                i = Image.GetInstance(BridePhotoPath);
                i.Alignment = Image.LEFT_ALIGN;
                i.ScaleToFit(100f, 160f);
                i.SetAbsolutePosition(column2LeftMargin, 360);
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

           BrideName = "Hiba Fahisa AV";
           BrideFatherName = "Asharaf AV";
           BrideHouseName = "Kulangarath Kuniyil";
           BrideArea = "Arakkilad";
           BridePincode = 675302;
           BridePostOffice = "Nadakkuthazhe";
           BrideDistrict = "Kozhikkode";
           BrideState = "Kerala";


            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomName, column1LeftMargin, 340, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideName, column2LeftMargin, 340, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "S/o " + GroomFatherName, column1LeftMargin, 322, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "D/o " + BrideFatherName, column2LeftMargin, 322, 0);
            pdfContentByte.EndText();


            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "Date of Birth :", 70, 300, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomDOB.ToShortDateString(), column1LeftMargin, 304, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideDOB.ToShortDateString(), column2LeftMargin, 304, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, "Address :", 70, 286, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomHouseName, column1LeftMargin, 286, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideHouseName, column2LeftMargin, 286, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomArea, column1LeftMargin, 268, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideArea, column2LeftMargin, 268, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomPostOffice, column1LeftMargin, 250, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BridePostOffice, column2LeftMargin, 250, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomPincode.ToString(), column1LeftMargin, 232, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BridePincode.ToString(), column2LeftMargin, 232, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomDistrict, column1LeftMargin, 214, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideDistrict, column2LeftMargin, 214, 0);
            pdfContentByte.EndText();

            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, GroomState, column1LeftMargin, 198, 0);
            pdfContentByte.EndText();
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(0, BrideState, column2LeftMargin, 198, 0);
            pdfContentByte.EndText();

            Chunk dateOfMarriage = new Chunk(MarriageDate.ToShortDateString());
            dateOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, BaseColor.BLACK);
            Chunk placeOfMarriage = new Chunk(PlaceOfMarriage);
            placeOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, BaseColor.BLACK);
            Paragraph content = new Paragraph();
            content.SpacingBefore = 10;
            content.SpacingAfter = 10;
            content.Font = FontFactory.GetFont(FontFactory.HELVETICA, 14f, BaseColor.BLACK);
            string contentText1 = "        Certify that the Marriage between the above mentioned person was solemnised by Mahallu Khazi of Muhiyidheen Masjid, Iringal Moorad in the presence of their friends and relatives on ";
            string contentText2 = " at ";
            string contentText3 = " according to the Islamic Shareeath Law.";
            content.Add(contentText1);
            content.Add(dateOfMarriage);
            content.Add(contentText2);
            content.Add(placeOfMarriage);
            content.Add(contentText3);
            content.Alignment = Element.ALIGN_JUSTIFIED;
            ColumnText ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 200, 560, 50));
            ct.AddElement(content);
            ct.Go();

            Paragraph secretary = new Paragraph("SECRETARY");
            secretary.Alignment = Element.ALIGN_RIGHT;
            secretary.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, BaseColor.BLACK);

            ct = new ColumnText(writer.DirectContent);
            ct.SetSimpleColumn(new Rectangle(30, 120, 540, 30));
            ct.AddElement(secretary);
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
