using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Controllers
{
    public class DepartmentController : GlobalController
    {
        ISessionTracing _sessionTracing;
        public DepartmentController(GDGContext gDGContext, IHostingEnvironment env, ISessionTracing sessionTracing)
            : base(gDGContext, env)
        {
            _sessionTracing = sessionTracing;
        }
        public IActionResult Index()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {

                var model = _GDGContext.Department.ToList();
                return View(model);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {
                return View();
            }
            return new RedirectResult(_sessionTracing.RoutPage());
       
    }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid) { 
                    _GDGContext.Department.Add(department);
                _GDGContext.SaveChanges();
                return RedirectToAction(nameof(Index));
                }else
                {
                    return View(department);
                }
            }
            catch (Exception e)
            {
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(department);
            }
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.admin))
            {
                if (id == null)
            {
                return RedirectToAction("Index");
            }
            var department = _GDGContext.Department.AsNoTracking().FirstOrDefault(x => x.DepId == id);
            if (department == null)
            {
                return RedirectToAction("Index");
            }
            return View(department);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Department department)
        {
            if (id == null || id != department.DepId)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _GDGContext.Department.Update(department);
                    _GDGContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(department);
                }
            }
            catch (Exception e)
            {
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(department);
            }
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

      
    }
}