using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace GDG_Project.Controllers
{
    public class HomeController : Controller
    {
        private GDGContext _GDGContext;
        private ISessionTracing _sessionTracing;

        public HomeController(GDGContext GDGContext, ISessionTracing sessionTracing)
        {
            _GDGContext = GDGContext;
            _sessionTracing = sessionTracing;
        }
        public ActionResult Index()
        {
            var model = _GDGContext.Activates.Where(x=>x.ActActive==x.Active).ToList();
            ViewBag.NewsViews = _GDGContext.News.OrderByDescending(x => x.NewsDate).Take(5).ToList();            
            return View(model);
        }
        public ActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactUs contact)
        {
            try
            {
                contact.Opend = false;
                contact.MessageDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _GDGContext.ContactUs.Add(contact);
                    _GDGContext.SaveChanges();
                    ViewBag.welcom = "done";
                    return RedirectToAction("Index");

                }
                ViewBag.welcom = "notDone";
                return View(contact);
            }
            catch(Exception e)
            {
                ViewBag.welcom = "notDone";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), 19, DateTime.Now + e.Message);
                return View(contact);
            }

            
        }
        [HttpGet]
        public ActionResult Search(string name)
        {
            try
            {
                if (name == null)
                {
                    ViewBag.welcom = "We cant find ";
                    return View();
                }
                var search = _GDGContext.PersonInfo.Where(x=>x.PName.Contains(name)&& x.PType ==x.MembersLabel);
                if (search != null && search.Count()>0)
                {
                    return View(search);
                }
                ViewBag.welcom = "We cant find"+name;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.welcom = "We cant find";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), 19, DateTime.Now + e.Message);
                return View();
            }  
        }
        [HttpGet]
        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Search");
            }

            var model = _GDGContext.PersonInfo.AsNoTracking().Where(x=>x.PId==id).Include(x=>x.Tournaments).Include(x=>x.Payment).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Search");
           
        }

        public ActionResult News(Double pageNumber =0)
        {
          var News = _GDGContext.News.OrderByDescending(x => x.NewsDate).ToList();
            Double pageZise = 3;
            Double totalItem = News.Count();
            Double NemberOfPages = Math.Ceiling(totalItem / pageZise);
            ViewBag.pageZise = pageZise;
            ViewBag.totalItem = totalItem;
            ViewBag.NemberOfPages = NemberOfPages;
            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            if(pageNumber> NemberOfPages-1)
            {
                pageNumber = NemberOfPages-1 ;
            }
            ViewBag.pageNumber = pageNumber;
            News = News.Skip(Convert.ToInt32( pageNumber * pageZise)).Take(Convert.ToInt32(pageZise)).ToList();

            return View(News);
        }
        [HttpGet]
        public ActionResult NewsDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(News));
            }
            var newsDetail = _GDGContext.News.Find(id);
            if (newsDetail == null)
            {
                return RedirectToAction(nameof(News));
            }
            newsDetail.NewsNviwer += 1;
            _GDGContext.Update(newsDetail);
            _GDGContext.SaveChanges();
            return View(newsDetail);
        }

        public ActionResult ActView(int? id)
        {
            if (id == null) {
                return RedirectToAction(nameof(Index));
            }
            var actID = _GDGContext.Activates.Where(X => X.ActId == id && X.ActActive == X.Active).FirstOrDefault();
            if (actID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Schools = _GDGContext.School.Where(x => x.SchoolAct == id && x.SchoolActive == x.Active).ToList();
            return View(actID);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string UserPassword)
       {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPassword))
            {
                ViewBag.mes = "من فضلك ادخل اسم المستخدم وكلمه المرور";
                return View();
            }
           
            if (_sessionTracing.Login(UserName,UserPassword)==true)
            {
                return new RedirectResult(_sessionTracing.RoutPage());
            } else
            {
                ViewBag.mes = "من فضلك ادخل بيانات صحيحه";
                return View();
            }
            
        }
      

        public ActionResult Logout()
        {
            _sessionTracing.Logout();
            return RedirectToAction(nameof(Login));
        }
    }
}
