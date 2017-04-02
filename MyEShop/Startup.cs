using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyEShop.Web.Startup))]
namespace MyEShop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
