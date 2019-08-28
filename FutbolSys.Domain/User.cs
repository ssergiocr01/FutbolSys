using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutbolSys.Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public int UserTypeId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        [Index("User_Email_Index", IsUnique = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "La longitud máxima para el campo {0} es de {1} caracteres")]
        [Display(Name = "Apodo")]
        [Index("User_Nickname_Index", IsUnique = true)]
        public string Nickname { get; set; }

        [Display(Name = "Equipo Favorito")]
        public int FavoriteTeamId { get; set; }

        [Display(Name = "Puntos")]
        public int Points { get; set; }

        [Display(Name = "Usuario")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        [JsonIgnore]
        public virtual Team FavoriteTeam { get; set; }

        [JsonIgnore]
        public virtual ICollection<Group> UserGroups { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupUser> GroupUsers { get; set; }

        [JsonIgnore]
        public virtual ICollection<Prediction> Predictions { get; set; }

        [JsonIgnore]
        public string PictureFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Picture))
                {
                    return string.Empty;
                }

                //if (UserTypeId == 1)
                //{
                //    return string.Format("http://soccerapi.azurewebsites.net{0}", Picture.Substring(1));
                //}

                return Picture;
            }
        }
    }
}
