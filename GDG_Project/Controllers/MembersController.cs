using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GDG_Project.Models;
using GDG_Project.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GDG_Project.Controllers
{
    public class MembersController : Controller
    {
        private  GDGContext _gDGContext;
        private ISessionTracing _sessionTracing;

        public MembersController(GDGContext gDGContext, ISessionTracing sessionTracing)
        {
            _gDGContext = gDGContext;
            _sessionTracing = sessionTracing;

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _sessionTracing.Authorization();
            if ((user == null) || (user.EmpPostion != user.admin||user.EmpPostion == user.dataEntry))
            {
                context.Result = new RedirectResult(_sessionTracing.RoutPage());

            }
        }

        // GET: Members
        public ActionResult Index()
        {
            var Model = _gDGContext.PersonInfo.Where(x=>x.PType==x.MembersLabel).Include(x=>x.Payment).ToList();
           //var Model= _gDGContext.Payment.Include(x => x.School).Include(x=>x.MemberInfoNavigation).GroupBy(x => x.MemberInfo).ToList();
            return View(Model);
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect(nameof(Index));
            }

            var members = _gDGContext.PersonInfo.Where(x => x.PId == id).Include(x => x.Payment).FirstOrDefault();
            if (members == null)
            {
                return Redirect(nameof(Index));
            }
             // ViewBag.paymentView = _gDGContext.Payment.Where(x => x.MemberInfo == id).Include(x => x.School).ToList();
            ViewBag.paymentView = (from p in _gDGContext.Payment
                 join e in _gDGContext.School
                 on p.SchoolId equals e.SchoolId
                 where p.MemberInfo == id
                 select e ).ToList();
         
            return View(members);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x=>x.PType==x.MembersLabel), "PId", "PName");
            ViewData["SchoolId"] = new SelectList(_gDGContext.School.Where(x=>x.SchoolActive==x.Active), "SchoolId", "SchoolName");
            ViewData["TrainerCode"] = new SelectList(_gDGContext.Trainer.Include(x=>x.TrainerInfoNavigation), "TrainerCode", "TrainerInfoNavigation.PName");
            Payment Model = new Payment();
            return View(Model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Payment payment)
        {
            if (payment.PayDate != null&&payment.PayType!=null)
            {
                if (payment.PayType == payment.Monthly)
                {
                    payment.PayDueDate = payment.PayDate.AddMonths(1);
                }else if (payment.PayType == payment.quarter)
                {
                    payment.PayDueDate = payment.PayDate.AddMonths(3);
                }
                else if(payment.PayType == payment.Half)
                {
                    payment.PayDueDate = payment.PayDate.AddMonths(6);
                }
                else if (payment.PayType == payment.PerYear)
                {
                    payment.PayDueDate = payment.PayDate.AddYears(1);
                }
                else
                {
                    ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.MembersLabel), "PId", "PName");
                    ViewData["SchoolId"] = new SelectList(_gDGContext.School, "SchoolId", "SchoolName");
                    ViewData["TrainerCode"] = new SelectList(_gDGContext.Trainer.Include(x => x.TrainerInfoNavigation), "TrainerCode", "TrainerInfoNavigation.PName");
                    return View(payment);
                } 



            }
            var DueDate = _gDGContext.Payment.Where(x => x.MemberInfo == payment.MemberInfo && x.SchoolId == payment.SchoolId).Select(x => x.PayDueDate).LastOrDefault();
            if (DueDate != null && DueDate > DateTime.Now)
            {
                ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.MembersLabel), "PId", "PName");
                ViewData["SchoolId"] = new SelectList(_gDGContext.School, "SchoolId", "SchoolName");
                ViewData["TrainerCode"] = new SelectList(_gDGContext.Trainer.Include(x => x.TrainerInfoNavigation), "TrainerCode", "TrainerInfoNavigation.PName");
                ViewBag.mes = "sorry we cant add now wait    "+DueDate;
                return View(payment);
            }
            ModelState.Remove("PayDueDate");
            if (ModelState.IsValid)
            {
                try
                {
                    _gDGContext.Payment.Add(payment);
                    _gDGContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.MembersLabel), "PId", "PName");
                    ViewData["SchoolId"] = new SelectList(_gDGContext.School, "SchoolId", "SchoolName");
                    ViewData["TrainerCode"] = new SelectList(_gDGContext.Trainer.Include(x => x.TrainerInfoNavigation), "TrainerCode", "TrainerInfoNavigation.PName");
                    ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);

                    return View(payment);


                } 
            }
            ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.MembersLabel), "PId", "PName");
            ViewData["SchoolId"] = new SelectList(_gDGContext.School, "SchoolId", "SchoolName");
            ViewData["TrainerCode"] = new SelectList(_gDGContext.Trainer.Include(x => x.TrainerInfoNavigation), "TrainerCode", "TrainerInfoNavigation.PName");
            return View(payment);
        }

        

       public ActionResult ViewPay(int? MemberId,int? SchoolId)
        {
            if (MemberId == null || SchoolId == null)
            {
                return Redirect(nameof(Index));
            }
            var Model = _gDGContext.Payment.AsNoTracking()
                .Where(x => x.MemberInfo == MemberId && x.SchoolId == SchoolId).ToList();
            
            if (Model == null)
            {
                return Redirect(nameof(Index));
            }
            ViewBag.SchoolName = _gDGContext.School.AsNoTracking().Where(x => x.SchoolId == SchoolId).FirstOrDefault();
            return View(Model);
        }



    }
}
