using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Common {
    /// <summary>
    /// Interaction logic for InputOuputColumns.xaml
    /// </summary>
    public partial class InputOuputColumns : UserControl, INotifyPropertyChanged {
        public InputOuputColumns() {
            InitializeComponent();
            if(InputColumnList?.Items?.Count > 0) {
                InputColumnList.SelectedIndex = 0;
            }
            if(OutputColumnList?.Items?.Count > 0) {
                OutputColumnList.SelectedIndex = 0;
            }
        }
        private IEnumerable<String> input;
        public IEnumerable<String> Input {
            get { return input; }
            set {
                input = value;
                InputColumnList.ItemsSource = input;
            }
        }

        private ObservableCollection<String> output;
        public ObservableCollection<String> Output {
            get { return output; }
            set {
                output = value;
                OutputColumnList.ItemsSource = output;
            }
        }

        public static readonly DependencyProperty InputListProperty =
                DependencyProperty.Register("InputList", typeof(IEnumerable<String>), typeof(UserControl),
                    new PropertyMetadata(null, OnInputListPropertyChanged));

        public IEnumerable<String> InputList {
            get { return (IEnumerable<String>)GetValue(InputListProperty); }
            set { SetValue(InputListProperty, value); }
        }
        private static void OnInputListPropertyChanged(DependencyObject dependencyObject,
                       DependencyPropertyChangedEventArgs e) {
            InputOuputColumns myUserControl = dependencyObject as InputOuputColumns;
            myUserControl.OnPropertyChanged("InputList");
            myUserControl.OnInputListChanged(e);
        }
        private void OnInputListChanged(DependencyPropertyChangedEventArgs e) {
            Input = (IEnumerable<String>)e.NewValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static readonly DependencyProperty OutputListProperty =
                DependencyProperty.Register("OutputList", typeof(IEnumerable<String>), typeof(UserControl),
                    new PropertyMetadata(null, OnOutputListPropertyChanged));

        public IEnumerable<String> OutputList {
            get { return (IEnumerable<String>)GetValue(OutputListProperty); }
            set { SetValue(OutputListProperty, value); }
        }
        private static void OnOutputListPropertyChanged(DependencyObject dependencyObject,
                       DependencyPropertyChangedEventArgs e) {
            InputOuputColumns myUserControl = dependencyObject as InputOuputColumns;
            myUserControl.OnPropertyChanged("OutputList");
            myUserControl.OnOutputListChanged(e);
        }
        private void OnOutputListChanged(DependencyPropertyChangedEventArgs e) {
            Output = (ObservableCollection<String>)e.NewValue;
        }
        private void AddColumn_Click(object sender, RoutedEventArgs e) {
            if(InputColumnList?.Items?.Count > 0) {
                InputColumnList.SelectedIndex = 0;
            }
            String selectedValue = (String)InputColumnList.SelectedValue;
            if(!string.IsNullOrEmpty(selectedValue)) {
                ((ObservableCollection<String>)Output).Add(selectedValue);
                ((ObservableCollection<String>)Input).Remove(selectedValue);
            }
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e) {
            if(OutputColumnList?.Items?.Count > 0) {
                OutputColumnList.SelectedIndex = 0;
            }
            String selectedValue = (String)OutputColumnList.SelectedValue;
            if(!string.IsNullOrEmpty(selectedValue)) {
                ((ObservableCollection<String>)Input).Add(selectedValue);
                ((ObservableCollection<String>)Output).Remove(selectedValue);
            }
        }

        private void AddAllColumn_Click(object sender, RoutedEventArgs e) {
            foreach(var item in Input) {
                ((ObservableCollection<String>)Output).Add(item);
            }
            ((ObservableCollection<String>)Input).Clear();
        }

        private void RemoveAllColumn_Click(object sender, RoutedEventArgs e) {
            foreach(var item in Output) {
                ((ObservableCollection<String>)Input).Add(item);
            }
            ((ObservableCollection<String>)Output).Clear();
        }
    }
}
