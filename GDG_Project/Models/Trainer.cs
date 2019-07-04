using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDG_Project.Models
{
    public partial class Trainer
    {
        public Trainer()
        {
           
            Payment = new HashSet<Payment>();
            TrainerSalary = new HashSet<TrainerSalary>();
        }
        public bool Active = true;
        public bool DisActive = false;
         
        public int TrainerCode { get; set; }
        [Required]
        public int TrainerInfo { get; set; }
        [Required]
        public short TrainerAct { get; set; }
        [Required]
        public bool? TrainerActive { get; set; }
        [Required]
        public DateTime? TrainerStartDay { get; set; }

        public virtual Activates TrainerActNavigation { get; set; }
        public virtual PersonInfo TrainerInfoNavigation { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<TrainerSalary> TrainerSalary { get; set; }
    }
}
