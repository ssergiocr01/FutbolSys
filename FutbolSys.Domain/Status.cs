using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Index("Status_Name_Index", IsUnique = true)]
        [Display(Name = "Estado")]
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
