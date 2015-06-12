using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkflowMax.Startup))]
namespace WorkflowMax
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
