using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class TrainerSalary
    {
        public int TrainerSalaryId { get; set; }
        public int TrainerCode { get; set; }
        public int SchoolId { get; set; }
        public int? MemberCount { get; set; }
        public DateTime SalaryDate { get; set; }
        public double? Salary { get; set; }

        public virtual School School { get; set; }
        public virtual Trainer TrainerCodeNavigation { get; set; }
    }
}
