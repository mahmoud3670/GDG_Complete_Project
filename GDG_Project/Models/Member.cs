using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public int MemberInfo { get; set; }
        public short ActInfo { get; set; }
        public DateTime StartDate { get; set; }
        public bool MemberActive { get; set; }

        public virtual Activates ActInfoNavigation { get; set; }
        public virtual PersonInfo MemberInfoNavigation { get; set; }
    }
}
