using System;
using System.Collections.Generic;

namespace GDG_Project.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsContent { get; set; }
        public int NewsNviwer { get; set; }
        public DateTime NewsDate { get; set; }
        public string NewsImg { get; set; }
    }
}
