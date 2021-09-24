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
    }
}