using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class Members
    {
        public Members()
        {
            Payment = new HashSet<Payment>();
            Tournaments = new HashSet<Tournaments>();
        }

        public int MemberCode { get; set; }
        public int MemberInfo { get; set; }
        public short MemberAct { get; set; }
        public string MemberLevel { get; set; }
        public bool? MemberActive { get; set; }

        public virtual Activates MemberActNavigation { get; set; }
        public virtual PersonInfo MemberInfoNavigation { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<Tournaments> Tournaments { get; set; }
    }
}
