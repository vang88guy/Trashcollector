using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trashcollector.Startup))]
namespace Trashcollector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
