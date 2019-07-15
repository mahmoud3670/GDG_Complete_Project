using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDG_Project.Models
{
    public partial class PersonInfo
    {
        public PersonInfo()
        {
            Employees = new HashSet<Employees>();
            LogEvent = new HashSet<LogEvent>();
            Member = new HashSet<Member>();
            Payment = new HashSet<Payment>();
            Tournaments = new HashSet<Tournaments>();
            Trainer = new HashSet<Trainer>();
        }

        public string TrainerLabel = "t";
        public string MembersLabel = "m";
        public string EmployeesLabel = "e";

        public string male = "m";
        public string famel = "f";
        //person info
        [Key]
        public int PId { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression("^[\u0621-\u064A\u0660-\u0669 ]+$", ErrorMessage = "ادخل الاسم صحيح")]
        [MaxLength(40, ErrorMessage = "اقصي عدد للحروف 40")]
        [MinLength(5, ErrorMessage = "اقل عدد للحروف 5")]
        public string PName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date, ErrorMessage = "ادخل تاريخ صحيح")]
        public DateTime PBirthDate { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression("(2|3)[0-9][0-9](01|02|03|04|05|06|07|08|09|10|11|12)[0-3][0-9](01|02|03|04|11|12|13|14|15|16|17|18|19|21|22|23|24|25|26|27|28|29|31|32|33|34|35|88)[0-9]{5}", ErrorMessage = "ادخل رقم قومي صحيح")]
        public string PNationalId { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression("(^(010|012|011|015))[0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صحيح")]
        public string PPhone { get; set; }
        //[Required(ErrorMessage = "*")]
        public string PImg { get; set; }

        [EmailAddress]
        public string PEmail { get; set; }

        public string PType { get; set; }
        [Required(ErrorMessage = "*")]
        public string PGender { get; set; }
        [Required(ErrorMessage = "*")]
        public string PAdress { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date, ErrorMessage = "ادخل تاريخ صحيح")]
        public DateTime? PStartDate { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<LogEvent> LogEvent { get; set; }
        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<Tournaments> Tournaments { get; set; }
        public virtual ICollection<Trainer> Trainer { get; set; }
    }
}
