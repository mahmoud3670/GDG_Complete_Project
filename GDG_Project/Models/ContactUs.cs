using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class ContactUs
    {
        public int MessageId { get; set; }
        public string AnonymsName { get; set; }
        public string AnonymsEmail { get; set; }
        public string AnonymsPhone { get; set; }
        public string AnonymsMessage { get; set; }
        public bool Opend { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
