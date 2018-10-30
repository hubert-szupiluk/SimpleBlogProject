using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleBlog.Startup))]
namespace SampleBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
