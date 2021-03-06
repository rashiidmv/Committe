﻿using Expense;
using MahalluManager.Infra;
using Marriage;
using Resident;
using System.Windows.Controls;

namespace Administrator {
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl, IView {
        public Main(IMainViewModel vm) {
            InitializeComponent();
            ViewModel = vm;
            TabItem newResident = new TabItem {
                Header = "New Resident",
                Content = new NewResident()
            };
            tabControl1.Items.Add(newResident);

            TabItem contribution = new TabItem {
                Header = "Contribution",
                Content = new Contribution.ContributionView()
            };
            tabControl1.Items.Add(contribution);

            TabItem expense = new TabItem {
                Header = "Expense",
                Content = new ExpenseView()
            };
            tabControl1.Items.Add(expense);

            TabItem marriage = new TabItem {
                Header = "Marriage Certificate",
                Content = new MarriageCertificateView()
            };
            tabControl1.Items.Add(marriage);

            TabItem settings = new TabItem {
                Header = "Settings",
                Content = new Settings()
            };
            tabControl1.Items.Add(settings);
        }

        public IViewModel ViewModel {
            get {
                return (IViewModel)DataContext; ;
            }

            set {
                DataContext = value;
            }
        }

    }
}