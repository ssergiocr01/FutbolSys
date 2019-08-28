using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Index("Group_Name_Index", IsUnique = true)]
        [Display(Name = "Grupo")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name = "Usuario")]
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; }
    }
}
