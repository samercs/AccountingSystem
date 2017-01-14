using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountingSystem.Startup))]
namespace AccountingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
