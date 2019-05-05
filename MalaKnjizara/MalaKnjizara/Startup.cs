using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MalaKnjizara.Startup))]
namespace MalaKnjizara
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
