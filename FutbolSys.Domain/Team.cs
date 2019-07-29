using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolSys.Domain
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Liga")]
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
