using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Index("UserType_Name_Index", IsUnique = true)]
        [Display(Name = "Tipo de Usuario")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
