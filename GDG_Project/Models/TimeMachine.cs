using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class TimeMachine
    {
        public string WeekEnd = "راحه اسبوعيه";
        public string Regular = "اعتيادي";
        public string Emergency = "عارضه";
        public string Sick = "مرضي";
        public string Absence = "غياب";
        public string Reast = "بدل راحه";
        public string furlough = "اذن";
        public int TimeId { get; set; }
        public int EmpId { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public DateTime TimeDate { get; set; }
        public string NoteCase { get; set; }

        public virtual Employees Emp { get; set; }
    }
}
