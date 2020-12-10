using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jctravel01.Startup))]
namespace jctravel01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
