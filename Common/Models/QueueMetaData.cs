using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Common.Constants;

namespace Common.Models
{
    public class QueueMetaData
    {
        public DateTime ChatEntryTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public Guid? CurrentAgent { get; set; }
        public Guid ClientID { get; set; }
        public MessageFunctionType? Function { get; set; }
        public List<Tuple<DateTime, Guid, string>> Messages { get; set; }
        public bool IsClosed { get; set; }
    }
}