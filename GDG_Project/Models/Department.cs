using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employees>();
        }

        public short DepId { get; set; }
        public string DepName { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
