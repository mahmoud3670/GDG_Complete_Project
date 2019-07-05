using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Controllers
{
    public class SchoolController : GlobalController
    {

        ISessionTracing _sessionTracing;
        public SchoolController(GDGContext GDGContext, IHostingEnvironment env, ISessionTracing sessionTracing) : base(GDGContext, env)
        {
            _sessionTracing = sessionTracing;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _sessionTracing.Authorization();
            if ((user == null) || (user.EmpPostion != user.admin ))
            {
                context.Result = new RedirectResult(_sessionTracing.RoutPage());
                
            }
        }

        // GET: School
        public ActionResult Index()
        {
           var Model = _GDGContext.School.Include(x=>x.SchoolActNavigation).Include(x=>x.Payment).ToList();
            return View(Model);
        }

        // GET: School/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var Model = _GDGContext.School.Where(x => x.SchoolId == id).Include(x=>x.SchoolActNavigation).FirstOrDefault();
            if (Model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(Model);
        }

        // GET: School/Create
        public ActionResult Create()
        {
            var Act = _GDGContext.Activates.ToList();
           List<SelectListItem> selectListItem =new List<SelectListItem>();
            foreach (var act in Act)
            {
                SelectListItem item = new SelectListItem();
                item.Value = act.ActId.ToString();
                item.Text = act.ActName;
                selectListItem.Add(item);
            }
            ViewBag.selectListItem = selectListItem;
            School school = new School();
            return View(school);
        }

        // POST: School/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School school)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    school.SchoolAct = 4;
                    _GDGContext.School.Add(school);
                    _GDGContext.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }

                return View(school);
            }
            catch(Exception e)
            {
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(school);
            }
        }

        // GET: School/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var Model = _GDGContext.School.Where(x => x.SchoolId == id).FirstOrDefault();
            if (Model == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var Act = _GDGContext.Activates.ToList();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            foreach (var act in Act)
            {
                SelectListItem item = new SelectListItem();
                item.Value = act.ActId.ToString();
                item.Text = act.ActName;
                selectListItem.Add(item);
            }
            ViewBag.selectListItem = selectListItem;

            return View(Model);
        }

        // POST: School/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, School school)
        {
            try
            {
                var oldData = _GDGContext.School.AsNoTracking().FirstOrDefault(x => x.SchoolId == id);
                if (ModelState.IsValid)
                {
                    school.SchoolMemberCount = oldData.SchoolMemberCount;
                    _GDGContext.School.Update(school);
                    _GDGContext.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }

                return View(school);
            }
            catch (Exception e)
            {
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(school);
            }
        }

        // GET: School/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        
    }
}