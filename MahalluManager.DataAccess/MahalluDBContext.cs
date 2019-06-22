using MahalluManager.Model;
using System.Data.Entity;

namespace MahalluManager.DataAccess {
    public class MahalluDBContext : DbContext {
        public DbSet<Residence> Residence { get; set; }
        public DbSet<ResidenceMember> ResidenceMember { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<IncomeCategory> IncomeCategory { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public DbSet<Contribution> Contribution { get; set; }
        public DbSet<ContributionDetail> ContributionDetail { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseDetails> ExpenseDetail { get; set; }
    }
}
