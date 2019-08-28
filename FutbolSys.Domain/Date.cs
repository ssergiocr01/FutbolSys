using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class Date
    {
        [Key]
        public int DateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es {1} caracteres")]
        [Index("Date_Name_TournamentId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Fecha")]
        public string Name { get; set; }

        [Display(Name = "Torneo")]
        [Index("Date_Name_TournamentId_Index", IsUnique = true, Order = 2)]
        public int TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

    }
}
