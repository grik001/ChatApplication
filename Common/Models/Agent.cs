using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Common.Constants;

namespace Common.Models
{
    public class Agent
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public int MaxChats { get; set; }
        public int ServiceCount { get; set; }
        public bool IsPaused { get; set; }

        public HashSet<Guid> ActiveChats { get; set; }

        public Agent()
        {
            ActiveChats = new HashSet<Guid>();
            MaxChats = 2;
        }
    }
}