using FutbolSys.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FutbolSys.Web.Models
{
    [NotMapped]
    public class GroupView : Group
    {
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }
    }
}