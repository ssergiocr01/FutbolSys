using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FutbolSys.Domain
{
    public class TournamentTeam
    {
        [Key]
        public int TournamentTeamId { get; set; }

        [Index("TournamentTeam_TournamentGroupId_TeamId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Grupo")]
        public int TournamentGroupId { get; set; }

        [Index("TournamentTeam_TournamentGroupId_TeamId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Equipo")]
        public int TeamId { get; set; }

        [Display(Name = "PJ")]
        public int MatchesPlayed { get; set; }

        [Display(Name = "PG")]
        public int MatchesWon { get; set; }

        [Display(Name = "PP")]
        public int MatchesLost { get; set; }

        [Display(Name = "PE")]
        public int MatchesTied { get; set; }

        [Display(Name = "GF")]
        public int FavorGoals { get; set; }

        [Display(Name = "GC")]
        public int AgainstGoals { get; set; }

        [Display(Name = "Puntos")]
        public int Points { get; set; }

        [Display(Name = "Posición")]
        public int Position { get; set; }

        public virtual TournamentGroup TournamentGroup { get; set; }

        public virtual Team Team { get; set; }

    }
}
