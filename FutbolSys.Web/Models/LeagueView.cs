using FutbolSys.Domain;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FutbolSys.Web.Models
{
    public class LeagueView : League
    {
        [Display(Name = "Logo")]
        public HttpPostedFileBase LogoFile { get; set; }
    }
}