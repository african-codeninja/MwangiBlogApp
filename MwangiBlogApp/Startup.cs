using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MwangiBlogApp.Startup))]
namespace MwangiBlogApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
