using System;

namespace MahalluManager.Model.EventTypes {
    public class SelectedYearType {
        public String SelectedYear { get; set; }
    }
    public class SystemTotalType : SelectedYearType {
        public Decimal Balance { get; set; }
    }
}
