using app_chat_realtime_db.Models;
using app_chat_realtime_db.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace app_chat_realtime_db.Hubs
{
    public class ChatHub : Hub
    {
        public static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        public override Task OnConnected()
        {
            int userId = new AppService().AddUserConnection(Guid.Parse(Context.ConnectionId));
            Clients.All.BroadcastOnlineUser(userId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            int userId = new AppService().RemoveUserConnection(Guid.Parse(Context.ConnectionId));
            Clients.All.BroadcastOfflineUser(userId);
            return base.OnDisconnected(stopCalled);
        }

        public void GetUserToChat()
        {
            int userId = int.Parse(HttpContext.Current.User.Identity.Name);
            List<UserDTO> users = new AppService().GetUserToChat();
            Clients.Clients(new AppService().GetUserConnections(userId)).BroadcastUserToChat(users);
        }

        internal static void OfflineUser(int userId)
        {
            context.Clients.All.BroadcastOfflineUser(userId);
        }
    }
}