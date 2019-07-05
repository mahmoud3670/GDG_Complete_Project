using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GDG_Project.Models;
using GDG_Project.Common;
using Microsoft.AspNetCore.Hosting;

namespace GDG_Project.Controllers
{
    public class TrainersController : Controller
    {
        ISessionTracing _sessionTracing;
        GDGContext _gDGContext;
        IHostingEnvironment _env;
        public TrainersController(GDGContext gDGContext, IHostingEnvironment env, ISessionTracing sessionTracing) 
        {
            _sessionTracing = sessionTracing;
            _gDGContext = gDGContext;
            _env = env;
        }

        // GET: Trainers
        public ActionResult Index()
        {
            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                var Trainers = _gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel).Include(x => x.Trainer).ToList();

                return View(Trainers);
            }
            return new RedirectResult(_sessionTracing.RoutPage());
        }

        // GET: Trainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect(nameof(Index));
            }
            var trainer = _gDGContext.PersonInfo.AsNoTracking().Where(x => x.PId == id).Include(x=>x.Trainer).FirstOrDefault();
            if (trainer == null)
            {
                return Redirect(nameof(Index));
            }
            ViewBag.trainerAct = _gDGContext.Trainer.Where(x=>x.TrainerInfo==id)
                .Include(t => t.TrainerActNavigation).ToList();

            

                return View(trainer);
        }

        // GET: Trainers/Create
        public ActionResult Create()
        {

            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName");
                ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x=>x.PType==x.TrainerLabel), "PId", "PName");
                Trainer Model = new Trainer();
                return View(Model);
            }
            return new RedirectResult(_sessionTracing.RoutPage());

        }

        // POST: Trainers/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Trainer trainer)
        {
            if (ModelState.IsValid == true)
            {
                var CheckTrainerActive = _gDGContext.Trainer.AsNoTracking().Where(x => x.TrainerInfo == trainer.TrainerInfo && x.TrainerAct == trainer.TrainerAct);
            if (CheckTrainerActive.Count() >= 1)
            {
                ModelState.AddModelError("TrainerInfo", "هذا المدرب موجود مسبقا مع هذا النشاط ");
                ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", trainer.TrainerAct);
                ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName", trainer.TrainerInfo);
                return View(trainer);
            }
            try
            {
                _gDGContext.Trainer.Add(trainer);
                _gDGContext.SaveChanges();
                return Redirect(nameof(Index));
            }
            catch(Exception e)
            {
                ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", trainer.TrainerAct);
                ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName", trainer.TrainerInfo);
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(trainer);
            }
        }

        ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", trainer.TrainerAct);
        ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName", trainer.TrainerInfo);
            return View(trainer);

        }

        // GET: Trainers/Edit/5
        public ActionResult Edit(int? id)
        {

            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {

                if (id == null)
                {
                    return Redirect(nameof(Index));
                }
                ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName");
                ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName");
                Trainer Model =_gDGContext.Trainer.Find(id);
                if (Model == null)
                {
                    return Redirect(nameof(Index));
                }
                return View(Model);
            }
            return new RedirectResult(_sessionTracing.RoutPage());

           
        }

        // POST: Trainers/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int? id, [Bind("TrainerCode,TrainerInfo,TrainerAct,TrainerActive,TrainerStartDay")] Trainer trainer)
        {

            var user = _sessionTracing.Authorization();
            if ((user != null) && (user.EmpPostion == user.HR || user.EmpPostion == user.admin || user.EmpPostion == user.dataEntry))
            {
                if (id == null || id != trainer.TrainerCode)
                {
                    return Redirect(nameof(Index));
                }

                var OldTrainer = _gDGContext.Trainer.AsNoTracking().Where(x => x.TrainerCode == id).FirstOrDefault();
                trainer.TrainerInfo = OldTrainer.TrainerInfo;
                trainer.TrainerAct = OldTrainer.TrainerAct;
                trainer.TrainerStartDay = OldTrainer.TrainerStartDay;
                ModelState.Remove("TrainerStartDay");
                if (ModelState.IsValid)
                {
                    try
                    {
                        _gDGContext.Trainer.Update(trainer);
                        _gDGContext.SaveChanges();
                        return Redirect("Trainers/Details/" + trainer.TrainerInfo);
                    }
                    catch(Exception e)
                    {
                        ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", trainer.TrainerAct);
                        ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName", trainer.TrainerInfo);
                        ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                        return View(trainer);
                    }

                }
                ViewData["TrainerAct"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", trainer.TrainerAct);
                ViewData["TrainerInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType == x.TrainerLabel), "PId", "PName", trainer.TrainerInfo);
                return View(trainer);

            }
            return new RedirectResult(_sessionTracing.RoutPage());

        }

        
        public ActionResult Delete(int id)
        {
           
            return RedirectToAction(nameof(Index));
        }

       
    }
}
