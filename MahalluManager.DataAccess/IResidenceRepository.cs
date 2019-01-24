using MahalluManager.Model;
using System.Collections.Generic;

namespace MahalluManager.DataAccess {
    public interface IResidenceRepository : IRepository<Residence> {
        IEnumerable<Residence> GetAreaWise();
    }
}
