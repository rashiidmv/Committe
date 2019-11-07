using MahalluManager.Model;

namespace MahalluManager.DataAccess {
    public class UnitOfWork : IUnitOfWork {

        private readonly MahalluDBContext mahalluDBContext;
        public UnitOfWork(MahalluDBContext context) {
            //Database.SetInitializer<MahalluDBContext>(null);  //for db upgrade
            mahalluDBContext = context;
            Residences = new ResidenceRepository(mahalluDBContext);
            ResidenceMembers = new Repository<ResidenceMember>(mahalluDBContext);
            Areas = new Repository<Area>(mahalluDBContext);
            IncomeCategories = new Repository<IncomeCategory>(mahalluDBContext);
            ExpenseCategories = new Repository<ExpenseCategory>(mahalluDBContext);
            Contributions = new Repository<Contribution>(mahalluDBContext);
            ContributionDetails = new Repository<ContributionDetail>(mahalluDBContext);
            Expenses = new Repository<Expense>(mahalluDBContext);
            ExpenseDetails = new Repository<ExpenseDetails>(mahalluDBContext);
            MarriageCertificates = new Repository<MarriageCertificate>(mahalluDBContext);
            CashSources = new Repository<CashSource>(mahalluDBContext);

        }
        public IResidenceRepository Residences { get; private set; }
        public IRepository<ResidenceMember> ResidenceMembers { get; private set; }
        public IRepository<Area> Areas { get; private set; }
        public IRepository<IncomeCategory> IncomeCategories { get; private set; }
        public IRepository<ExpenseCategory> ExpenseCategories { get; private set; }
        public IRepository<Contribution> Contributions { get; private set; }
        public IRepository<ContributionDetail> ContributionDetails { get; private set; }
        public IRepository<Expense> Expenses { get; private set; }
        public IRepository<ExpenseDetails> ExpenseDetails { get; private set; }
        public IRepository<MarriageCertificate> MarriageCertificates { get; private set; }
        public IRepository<CashSource> CashSources { get; private set; }

        public int Complete() {
            return mahalluDBContext.SaveChanges();
        }

        public void Dispose() {
            mahalluDBContext.Dispose();
        }
    }
}
