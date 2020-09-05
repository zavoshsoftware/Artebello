using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Artebello.Startup))]
namespace Artebello

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
