using Microsoft.AspNet.SignalR;

namespace AmDmSite.Hubs
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
            System.Diagnostics.Debug.WriteLine("TestHub instantiated");
        }

        public static void Notify(string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.notify(msg);
        }

        public static void SendPushMessage(string message)
        {
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.displayMessage(message);
        }
    }
}