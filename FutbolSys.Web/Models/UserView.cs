using FutbolSys.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FutbolSys.Web.Models
{
    public class UserView : User
    {
        [Display(Name = "Foto")]
        public HttpPostedFileBase PictureFile { get; set; }

        [Display(Name = "Liga Favorita")]
        public int FavoriteLeagueId { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(20, ErrorMessage = "La longitud del campo {0} debe estar entre {1} y {2} caracteres", MinimumLength = 6)]
        [Display(Name ="Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
        [Display(Name ="Confirmar Contraseña")]
        public string PasswordConfirm { get; set; }

        public List<Group> Groups { get; set; }
    }
}