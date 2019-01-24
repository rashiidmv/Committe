using MahalluManager.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator {
    public class MainViewModel : ViewModelBase, IMainViewModel {
        private string title;
        public string Title {
            get { return "Administrator"; }
            set { title = value; }
        }
    }
} 