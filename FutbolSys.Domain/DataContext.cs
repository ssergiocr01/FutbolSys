using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FutbolSys.Domain
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new UsersMap());
            modelBuilder.Configurations.Add(new MatchesMap());
            modelBuilder.Configurations.Add(new GroupsMap());
        }

        public System.Data.Entity.DbSet<FutbolSys.Domain.User> Users { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.Team> Teams { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.UserType> UserTypes { get; set; }
    }
}
