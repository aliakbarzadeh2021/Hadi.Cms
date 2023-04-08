using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Hadi.Cms.Web.OwinStartup))]
namespace Hadi.Cms.Web
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //	AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //	LoginPath = new PathString("/User/Login"),
            //});
        }
    }
}