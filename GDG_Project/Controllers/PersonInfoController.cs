using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GDG_Project.Common;
using GDG_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GDG_Project.Controllers
{
    public class PersonInfoController : GlobalController
    {

       private ISessionTracing _sessionTracing;
        public PersonInfoController(GDGContext gDGContext, IHostingEnvironment env, ISessionTracing sessionTracing)
            : base(gDGContext, env)
        {
            _sessionTracing = sessionTracing;
        }



        // GET: PersonInfo
        public ActionResult Index()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                var person = _GDGContext.PersonInfo.Where(x=>x.PType==x.TrainerLabel||x.PType==x.MembersLabel).ToList();
                return View(person);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // GET: PersonInfo/Details/5
        public ActionResult Details(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                if (id == null)
            {
                return RedirectToAction("Index");
            }
            var personID = _GDGContext.PersonInfo.Find(id);
            if (personID == null||personID.PType==personID.EmployeesLabel)
            {
                return RedirectToAction("Index");
            }
            return View(personID);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // GET: PersonInfo/Create
        public ActionResult Create()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                PersonInfo person = new PersonInfo();
            return View(person);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // POST: PersonInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonInfo person)
        {
            var user = _sessionTracing.Authorization();
            if (person.PType == person.TrainerLabel&&(user.EmpPostion==user.dataEntry))
            {
                ModelState.AddModelError(person.PType, "ليس لديك الصلاحيه للمدرب");
                return View(person);
            }
            
            try
            {
                if (ModelState.IsValid)
                {
                    if ((person.PType==person.TrainerLabel|| person.PType == person.MembersLabel)&&(person.PGender==person.male||person.PGender==person.famel)) {
                        var file = HttpContext.Request.Form.Files;
                        if (file.Count > 0)
                        {
                          //  GlobalController globalController = new GlobalController(_GDGContext,_env);
                            var imgName =saveImg(file, "person");
                            //save data in db
                            _GDGContext.PersonInfo.Add(person);
                            person.PImg = "/img/person/" + imgName;
                            _GDGContext.SaveChanges();
                            if (person.PType == person.EmployeesLabel)
                            {
                                return RedirectToAction(nameof(Create), "Employee", new { id = person.PId });
                            }
                            return RedirectToAction(nameof(Details), new { id = person.PId });
                            
                        }

                        return View(person);
                    }


                    return View(person);
                }
                return View(person);
            }
            catch (Exception e)
            {
                ViewBag.ex = e;
                return View(person);
            }

        }

        // GET: PersonInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                if (id == null)
            {
                return RedirectToAction("Index");
            }
            var personID = _GDGContext.PersonInfo.Find(id);
            if (personID == null)
            {
                return RedirectToAction("Index");
            }
            return View(personID);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // POST: PersonInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int?id, PersonInfo person)
        {
            var user = _sessionTracing.Authorization();
            if (person.PType == person.TrainerLabel && (user.EmpPostion == user.dataEntry))
            {
                ModelState.AddModelError(person.PType, "ليس لديك الصلاحيه للمدرب");
                return View(person);
            }
            if (id == null || id != person.PId)
            {
                return RedirectToAction("Index");
            }
            var oldData = _GDGContext.PersonInfo.AsNoTracking().Where(x => x.PId == id).First();
           
                person.PType = oldData.PType;
            if ((person.PType == person.TrainerLabel || person.PType == person.MembersLabel )
                && (person.PGender == person.male || person.PGender == person.famel))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var file = HttpContext.Request.Form.Files;
                        if (file.Count > 0)
                        {
                           // GlobalController globalController = new GlobalController(_GDGContext, _env);
                            var imgName = saveImg(file, "person");

                            person.PImg = "/img/person/" + imgName;
                            
                            _GDGContext.PersonInfo.Update(person);
                            _GDGContext.SaveChanges();
                            if (oldData.PImg != null)
                            {
                                System.IO.File.Delete(_env.ContentRootPath + "/wwwroot" + oldData.PImg);
                            }

                            return RedirectToAction("Details", new { id = person.PId });
                        }

                        person.PImg = oldData.PImg;
                       _GDGContext.Update(person);
                        _GDGContext.SaveChanges();
                      //  return RedirectToAction(nameof(Details),nameof(EmployeeController), new { id = person.PId });
                        return RedirectToAction("Details", new { id = person.PId });
                    }


                    return View(person);
                }
                catch (Exception e)
                {
                    ViewBag.ex = e;
                    return View(person);
                }
            }
            return View(person);
        }

        // GET: PersonInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }

       



    }
}