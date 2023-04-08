using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Hadi.Cms.Web.Hubs
{
    public class ContentHub : Hub
    {
        public static ContentHub Current
        {
            get
            {
                DefaultHubManager hubManager = new DefaultHubManager(GlobalHost.DependencyResolver);
                return hubManager.ResolveHub("ContentHub") as ContentHub;
            }
        }

        public void PushNotification(string title, string text, string link)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ContentHub>();

            dynamic newContent = new
            {
                Title = title,
                Text = text,
                Link = link
            };

            context.Clients.All.pushNotification(newContent);
        }
    }
}