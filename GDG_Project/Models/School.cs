using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class School
    {
        public School()
        {
          
            Payment = new HashSet<Payment>();
            TrainerSalary = new HashSet<TrainerSalary>();
        }
        public bool Active = true;
        public bool NotActive = false;

        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolLevel { get; set; }
        public short SchoolAct { get; set; }
        public decimal SchoolPrice { get; set; }
        public DateTime SchoolStartDay { get; set; }
        public DateTime SchoolEndDay { get; set; }
        public short SchoolMaxTrainer { get; set; }
        public short SchoolMaxMember { get; set; }
        public string SchoolDays { get; set; }
        public bool SchoolActive { get; set; }
        public double TrainerPercent { get; set; }
        public int? SchoolMemberCount { get; set; }
        public int? SchoolTrainerCount { get; set; }

        public virtual Activates SchoolActNavigation { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<TrainerSalary> TrainerSalary { get; set; }
       
    }
}
