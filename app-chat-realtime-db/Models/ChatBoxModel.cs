using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_chat_realtime_db.Models
{
    public class ChatBoxModel
    {
        public UserDTO ToUser { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}