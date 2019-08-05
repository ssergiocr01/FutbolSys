using FutbolSys.Domain;
using System.Data.Entity;

namespace FutbolSys.Web.Models
{
    public class DataContextLocal : DataContext
    {
        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.Tournament> Tournaments { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.TournamentGroup> TournamentGroups { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.Date> Dates { get; set; }

        public System.Data.Entity.DbSet<FutbolSys.Domain.TournamentTeam> TournamentTeams { get; set; }
    }
}