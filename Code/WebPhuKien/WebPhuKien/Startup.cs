using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPhuKien.Startup))]
namespace WebPhuKien
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
