using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Subjoin.Web.Startup))]
namespace Subjoin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
