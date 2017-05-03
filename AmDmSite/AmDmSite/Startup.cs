using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmDmSite.Startup))]
namespace AmDmSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
