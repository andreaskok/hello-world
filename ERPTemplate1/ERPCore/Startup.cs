using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERPCore.Startup))]
namespace ERPCore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
