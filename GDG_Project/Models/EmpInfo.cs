using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GDG_Project.Models
{
    public class EmpInfo : PersonInfo

    {
        public EmpInfo()
        {

        }
        //public string male = "m";
        //public string famel = "f";
        ////person info
        //[Key]
        //public int PId { get; set; }
        //[Required(ErrorMessage = "*")]
        //[RegularExpression("^[\u0621-\u064A\u0660-\u0669 ]+$", ErrorMessage = "ادخل الاسم صحيح")]
        //[MaxLength(40, ErrorMessage = "اقصي عدد للحروف 40")]
        //[MinLength(5, ErrorMessage = "اقل عدد للحروف 5")]
        //public string PName { get; set; }
        //[Required(ErrorMessage = "*")]
        //[DataType(DataType.Date,ErrorMessage ="ادخل تاريخ صحيح")]
        //public DateTime PBirthDate { get; set; }
        //[Required(ErrorMessage = "*")]
        //[RegularExpression("(2|3)[0-9][0-9](01|02|03|04|05|06|07|08|09|10|11|12)[0-3][0-9](01|02|03|04|11|12|13|14|15|16|17|18|19|21|22|23|24|25|26|27|28|29|31|32|33|34|35|88)[0-9]{5}", ErrorMessage = "ادخل رقم قومي صحيح")]
        //public string PNationalId { get; set; }
        //[Required(ErrorMessage = "*")]
        //[RegularExpression("(^(010|012|011|015))[0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صحيح")]
        //public string PPhone { get; set; }
        ////[Required(ErrorMessage = "*")]
        //public string PImg { get; set; }

        //[EmailAddress]
        //public string PEmail { get; set; }

        //public string PType { get; set; }
        //[Required(ErrorMessage = "*")]
        //public string PGender { get; set; }
        //[Required(ErrorMessage = "*")]
        //public string PAdress { get; set; }
        //[Required(ErrorMessage = "*")]
        //[DataType(DataType.Date, ErrorMessage = "ادخل تاريخ صحيح")]
        //public DateTime? PStartDate { get; set; }

        // emp info

        public short admin = 1;
        public short accounting = 2;
        public short worker = 3;
        public short HR = 4;
        public short dataEntry = 5;

        public bool active = true;
        public bool NotActive = false;
        public int EmpId { get; set; }
        public int Empinfo { get; set; }
        [Required(ErrorMessage = "*")]
        public short EmpDepartment { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal EmpSalary { get; set; }


        [Required(ErrorMessage = "*")]
        [MaxLength(40, ErrorMessage = "اقصي عدد للحروف 40")]
        [MinLength(5, ErrorMessage = "اقل عدد للحروف 5")]
        [RegularExpression("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage ="ادخل الاسم صحيح ")]
        public string EmpUserName { get; set; }


        //[Required(ErrorMessage = "*")]
        [MinLength(8, ErrorMessage = "يجب ان لا تقل عن 8")]
        [MaxLength(20, ErrorMessage = "يجب ان لا تزيد عن 20")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?:(?=.*?[A-Z])(?:(?=.*?[0-9])(?=.*?[-!@#$%^&*()_[\\]{},.<>+=])|(?=.*?[a-z])(?:(?=.*?[0-9])|(?=.*?[-!@#$%^&*()_[\\]{},.<>+=])))|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[-!@#$%^&*()_[\\]{},.<>+=]))[A-Za-z0-9!@#$%^&*()_[\\]{},.<>+=-]{7,50}$"
            ,ErrorMessage ="ادخل رمز سري يحتوي علي A-Z,a-z,0-9,@")]
        public string EmpPassword { get; set; }



       // [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Compare("EmpPassword",ErrorMessage ="ادخل الرمز مره اخري")]
        [NotMapped]
        public string conPassword { get; set; }

        [Required(ErrorMessage ="*")]
        public bool? EmpActive { get; set; }

        [Required(ErrorMessage = "*")]
        public short? EmpPostion { get; set; }







        public EmpInfo(Employees employees, PersonInfo personInfo)
        {
           // person info
            PId = personInfo.PId;
            PName = personInfo.PName;
            PBirthDate = personInfo.PBirthDate;
            PNationalId = personInfo.PNationalId;
            PPhone = personInfo.PPhone;
            PImg = personInfo.PImg;
            PEmail = personInfo.PEmail;
            PType = personInfo.EmployeesLabel;
            PGender = personInfo.PGender;
            PAdress = personInfo.PAdress;
            PStartDate = personInfo.PStartDate;

           // emp info



            EmpId = employees.EmpId;
            Empinfo = PId;
            EmpDepartment = employees.EmpDepartment;
            EmpSalary = employees.EmpSalary;
            EmpUserName = employees.EmpUserName;
            EmpPassword = employees.EmpPassword;
            conPassword = employees.conPassword;
            EmpActive = employees.EmpActive;
            EmpPostion = employees.EmpPostion;



        }


    }
}
