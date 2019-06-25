using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Common {
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : UserControl, INotifyPropertyChanged {
        public PdfViewer() {
            InitializeComponent();
        }

        public static readonly DependencyProperty PdfFilePathProperty =
                DependencyProperty.Register("PdfFilePath", typeof(string), typeof(UserControl),
                    new PropertyMetadata(string.Empty, OnCaptionPropertyChanged));

        public string PdfFilePath {
            get { return (string)GetValue(PdfFilePathProperty); }
            set { SetValue(PdfFilePathProperty, value); }
        }
        private static void OnCaptionPropertyChanged(DependencyObject dependencyObject,
                       DependencyPropertyChangedEventArgs e) {
            PdfViewer myUserControl = dependencyObject as PdfViewer;
            myUserControl.OnPropertyChanged("PdfFilePath");
            myUserControl.OnPdfFilePathPropertyChanged(e);
        }
        private void OnPdfFilePathPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if(!String.IsNullOrEmpty((String)e.NewValue)) {

                pdfWebViewer.Navigate(new Uri((String)e.NewValue));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
