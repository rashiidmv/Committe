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
            SaveMarriageCommand = new DelegateCommand(ExecuteSaveMarriageCommand);
            GenerateCertificateCommand = new DelegateCommand(ExecuteGenerateCertificateCommand);
        }

        private DateTime startDate;
        public DateTime StartDate {
            get { return startDate; }
            set {
                startDate = value;
                OnPropertyChanged("startDate");
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
            set { groomHouseName = value;
                OnPropertyChanged("GroomHouseName");
            }
        }

        private string groomArea;
        public string GroomArea {
            get { return groomArea; }
            set { groomArea = value;
                OnPropertyChanged("GroomArea");
            }
        }

        private int groomPincode;
        public int GroomPincode {
            get { return groomPincode; }
            set { groomPincode = value;
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
            set { brideFatherName = value;
                OnPropertyChanged("BrideFatherName");
            }
        }

        private string brideHouseName;
        public string BrideHouseName {
            get { return brideHouseName; }
            set { brideHouseName = value;
                OnPropertyChanged("BrideHouseName");
            }
        }

        private string brideArea;

        public string BrideArea {
            get { return brideArea; }
            set { brideArea = value;
                OnPropertyChanged("BrideArea");
            }
        }
        private int bridePincode;

        public int BridePinCode {
            get { return bridePincode; }
            set { bridePincode = value;
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
        private void ExecuteGenerateCertificateCommand() {
            string certificateName = "Certificate.pdf";
            PdfReader reader = new PdfReader("Template.pdf");
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);
            FileStream fs = new FileStream(certificateName, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();


            Chunk chunk = new Chunk("This is from chunk. ");
            document.Add(chunk);

            Phrase phrase = new Phrase("This is from Phrase.");
            document.Add(phrase);

            Image i = Image.GetInstance(BridePhotoPath);
            i.Alignment = Image.UNDERLYING;
            i.ScaleToFit(300f, 400f);
            //document.Add(i);
            i.SetAbsolutePosition(200, 200);

            Paragraph para = new Paragraph("This is from paragraph.");
            document.Add(para);

            string text1 = @"you are successfully created PDF file.";
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingBefore = 10;
            paragraph.SpacingAfter = 10;
            paragraph.Alignment = Element.ALIGN_LEFT;
            paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
            paragraph.Add(text1);
            document.Add(paragraph);

            // the pdf content
            PdfContentByte cb = writer.DirectContent;
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);
            cb.AddImage(i);
            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 8);

            // write the text in the pdf content
            cb.BeginText();
            cb.ShowTextAligned(1, GroomName, 520, 640, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, GroomFatherName, 100, 200, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(1, GroomHouseName, 230, 230, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, GroomArea, 150, 230, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, GroomPincode.ToString(), 150, 230, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(1, BrideName, 520, 640, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, BrideFatherName, 100, 200, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(1, BrideHouseName, 230, 230, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, BrideArea, 150, 230, 0);
            cb.EndText();
            cb.BeginText();
            cb.ShowTextAligned(2, BridePinCode.ToString(), 150, 230, 0);
            cb.EndText();
            //cb.AddTemplate(page, 0, 0);   
            // page.Add(cb);

            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();

            String currentFolder = Path.GetDirectoryName(Application.ExecutablePath);
            CertificatePath = Path.Combine(currentFolder, certificateName);

        }

    }
}
