using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDG_Project.Models
{
    public partial class Tournaments
    {
        public int TourId { get; set; }
        [Required]
        public int MemberInfo { get; set; }
        [Required]
        public string TourName { get; set; }
        [Required]
        public string TourDescription { get; set; }
        [Required]
        public DateTime TourDate { get; set; }

        public string MemberImg { get; set; }
        [Required]
        public string MemberLevel { get; set; }
        [Required]
        public short ActId { get; set; }

        public virtual Activates Act { get; set; }
        public virtual PersonInfo MemberInfoNavigation { get; set; }
    }
}
