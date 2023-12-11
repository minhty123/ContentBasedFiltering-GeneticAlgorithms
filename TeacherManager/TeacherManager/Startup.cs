using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeacherManager.Startup))]
namespace TeacherManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
