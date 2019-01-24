using MahalluManager.DataAccess;
using MahalluManager.Model;
using System.Windows;
using System.Windows.Controls;

namespace Administrator {
    /// <summary>
    /// Interaction logic for NewResident.xaml
    /// </summary>
    public partial class NewResident : UserControl {
        public NewResident() {
            InitializeComponent();
            this.DataContext = new NewResidentViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            using(var unitofWork=new UnitOfWork(new MahalluDBContext())) {
                var x =unitofWork.Residences.GetAll();
                Residence r = new Residence();
                r.Name = "Manappuram Vayalil";
                r.Number = "rmmlasdf";
                r.Area = "Kottakunnu";

                unitofWork.Residences.Add(r);
                unitofWork.Complete();
            }
        }
    }
}
