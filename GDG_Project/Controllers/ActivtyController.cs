using System;
using System.IO;
using System.Linq;
using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Controllers
{
    public class ActivtyController : GlobalController
    {
        ISessionTracing _sessionTracing;
        public ActivtyController(GDGContext gDGContext, IHostingEnvironment env, ISessionTracing sessionTracing)
            : base(gDGContext, env)
        {
            _sessionTracing = sessionTracing;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _sessionTracing.Authorization();
            if ((user == null) || (user.EmpPostion != user.admin))
            {
                context.Result = new RedirectResult(_sessionTracing.RoutPage());

            }
        }

        // GET: Activty
        public ActionResult Index()
        {
           
                var activty = _GDGContext.Activates.ToList();
                return View(activty);
           
        }

        // GET: Activty/Details/5
        public ActionResult Details(int id)
        {
           
                var findID=find(id);
                if (findID == null)
                {
                    return RedirectToAction("Index");
                }
                return View(findID);
           
        }

        // GET: Activty/Create
        public ActionResult Create()
        {
            
                return View();
            
        }

        // POST: Activty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Activates activates)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var file = HttpContext.Request.Form.Files;
                    if (file.Count > 0)
                    {
                        //uplode img
                        //GlobalController globalController = new GlobalController(_GDGContext, _env);
                        var imgName = saveImg(file, "ActImg");
                        //save data in db
                        _GDGContext.Activates.Add(activates);
                        activates.ActImg = "/img/ActImg/" + imgName;
                        _GDGContext.SaveChanges();

                        return RedirectToAction("Index");
                    }

                    return View(activates);
                }


                return View(activates);
            }
            catch (Exception e)
            {
                ViewBag.ex = e;
                return View(activates);
            }
        }

        // GET: Activty/Edit/5
        public ActionResult Edit(int id)
        {
           
                var findID = find(id);
                if (findID == null)
                {
                    return RedirectToAction("Index");
                }
                return View(findID);
            
        }

        // POST: Activty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Activates activates)
        {

            if (id == null || id != activates.ActId)
            {
                return RedirectToAction("Index");
            }

            var oldData = find(id);

            try
            {
                if (ModelState.IsValid)
                {
                    var file = HttpContext.Request.Form.Files;
                    if (file.Count > 0)
                    {
                       // GlobalController globalController = new GlobalController(_GDGContext, _env);
                        var imgName =  saveImg(file, "ActImg");
                        activates.ActImg = "/img/ActImg/" + imgName;
                        _GDGContext.Activates.Update(activates);
                        _GDGContext.SaveChanges();
                        if (oldData.ActImg != null)
                        {
                            System.IO.File.Delete(_env.ContentRootPath + "/wwwroot" + oldData.ActImg);
                        }

                        return RedirectToAction("Index");
                    }
                    
                    activates.ActImg = oldData.ActImg;
                    _GDGContext.Update(activates);
                    _GDGContext.SaveChanges();
                    return RedirectToAction("Index");
                }


                return View(activates);
            }
            catch (Exception e)
            {
                ViewBag.ex = e;
                return View(activates);
            }
        }

        public Activates find(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var activtyID = _GDGContext.Activates.AsNoTracking().FirstOrDefault(x => x.ActId == id);
            if (activtyID == null)
            {
                return null;
            }

            return activtyID;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            //if (id == null)
            //{
                return RedirectToAction(nameof(Index));
            //}
            //var activtyID = _GDGContext.Activates.Find(id);
            //if (activtyID == null)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //_GDGContext.Activates.Remove(activtyID);
            //_GDGContext.SaveChanges();
            //return RedirectToAction(nameof(Index));

        }
    }
}