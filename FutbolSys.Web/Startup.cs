using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FutbolSys.Web.Startup))]
namespace FutbolSys.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
