using MahalluManager.Model;
using System;

namespace MahalluManager.DataAccess {
    public interface IUnitOfWork : IDisposable {
        IResidenceRepository Residences { get; }
        IRepository<ResidenceMember> ResidenceMembers { get; }
        IRepository<Area> Areas { get; }
        IRepository<Category> Categories { get; }
        IRepository<Contribution> Contributions { get; }
        IRepository<ContributionDetail> ContributionDetails { get; }

        int Complete();
    }
}
