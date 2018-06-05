using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using ResourceBibleStudy.Helpers;
using ResourceBibleStudy.Models;

namespace ResourceBibleStudy
{
    public class ChatHub : Hub
    {
        #region Data Members

        static readonly List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static readonly List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public void Connect(string userName, string userImageUrl = null)
        {
            var id = Context.ConnectionId;


            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName, UserImageUrl = userImageUrl ?? "~/Content/themes/inspinia/img/avatar.jpg" });

                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName, userImageUrl);

            }

        }
        public void SendMessageToAll(string message)
        {
            var messsageDateTime = DateHelper.GetCurrentDate().ToString("F");

            // store last 100 messages in cache
            AddMessageinCache("Olaolu Testing", message, messsageDateTime);

            // Broad cast message
            Clients.All.messageReceived("Olaolu Testing", message, messsageDateTime, null);
        }

        public void SendMessageToAll(string userName, string message, string userImageUrl = null)
        {
            var messsageDateTime = DateHelper.GetCurrentDate().ToString("F");

            // store last 100 messages in cache
            AddMessageinCache(userName, message, messsageDateTime, userImageUrl);

            // Broad cast message
            Clients.All.messageReceived(userName, message, messsageDateTime, userImageUrl);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            var fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }

            return base.OnDisconnected(stopCalled);
        }


        #endregion

        #region private Messages

        private void AddMessageinCache(string userName, string message, string messageDateTime, string userImageUrl = null)
        {
            CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message, MessageDateTime = messageDateTime, UserImageUrl = userImageUrl });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        #endregion
    }
}