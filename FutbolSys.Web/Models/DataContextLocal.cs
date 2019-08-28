using FutbolSys.Domain;
using System.Data.Entity;

namespace FutbolSys.Web.Models
{
    public class DataContextLocal : DataContext
    {
        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentGroup> TournamentGroups { get; set; }

        public DbSet<Date> Dates { get; set; }

        public DbSet<TournamentTeam> TournamentTeams { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<Prediction> Predictions { get; set; }
    }
}