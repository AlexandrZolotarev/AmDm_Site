using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AmDmSite.Startup))]
namespace AmDmSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
