using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDG_Project.Models
{
    public partial class Employees
    {
        public Employees()
        {
            EmpSalaryNavigation = new HashSet<EmpSalary>();
            TimeMachine = new HashSet<TimeMachine>();
        }
        public short admin = 1;
        public short accounting = 2;
        public short worker = 3;
        public short HR = 4;
        public short dataEntry = 5;

        public bool active = true;
        public bool NotActive = false;

        public int EmpId { get; set; }
        public int EmpInfo { get; set; }
        [Required]
        public short EmpDepartment { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal EmpSalary { get; set; }


        [Required(ErrorMessage = "من فضلك قم بادخال اسم المستخدم")]
        [MaxLength(40, ErrorMessage = "اقصي عدد للحروف 40")]
        [MinLength(5, ErrorMessage = "اقل عدد للحروف 5")]
        //[RegularExpression("/^[a-zA-Zء-ي ]+$/u",ErrorMessage ="ادخل الاسم صحيح")]
        public string EmpUserName { get; set; }


        [Required(ErrorMessage = "ادخل كلمه المرور")]
        [MinLength(8, ErrorMessage = "يجب ان لا تقل عن 8")]
        [MaxLength(20, ErrorMessage = "يجب ان لا تزيد عن 20")]
        public string EmpPassword { get; set; }



        [Required]
        [DataType(DataType.Password)]
        [Compare("EmpPassword")]
        [NotMapped]
        public string conPassword { get; set; }


        public bool? EmpActive { get; set; }

        [Required]
        public short? EmpPostion { get; set; }



        public virtual Department EmpDepartmentNavigation { get; set; }
        public virtual PersonInfo EmpInfoNavigation { get; set; }
        public virtual ICollection<EmpSalary> EmpSalaryNavigation { get; set; }
        public virtual ICollection<TimeMachine> TimeMachine { get; set; }
    }
}
