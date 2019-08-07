using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahalluManager.Model.SeedData {
    public static class MarriageStatus {
        public static List<String> MarriageStatuses {
            get {
                return new List<String>() { "Married", "Batchelor", "Spinster", "Widower", "Widow", "Divorcee" };
            }
        }
    }
}
