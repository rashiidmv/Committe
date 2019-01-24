using MahalluManager.Model;
using System.Collections.Generic;
using System.Linq;

namespace MahalluManager.DataAccess {
    public class ResidenceRepository : Repository<Residence>, IResidenceRepository {
        public MahalluDBContext MahalluDBContext { get; private set; }
        public ResidenceRepository(MahalluDBContext mahalluDBContext) : base(mahalluDBContext) {
            MahalluDBContext = mahalluDBContext;
        }

        public IEnumerable<Residence> GetAreaWise() {
            return MahalluContext.Residence.ToList();
        }
        public MahalluDBContext MahalluContext { get { return Context as MahalluDBContext; } }
    }
}
