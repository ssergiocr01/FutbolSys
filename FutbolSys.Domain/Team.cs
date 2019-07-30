using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Equipo")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(3, ErrorMessage = "La longitud del campo {0} debe ser {1} caracteres", MinimumLength = 3)]
        public string Initials { get; set; }

        [Display(Name = "Liga")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 2)]
        public int LeagueId { get; set; }

        public virtual League League { get; set; }
    }
}
