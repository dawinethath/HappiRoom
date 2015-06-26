using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HappiRoom.Startup))]
namespace HappiRoom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
