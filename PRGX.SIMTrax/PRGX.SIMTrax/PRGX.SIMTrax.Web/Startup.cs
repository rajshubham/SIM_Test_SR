using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PRGX.SIMTrax.Web.Startup))]
namespace PRGX.SIMTrax.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
