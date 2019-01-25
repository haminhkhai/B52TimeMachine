using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(B52TimeMachine.Startup))]
namespace B52TimeMachine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
