using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkflowMaxMVC.Startup))]
namespace WorkflowMaxMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
