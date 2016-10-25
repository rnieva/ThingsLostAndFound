using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThingsLostAndFound.Startup))]
namespace ThingsLostAndFound
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
