using MahalluManager.Model.Common;

namespace MahalluManager.Model.EventTypes {
    public class ExpenseType {
        public Expense Expense { get; set; }
        public Operation Operation { get; set; }
    }
}
