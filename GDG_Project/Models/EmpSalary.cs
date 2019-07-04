using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class EmpSalary
    {
        public int SalaryId { get; set; }
        public DateTime SalaryDate { get; set; }
        public int EmpId { get; set; }
        public double Salary { get; set; }

        public virtual Employees Emp { get; set; }
    }
}
