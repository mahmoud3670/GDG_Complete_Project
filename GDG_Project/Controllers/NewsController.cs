using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace GDG_Project.Controllers
{
    public class NewsController : GlobalController
    {
        ISessionTracing _sessionTracing;
        public NewsController(GDGContext GDGContext ,IHostingEnvironment env, ISessionTracing sessionTracing) : base(GDGContext, env)
        {
            _sessionTracing = sessionTracing;
        }
        public ActionResult Index()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin|| user.EmpPostion == user.dataEntry))
            {

                var model = _GDGContext.News.ToList();
                return View(model);
        }
            return new RedirectResult(_sessionTracing.RoutPage());
    }
      

       [HttpGet]
        public ActionResult Details(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {

                if (id == null)
            {
                return RedirectToAction("Index");
            }
            var newsID = _GDGContext.News.Find(id);
            if (newsID == null)
            {
                return RedirectToAction("Index");
            } 
            return View(newsID);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

      
        public ActionResult Create()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                return View();
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var file = HttpContext.Request.Form.Files;
                    if (file.Count > 0)
                    {
                        //GlobalController globalController = new GlobalController(_GDGContext, _env);
                        var imgName =saveImg(file, "News");
                        //save data in db
                        _GDGContext.News.Add(news);
                        news.NewsDate = DateTime.Now;
                        news.NewsNviwer = 0;
                        news.NewsImg = "/img/News/" + imgName;
                        _GDGContext.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(news);
                }


                return View(news);
            }
            catch (Exception e)
            { ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(news);
            }
        
        }
     
        public ActionResult Edit(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {

                if (id == null)
            {
                return RedirectToAction("Index");
            }
            var newsID = _GDGContext.News.AsNoTracking().FirstOrDefault(x => x.NewsId == id);
            if (newsID == null)
            {
                return RedirectToAction("Index");
            }
            return View(newsID);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,News news)
        {   
            if (id == null||id!=news.NewsId)
            {
                return RedirectToAction("Index");
            }

            var oldData = _GDGContext.News.AsNoTracking().FirstOrDefault(x=>x.NewsId==id);

            try {
                if (ModelState.IsValid)
                {
                    var file = HttpContext.Request.Form.Files;
                    if (file.Count > 0)
                    {
                       // GlobalController globalController = new GlobalController(_GDGContext, _env);
                        var imgName = saveImg(file, "News");
                        news.NewsDate = oldData.NewsDate;
                        news.NewsNviwer = oldData.NewsNviwer;
                        news.NewsImg = "/img/News/" + imgName;
                        _GDGContext.News.Update(news);
                        _GDGContext.SaveChanges();
                       
                        
                        if (oldData.NewsImg != null)
                        {
                            System.IO.File.Delete(_env.ContentRootPath + "/wwwroot" + oldData.NewsImg);
                        }

                        return RedirectToAction("Index");
                    }
                    news.NewsDate = oldData.NewsDate;
                    news.NewsImg = oldData.NewsImg;
                    news.NewsNviwer = oldData.NewsNviwer;
                    _GDGContext.Update(news);
                    _GDGContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                

                return View(news);
            }
            catch(Exception e)
            {
                 ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(news);
            }
        }
            
        public ActionResult Delete(int id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                var newsID = _GDGContext.News.Find(id);
            _GDGContext.News.Remove(newsID);
            _GDGContext.SaveChanges();
            System.IO.File.Delete(_env.ContentRootPath + "/wwwroot"+newsID.NewsImg);
            return RedirectToAction("Index");
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        public ActionResult ContactUs()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin ))
            {
                var contact = _GDGContext.ContactUs.OrderByDescending(x => x.MessageDate).ToList();
           return View(contact);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        public ActionResult ReadContactUs(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {
                if (id == null)
            {
                return RedirectToAction(nameof(ContactUs));
            }
            var contactID = _GDGContext.ContactUs.Find(id);
            if (contactID == null)
            {
                return RedirectToAction(nameof(ContactUs));
            }
             contactID.Opend = true;
            _GDGContext.ContactUs.Update(contactID);
            _GDGContext.SaveChanges();
            return View(contactID);
            }
            return new RedirectResult(_sessionTracing.RoutPage());

        }
        public ActionResult DeleteContactUs(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {

                if (id == null)
            {
                return RedirectToAction(nameof(ContactUs));
            }
            var contactID = _GDGContext.ContactUs.Find(id);
            if (contactID == null)
            {
                return RedirectToAction(nameof(ContactUs));
            }
            _GDGContext.ContactUs.Remove(contactID);
            _GDGContext.SaveChanges();
            return RedirectToAction(nameof(ContactUs));
        }
            return new RedirectResult(_sessionTracing.RoutPage());
    }



     public ActionResult LogIndex()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {

                var Model=_GDGContext.LogEvent.Include(x=>x.EventActorNavigation).ToList();
                return View(Model);

        }
            return new RedirectResult(_sessionTracing.RoutPage());
    }
        public ActionResult DetailsLog(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {
                if (id == null)
                {
                    return Redirect(nameof(LogIndex));
                }
                var Model = _GDGContext.LogEvent.Where(x=>x.LogId==id).Include(x=>x.EventActorNavigation).FirstOrDefault();
                if (Model == null)
                {
                    return Redirect(nameof(LogIndex));
                }
                return View(Model);

            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }
        public ActionResult DeleteLog(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {
                if (id == null)
                {
                    return Redirect(nameof(LogIndex));
                }
                var Model = _GDGContext.LogEvent.Where(x => x.LogId == id).FirstOrDefault();
                if (Model == null)
                {
                    return Redirect(nameof(LogIndex));
                }
                _GDGContext.LogEvent.Remove(Model);
                _GDGContext.SaveChanges();
                return Redirect(nameof(LogIndex));

            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }



    }


}