using iTextSharp.text;
using iTextSharp.text.pdf;
using MahalluManager.Infra;
using Microsoft.Practices.Prism.Commands;
using System;
using System.IO;
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
            get { return placeOfMarriage +"Moorad"; }
            set {
                placeOfMarriage = value;
                OnPropertyChanged("PlaceOfMarriage");
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

        public int BridePinCode {
            get { return bridePincode; }
            set {
                bridePincode = value;
                OnPropertyChanged("BridePinCode");
            }
        }



        private DelegateCommand saveMarriageCommand;

        public DelegateCommand SaveMarriageCommand {
            get { return saveMarriageCommand; }
            set { saveMarriageCommand = value; }
        }
        private void ExecuteSaveMarriageCommand() {
        }

        private void InitializeDatePicker() {
            StartDate = DateTime.Now.AddMonths(-2);
            EndDate = DateTime.Now;
        }


        private DelegateCommand generateCertificateCommand;

        public DelegateCommand GenerateCertificateCommand {
            get { return generateCertificateCommand; }
            set { generateCertificateCommand = value; }
        }
        private void ExecuteGenerateCertificate() {
            string certificateName = "Certificate.pdf";
            Document document = new Document();
            FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();


            Chunk dateOfMarriage = new Chunk(MarriageDate.ToShortDateString());
            dateOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f,BaseColor.BLACK);
            

            Chunk placeOfMarriage = new Chunk(PlaceOfMarriage);
            placeOfMarriage.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.BLACK);

            Phrase phrase = new Phrase("This is from Phrase.");
            document.Add(phrase);

            Image i = Image.GetInstance(GroomPhotoPath);
            i.Alignment = Image.UNDERLYING;
            i.ScaleToFit(100f, 160f);
            i.SetAbsolutePosition(100, 360);
            document.Add(i);

            i = Image.GetInstance(BridePhotoPath);
            i.Alignment = Image.UNDERLYING;
            i.ScaleToFit(100f, 160f);
            i.SetAbsolutePosition(100, 240);
            document.Add(i);

            Paragraph para = new Paragraph("This is from paragraph.");
            document.Add(para);

            Paragraph title = new Paragraph();
            title.Alignment = Element.ALIGN_CENTER;
            title.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
            title.Add("MARRIAGE CERTIFICATE");
            document.Add(title);

            Paragraph content = new Paragraph();
            content.SpacingBefore = 10;
            content.SpacingAfter = 10;
            content.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
            string contentText1 = "Certify that the Marriage between the above mentioned person was solemnised by Mahallu Khazi of Muhiyidheen Masjid, Iringal Moorad in the presence of their friends and relative ";
            string contentText2 = " at ";
            string contentText3 = " according to the Islamic Shareeath Law";
            content.Add(contentText1);
            content.Add(dateOfMarriage);
            content.Add(contentText2);
            content.Add(placeOfMarriage);
            content.Add(contentText3);
            document.Add(content);

            // the pdf content
            PdfContentByte cb = writer.DirectContent;
            //cb.AddImage(i);
            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 8);

            //// write the text in the pdf content
            //cb.BeginText();
            //cb.ShowTextAligned(1, GroomName, 520, 640, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(2, GroomFatherName, 100, 200, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(1, GroomHouseName, 230, 230, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(2, GroomArea, 150, 230, 0);
            //cb.EndText();
            //cb.BeginText();
            // cb.ShowTextAligned(2, GroomPincode.ToString(), 150, 230, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(1, BrideName, 520, 640, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(2, BrideFatherName, 100, 200, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(1, BrideHouseName, 230, 230, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(2, BrideArea, 150, 230, 0);
            //cb.EndText();
            //cb.BeginText();
            //cb.ShowTextAligned(2, BridePinCode.ToString(), 150, 230, 0);
            //cb.EndText();
            //cb.AddTemplate(page, 0, 0);   
            //page.Add(cb);

            document.Close();
            fs.Close();
            writer.Close();

            String currentFolder = Path.GetDirectoryName(Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, "Certificate.pdf");

        }
    }
}
