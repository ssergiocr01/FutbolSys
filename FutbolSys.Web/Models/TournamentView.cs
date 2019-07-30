using FutbolSys.Domain;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FutbolSys.Web.Models
{
    public class TournamentView : Tournament
    {
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }
    }
}