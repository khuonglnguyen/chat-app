using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_chat_realtime_db.Models
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}