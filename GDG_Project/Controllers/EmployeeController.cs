using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace GDG_Project.Controllers
{
    public class EmployeeController : GlobalController
    {
        ISessionTracing _sessionTracing;
        public EmployeeController( GDGContext gDGContext, IHostingEnvironment env,ISessionTracing sessionTracing) 
            :base(gDGContext, env)   
        {
            _sessionTracing = sessionTracing;
        }
      

        // GET: Employee
        public ActionResult Index()
        {
            var user = _sessionTracing.Authorization();
            if((user!=null)&&(user.EmpPostion==user.HR|| user.EmpPostion == user.admin) )
            {
                var Model = _GDGContext.Employees.Include(x => x.EmpInfoNavigation).Include(x => x.EmpDepartmentNavigation).ToList();
                return View(Model);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            var user = _sessionTracing.Authorization();
            
            if ((user != null)&& (user.EmpPostion == user.HR || user.EmpPostion == user.admin|| user.EmpId == id))
            {
                var findID = find(id);
                if (findID == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(findID);
            }  
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // test create
        public ActionResult CreateEmp()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin)) { 
                ViewBag.selectListItem = Depart();
            EmpInfo model = new EmpInfo();
            return View(model);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        [HttpPost]
        public ActionResult CreateEmp(EmpInfo emp)
        {
            Emp(emp, out PersonInfo info, out Employees employees);
            checkedItem(emp, null);
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                using (var transaction = _GDGContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (file.Count > 0)
                        {
                            var imgName = saveImg(file, "emp");
                            info.PImg = "/img/emp/" + imgName;
                            //save data in db
                            _GDGContext.PersonInfo.Add(info);
                            _GDGContext.SaveChanges();
                            _GDGContext.Employees.Add(employees);
                            _GDGContext.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction(nameof(Details), new { id = employees.EmpId });
                        }
                        else
                        {
                            ViewBag.selectListItem = Depart();
                            return View(emp);
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ViewBag.selectListItem = Depart();
                        ViewBag.mes = e;
                        return View(emp);
                    }
                }
            }
            else
            {
                ViewBag.selectListItem = Depart();
                return View(emp);
            }
        }

        public ActionResult updateEmp(int id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin))
            {
                var emp = find(id);
            var per = _GDGContext.PersonInfo.AsNoTracking().Where(x => x.PId == emp.EmpInfo).ToList().FirstOrDefault();
            EmpInfo empInfo = new EmpInfo(emp,per);
            ViewBag.selectListItem = Depart();

            return View(empInfo);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        [HttpPost]
        public ActionResult updateEmp(int id, EmpInfo emp)
        {

            var oldEmp = find(id);
            checkedItem(emp, id);
            Emp(emp,out PersonInfo info,out Employees employees);

            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                using (var transaction = _GDGContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (file.Count > 0)
                        {
                            var imgName = saveImg(file, "emp");
                            info.PImg = "/img/emp/" + imgName;
                            //save data in db
                            _GDGContext.PersonInfo.Update(info);
                            _GDGContext.SaveChanges();
                            if (oldEmp.EmpInfoNavigation.PImg != null)
                            {
                                System.IO.File.Delete(_env.ContentRootPath + "/wwwroot" + oldEmp.EmpInfoNavigation.PImg);
                            }
                            _GDGContext.Employees.Update(employees);
                            _GDGContext.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction(nameof(Details), new { id = employees.EmpId });
                        }
                        else
                        {
                            _GDGContext.PersonInfo.Update(info);
                            _GDGContext.SaveChanges();
                            _GDGContext.Employees.Update(employees);
                            _GDGContext.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction(nameof(Details), new { id = employees.EmpId });
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ViewBag.selectListItem = Depart();
                        ViewBag.mes = e;
                        return View(emp);
                    }
                }
            }
            else
            {
                ViewBag.selectListItem = Depart();
                return View(emp);
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        //absent

        public IActionResult AbsentIndex(int id)
        {
            var user = _sessionTracing.Authorization();
            
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpId == id))
            {
                var findID = find(id);
            if (findID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var absent = _GDGContext.TimeMachine.Where(x => x.EmpId == id).ToList();
            ViewBag.EmpID = findID;
           
            return View(absent);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        public IActionResult AbsentNote(int id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin ))
            {

             var findID = find(id);

            if (findID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            TimeMachine absentnote = new TimeMachine();
            ViewBag.EmpID = findID;
            return View(absentnote);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        [HttpPost]
        public IActionResult AbsentNote(int id,TimeMachine timeMachine)
        {
            try
            {

                var findID = find(id);

                if (findID == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                timeMachine.EmpId = id;
                //limit days
                var timeDateConvert = int.Parse(timeMachine.TimeDate.ToString("yyyyMMdd")) - int.Parse(DateTime.Today.ToString("yyyyMMdd"));
                if (timeDateConvert > 10 || timeDateConvert < -5)
                {
                    ViewBag.mes = " تم انتهاء الفتره المحدده للتعديل علي البيان";
                    ViewBag.EmpID = find(id);
                    return View(timeMachine);
                }
                if (timeMachine.NoteCase == timeMachine.Regular) {
                    if (_GDGContext.TimeMachine.Where(x => x.EmpId == id &&x.TimeDate.Year==DateTime.Now.Year&& x.NoteCase == x.Regular).Count() >= 9)
                    {

                        ViewBag.mes = " تم انتهاء الاجازات الاعتيادي ";
                        ViewBag.EmpID = find(id);
                        return View(timeMachine);
                    }
                }else if(timeMachine.NoteCase == timeMachine.furlough)
                {
                    if (_GDGContext.TimeMachine.Where(x => x.EmpId == id && x.TimeDate.Year == DateTime.Now.Year && x.NoteCase == x.furlough).Count() >= 2)
                    {

                        ViewBag.mes = " تم انتهاء الاذونات ";
                        ViewBag.EmpID = find(id);
                        return View(timeMachine);
                    }
                }else if(timeMachine.NoteCase == timeMachine.Emergency)
                {
                    if (_GDGContext.TimeMachine.Where(x => x.EmpId == id && x.TimeDate.Year == DateTime.Now.Year && x.NoteCase == x.Emergency).Count() >= 8)
                    {
                        ViewBag.mes = " تم انتهاء الاجازات العارضه ";
                        ViewBag.EmpID = find(id);
                        return View(timeMachine);
                    }
                }
                if (ModelState.IsValid)
                {

                    var FindTime = _GDGContext.TimeMachine.Where(x => x.TimeDate.ToString("dd/MM/yyyy") == timeMachine.TimeDate.ToString("dd/MM/yyyy") && x.EmpId == id).FirstOrDefault();
                    if (FindTime == null)
                    {
                        _GDGContext.TimeMachine.Add(timeMachine);
                        _GDGContext.SaveChanges();
                        return RedirectToAction(nameof(AbsentIndex), new { id = timeMachine.EmpId });
                    }
                    else
                    {
                        FindTime.NoteCase = timeMachine.NoteCase;
                        _GDGContext.TimeMachine.Update(FindTime);
                        _GDGContext.SaveChanges();
                        return RedirectToAction(nameof(AbsentIndex), new { id = timeMachine.EmpId });
                    }
                }
                ViewBag.EmpID = find(id);
                return View(timeMachine);
            }
            catch (Exception e)
            {
                ViewBag.ex = e;
                ViewBag.EmpID = find(id);
                return View(timeMachine);
            }
            
        }
        public IActionResult EmpSalaryIndex()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin||user.EmpPostion==user.accounting))
            {
                var SalaryList = _GDGContext.EmpSalary.Include(x => x.Emp.EmpInfoNavigation);
                return View(SalaryList);
            }
            return new RedirectResult(_sessionTracing.RoutPage());

        }

        public IActionResult SalaryView(int? id)

        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.accounting))
            {
                if (id == null)
                {
                    return RedirectToAction(nameof(EmpSalaryIndex));
                }
                var salaryview = _GDGContext.EmpSalary.Find(id);
                if (salaryview == null)
                {
                    return RedirectToAction(nameof(EmpSalaryIndex));
                }
                var absentDate = salaryview.SalaryDate.AddMonths(-1).ToString("yyyyMM");
                var absentSearch = _GDGContext.TimeMachine.Where(x => x.EmpId == salaryview.EmpId && x.TimeDate.ToString("yyyyMM") == absentDate).ToList();
                ViewBag.absentCount = absentSearch.Where(x => x.NoteCase != x.Absence).Count();
                ViewBag.absent = absentSearch;
                ViewBag.empinfo = find(salaryview.EmpId);


                return View(salaryview);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        public Employees find(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var findID =_GDGContext.Employees.AsNoTracking().Where(x=>x.EmpId==id).Include(x => x.EmpInfoNavigation).ToList().FirstOrDefault();
            if (findID == null)
            {
                return null;
            }
            return findID;
        }

        private EmpInfo checkedItem(EmpInfo emp, int? id)
        {
            if (id == null)
            {
                //create Method
                // check password
                if (string.IsNullOrEmpty(emp.EmpPassword))
                {
                    ModelState.AddModelError("EmpPassword", "من فضلك ادخل رمز سري ");
                    ModelState.AddModelError("conPassword", "من فضلك ادخل رمز سري ");
                }
                // check img
                if (string.IsNullOrEmpty(emp.PImg))
                {
                    ModelState.AddModelError("PImg", "من فضلك اختر صوره  ");
                   
                }


                // check user name 
                if (!string.IsNullOrEmpty(emp.EmpUserName))
                {
                        int IsUserNameExist = _GDGContext.Employees.AsNoTracking().Where
                                        (x => x.EmpUserName == emp.EmpUserName).Count();
                        if (IsUserNameExist >= 1)
                        {
                            ModelState.AddModelError("EmpUserName", "هذاالاسم موجود مسبقا");
                            
                        }
                }
                else
                {
                    ModelState.AddModelError("EmpUserName", "*");
                }

                // check national id
                if (!string.IsNullOrEmpty(emp.PNationalId))
                {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PNationalId == emp.PNationalId).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PNationalId", "هذاالرقم موجود مسبقا ");

                        }

                }
                else
                {
                    ModelState.AddModelError("PNationalId", "* ");
                }
                // checked email
                if (!string.IsNullOrEmpty(emp.PEmail))
                {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PEmail == emp.PEmail).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PEmail", "هذاالايميل موجود مسبقا  ");

                        }
                }
                else
                {
                    ModelState.AddModelError("PEmail", "*");
                }
                // checked phone
                if (!string.IsNullOrEmpty(emp.PPhone))
                {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PPhone == emp.PPhone).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PPhone", "هذاالرقم موجود مسبقا  ");

                        }

                }
                else
                {
                    ModelState.AddModelError("PPhone", "*  ");
                }
                return emp;
            }else
            {
                // update Metode

                var oldEmp = find(id);
                // check user name 
                if (!string.IsNullOrEmpty(emp.EmpUserName))
                {
                    if (oldEmp.EmpUserName != emp.EmpUserName)
                    {
                        int IsUserNameExist = _GDGContext.Employees.AsNoTracking().Where
                                        (x => x.EmpUserName == emp.EmpUserName).Count();
                        if (IsUserNameExist >= 1)
                        {
                            ModelState.AddModelError("EmpUserName", "هذاالاسم موجود مسبقا");
                           
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("EmpUserName", "*");
                }
                // check national id
                if (!string.IsNullOrEmpty(emp.PNationalId))
                {
                    if (oldEmp.EmpInfoNavigation.PNationalId != emp.PNationalId)
                    {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PNationalId == emp.PNationalId).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PNationalId", "هذاالرقم موجود مسبقا ");
                           
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("PNationalId", "* ");
                }
                // checked email
                if (!string.IsNullOrEmpty(emp.PEmail))
                {
                    if (oldEmp.EmpInfoNavigation.PEmail != emp.PEmail)
                    {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PEmail == emp.PEmail).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PEmail", "هذاالايميل موجود مسبقا  ");

                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("PEmail", "*");
                }
                // checked phone
                if (!string.IsNullOrEmpty(emp.PPhone))
                {
                    if (oldEmp.EmpInfoNavigation.PPhone != emp.PPhone)
                    {
                        int IsUserExist = _GDGContext.PersonInfo.AsNoTracking().Where
                                        (x => x.PPhone == emp.PPhone).Count();
                        if (IsUserExist >= 1)
                        {
                            ModelState.AddModelError("PPhone", "هذاالرقم موجود مسبقا  ");

                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("PPhone", "*  ");
                }

                //check password
                if (string.IsNullOrEmpty(emp.EmpPassword))
                {
                    ModelState.Remove("EmpPassword");
                    ModelState.Remove("conPassword");
                    emp.EmpPassword = oldEmp.EmpPassword;
                    emp.conPassword = oldEmp.EmpPassword;
                }
                // check image
                if (string.IsNullOrEmpty(emp.PImg))
                {
                    ModelState.Remove("PImg");
                    emp.PImg = oldEmp.EmpInfoNavigation.PImg;
                }
                return emp ;
            }
        }

        public List<SelectListItem> Depart()
        {
            var Dept = _GDGContext.Department.ToList();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (var dept in Dept)
            {
                SelectListItem item = new SelectListItem();
                item.Value = dept.DepId.ToString();
                item.Text = dept.DepName;
                selectListItem.Add(item);
            }
            return selectListItem;
        }

        public void Emp(EmpInfo emp,out PersonInfo info,out Employees employee)
        {
            var p = new PersonInfo();
            p.PId = emp.PId;
            p.PName = emp.PName;
            p.PBirthDate = emp.PBirthDate;
            p.PNationalId = emp.PNationalId;
            p.PPhone = emp.PPhone;
            p.PImg = emp.PImg;
            p.PEmail = emp.PEmail;
            p.PType = p.EmployeesLabel;
            p.PGender = emp.PGender;
            p.PAdress = emp.PAdress;
            p.PStartDate = emp.PStartDate;
            var e = new Employees();
            e.EmpId = emp.EmpId;
            e.EmpInfo = p.PId;
            e.EmpDepartment = emp.EmpDepartment;
            e.EmpSalary = emp.EmpSalary;
            e.EmpUserName = emp.EmpUserName;
            e.EmpPassword = emp.EmpPassword;
            e.conPassword = emp.conPassword;
            e.EmpActive = emp.EmpActive;
            e.EmpPostion = emp.EmpPostion;

            info = p;
            employee = e;
        }
       


    }
}