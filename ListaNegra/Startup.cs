using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ListaNegra.Startup))]
namespace ListaNegra
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
