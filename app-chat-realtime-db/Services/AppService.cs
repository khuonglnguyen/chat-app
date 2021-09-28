using app_chat_realtime_db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_chat_realtime_db.Services
{
    public class AppService
    {
        ChatAppEntities _context;
        public AppService()
        {
            _context = new ChatAppEntities();
        }

        public bool Login(LoginData loginData, out int userId)
        {
            userId = 0;
            var currentUser = _context.Users.FirstOrDefault(x => x.Username == loginData.Username && x.Password == loginData.Password);
            if (currentUser != null)
            {
                userId = currentUser.Id;
                return true;
            }
            return false;
        }

        public List<UserDTO> GetUserToChat()
        {
            var userId = int.Parse(HttpContext.Current.User.Identity.Name);

            return _context.Users.Include("UserConnections").Where(x => x.Id != userId).Select(x => new UserDTO
            {
                UserId = x.Id,
                Username = x.Username,
                Fullname = x.Fullname,
                IsOnline = x.UserConnections.Count > 0
            }).ToList();
        }

        internal int AddUserConnection(Guid guid)
        {
            var userId = int.Parse(HttpContext.Current.User.Identity.Name);
            _context.UserConnections.Add(new UserConnection
            {
                ConnectionId = guid,
                UserId = userId
            });
            _context.SaveChanges();
            return userId;
        }

        internal int RemoveUserConnection(Guid connectionId)
        {
            int userId = 0;
            var current = _context.UserConnections.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (current != null)
            {
                userId = current.UserId ?? 0;
                _context.UserConnections.Remove(current);
                _context.SaveChanges();
                return current.UserId ?? 0;
            }
            return userId;
        }

        internal IList<string> GetUserConnections(int userId)
        {
            return _context.UserConnections.Where(x => x.UserId == userId).Select(x => x.ConnectionId.ToString()).ToList();
        }

        internal void RemoveAllUserConnections(int userId)
        {
            var current = _context.UserConnections.Where(x => x.UserId == userId);
            _context.UserConnections.RemoveRange(current);
            _context.SaveChanges();
        }

        internal ChatBoxModel GetChatBox(int toUserId)
        {
            var userId = int.Parse(HttpContext.Current.User.Identity.Name);
            var toUser = _context.Users.FirstOrDefault(x => x.Id == toUserId);
            var messages = _context.Messages.Where(x => (x.FromUser == userId && x.ToUser.Value == toUserId) || (x.FromUser == toUserId && x.ToUser == userId)).Select(x=>new MessageDTO { Message=x.Msg}).ToList();
            return new ChatBoxModel
            {
                ToUser= ToUserDTO(toUser),
                Messages = messages
            };
        }

        public UserDTO ToUserDTO(User user)
        {
            return new UserDTO
            {
                Fullname = user.Fullname,
                UserId = user.Id,
                Username = user.Username
            };
        }
    }
}