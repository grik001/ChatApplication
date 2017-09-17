using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Models
{
    public class QueueMetaData
    {
        public DateTime ChatEntryTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsActive { get; set; }
        public Guid? CurrentAgent { get; set; }
        public Guid ClientID { get; set; }
    }
}