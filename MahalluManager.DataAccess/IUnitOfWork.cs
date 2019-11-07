using MahalluManager.Model;
using System;

namespace MahalluManager.DataAccess {
    public interface IUnitOfWork : IDisposable {
        IResidenceRepository Residences { get; }
        IRepository<ResidenceMember> ResidenceMembers { get; }
        IRepository<Area> Areas { get; }
        IRepository<IncomeCategory> IncomeCategories { get; }
        IRepository<ExpenseCategory> ExpenseCategories { get; }
        IRepository<Contribution> Contributions { get; }
        IRepository<ContributionDetail> ContributionDetails { get; }
        IRepository<Expense> Expenses { get; }
        IRepository<ExpenseDetails> ExpenseDetails { get; }
        IRepository<MarriageCertificate> MarriageCertificates { get; }
        IRepository<CashSource> CashSources { get; }
        int Complete();
    }
}
