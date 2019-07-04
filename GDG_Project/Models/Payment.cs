using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDG_Project.Models
{
    public partial class Payment
    {
        public short Monthly = 1;
        public short quarter = 2;
        public short Half = 3;
        public short PerYear = 4;


        public int PayId { get; set; }
        [Required]
        public int MemberInfo { get; set; }
        [Required]
        public int SchoolId { get; set; }
        [Required]
        public int TrainerCode { get; set; }
        [Required]
        public DateTime PayDate { get; set; }
        public DateTime PayDueDate { get; set; }
        [Required]
        public double PayAmountPaid { get; set; }
        public double? PayDiscount { get; set; }
        [Required]
        public short? PayType { get; set; }

        public virtual PersonInfo MemberInfoNavigation { get; set; }
        public virtual School School { get; set; }
        public virtual Trainer TrainerCodeNavigation { get; set; }
    }
}
