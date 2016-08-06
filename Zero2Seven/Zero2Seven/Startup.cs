using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zero2Seven.Startup))]
namespace Zero2Seven
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
