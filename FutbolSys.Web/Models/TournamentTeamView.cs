using FutbolSys.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Web.Models
{
    [NotMapped]
    public class TournamentTeamView : TournamentTeam
    {        
        [Display(Name = "Liga")]
        public int LeagueId { get; set; }
    }
}