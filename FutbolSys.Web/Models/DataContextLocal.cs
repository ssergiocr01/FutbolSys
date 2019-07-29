using FutbolSys.Domain;
using System.Data.Entity;

namespace FutbolSys.Web.Models
{
    public class DataContextLocal : DataContext
    {
        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }
    }
}