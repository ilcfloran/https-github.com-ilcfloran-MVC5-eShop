using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyEShop.Startup))]
namespace MyEShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
