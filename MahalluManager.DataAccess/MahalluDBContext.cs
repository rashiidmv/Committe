using MahalluManager.Model;
using System.Data.Entity;

namespace MahalluManager.DataAccess {
    public class MahalluDBContext : DbContext {
        public DbSet<Residence> Residence { get; set; }
        public DbSet<ResidenceMember> ResidenceMember { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
