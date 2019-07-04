using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class LogEvent
    {
        public int LogId { get; set; }
        public string EventName { get; set; }
        public int EventActor { get; set; }
        public string EventReport { get; set; }

        public virtual PersonInfo EventActorNavigation { get; set; }
    }
}
