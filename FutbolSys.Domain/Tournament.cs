using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class Tournament
    {
        [Key]
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es {1} caracteres")]
        [Index("Tournament_Name_Index", IsUnique = true)]
        [Display(Name = "Torneo")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name ="¿Está Activo?")]
        public bool IsActive { get; set; }

        [Display(Name ="Orden")]
        public int Order { get; set; }

        public virtual ICollection<TournamentGroup> TournamentGroups { get; set; }
    }
}
