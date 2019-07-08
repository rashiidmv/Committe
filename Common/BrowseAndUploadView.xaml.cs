using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Common {
    /// <summary>
    /// Interaction logic for BrowseAndUpload.xaml
    /// </summary>
    public partial class BrowseAndUploadView : UserControl, INotifyPropertyChanged {
        public BrowseAndUploadView() {
            InitializeComponent();
        }

            public static readonly DependencyProperty FilePathProperty =
                    DependencyProperty.Register("FilePath", typeof(string), typeof(UserControl),
                        new PropertyMetadata(string.Empty, OnCaptionPropertyChanged));

            public string FilePath {
                get { return (string)GetValue(FilePathProperty); }
                set { SetValue(FilePathProperty, value); }
            }
            private static void OnCaptionPropertyChanged(DependencyObject dependencyObject,
                           DependencyPropertyChangedEventArgs e) {
                BrowseAndUploadView myUserControl = dependencyObject as BrowseAndUploadView;
                myUserControl.OnPropertyChanged("FilePath");
                myUserControl.OnFilePathPropertyChanged(e);
            }
        private void OnFilePathPropertyChanged(DependencyPropertyChangedEventArgs e) {
            FileName.Text = (String)e.NewValue;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public byte[] FileContent { get; private set; }
        private void BrowseFileOpen_Click(object sender, System.Windows.RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Images (.jpeg) | *.jpeg;*.png| All Files (*.*) | *.*";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Nullable<bool> result = openFileDialog.ShowDialog();
            if(result == true) {
                FileName.Text = openFileDialog.FileName;
                FileName.ToolTip = FileName.Text;
                FilePath = FileName.Text;
                FileContent = File.ReadAllBytes(FileName.Text);
            }
        }
    }
}
