using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatWebApplication.API.Models
{
    public class ChatQueuePostRequest
    {
        public Guid ChatQueueKey { get; set; }
        public string Username { get; set; }
    }
}