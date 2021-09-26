using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_chat_realtime_db.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public bool IsOnline { get; set; }
    }
}