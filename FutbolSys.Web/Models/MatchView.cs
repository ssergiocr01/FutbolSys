using FutbolSys.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Web.Models
{
    [NotMapped]
    public class MatchView : Match
    {
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Fecha")]
        public string DateString { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public string TimeString { get; set; }

        [Display(Name = "Liga del Local")]
        public int LocalLeagueId { get; set; }

        [Display(Name = "Liga del Visitante")]
        public int VisitorLeagueId { get; set; }
    }
}