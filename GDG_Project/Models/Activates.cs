using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class Activates
    {
        public Activates()
        {
            Active = true;
            NotActive = false;
            
            School = new HashSet<School>();
            Tournaments = new HashSet<Tournaments>();
            Trainer = new HashSet<Trainer>();
        }

        public short ActId { get; set; }
        public string ActName { get; set; }
        public string ActImg { get; set; }
        public byte? ActMinAge { get; set; }
        public bool? ActActive { get; set; }
        public string ActDescription { get; set; }

      
        public virtual ICollection<School> School { get; set; }
        public virtual ICollection<Tournaments> Tournaments { get; set; }
        public virtual ICollection<Trainer> Trainer { get; set; }
        public bool Active { get; }
        public bool NotActive { get; }
    }
}
