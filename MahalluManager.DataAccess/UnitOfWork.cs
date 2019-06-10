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
            Categories = new Repository<Category>(mahalluDBContext);
            Contributions = new Repository<Contribution>(mahalluDBContext);

        }
        public IResidenceRepository Residences { get; private set; }
        public IRepository<ResidenceMember> ResidenceMembers { get; private set; }
        public IRepository<Area> Areas { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Contribution> Contributions { get; private set; }

        public IRepository<ContributionDetail> ContributionDetails { get; private set; }

        public int Complete() {
            return mahalluDBContext.SaveChanges();
        }

        public void Dispose() {
            mahalluDBContext.Dispose();
        }
    }
}
