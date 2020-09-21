using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sales.BackEnd.Startup))]
namespace Sales.BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
