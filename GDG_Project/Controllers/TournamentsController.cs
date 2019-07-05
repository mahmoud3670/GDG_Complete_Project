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
using Microsoft.AspNetCore.Hosting;

namespace GDG_Project.Controllers
{
    public class TournamentsController :GlobalController 
    {
       
        private ISessionTracing _sessionTracing;

        public TournamentsController(GDGContext gDGContext, ISessionTracing sessionTracing,IHostingEnvironment env):base(gDGContext,env)
        {
          
            _sessionTracing = sessionTracing;

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _sessionTracing.Authorization();
            if ((user == null) || (user.EmpPostion != user.admin || user.EmpPostion == user.dataEntry))
            {
                context.Result = new RedirectResult(_sessionTracing.RoutPage());

            }
        }
        // GET: Tournaments
        public ActionResult Index()
        {
            var Model = (from p in _GDGContext.PersonInfo
                 join t in _GDGContext.Tournaments
                 on p.PId equals t.MemberInfo
                 select p).ToList();

            return View(Model);
        }
        public ActionResult ViewTour(int? id)
        {
            if (id == null)
            {
                return Redirect(nameof(Index));
            }
            var Model = _GDGContext.PersonInfo.AsNoTracking().Where(x => x.PId == id).Include(x=>x.Tournaments).FirstOrDefault();
            if (Model == null)
            {
                return Redirect(nameof(Index));
            }
            ViewBag.Tour = _GDGContext.Tournaments.AsNoTracking().Where(x => x.MemberInfo == id).ToList();
           
            return View(Model);
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect(nameof(Index));
            }

            var tournaments = _GDGContext.Tournaments
                .Include(t => t.Act)
                .Include(t => t.MemberInfoNavigation)
                .FirstOrDefault();
            if (tournaments == null)
            {
                return Redirect(nameof(Index));
            }

            return View(tournaments);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            ViewData["ActId"] = new SelectList(_GDGContext.Activates, "ActId", "ActName");
            ViewData["MemberInfo"] = new SelectList(_GDGContext.PersonInfo.Where(x=>x.PType!=x.EmployeesLabel), "PId", "PName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (Tournaments tournaments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var file = HttpContext.Request.Form.Files;
                    if (file.Count > 0)
                    {

                        var imgName = saveImg(file, "Tour");
                        //save data in db
                        _GDGContext.Add(tournaments);
                        tournaments.MemberImg = "/img/Tour/" + imgName;
                      
                        _GDGContext.SaveChanges();
                        
                        return RedirectToAction(nameof(Index));


                    }

                    ViewData["ActId"] = new SelectList(_GDGContext.Activates, "ActId", "ActName", tournaments.ActId);
                    ViewData["MemberInfo"] = new SelectList(_GDGContext.PersonInfo.Where(x => x.PType != x.EmployeesLabel), "PId", "PName", tournaments.MemberInfo);
                    return View(tournaments);
                }

                ViewData["ActId"] = new SelectList(_GDGContext.Activates, "ActId", "ActName", tournaments.ActId);
                ViewData["MemberInfo"] = new SelectList(_GDGContext.PersonInfo.Where(x => x.PType != x.EmployeesLabel), "PId", "PName", tournaments.MemberInfo);
                return View(tournaments);
            }
            catch (Exception e)
            {
                ViewData["ActId"] = new SelectList(_GDGContext.Activates, "ActId", "ActName", tournaments.ActId);
                ViewData["MemberInfo"] = new SelectList(_GDGContext.PersonInfo.Where(x => x.PType != x.EmployeesLabel), "PId", "PName", tournaments.MemberInfo);
                ViewBag.mes = "we sory cant add now ";
                _sessionTracing.LogEventError(e.TargetSite.ToString(), _sessionTracing.Authorization().EmpInfo, DateTime.Now + e.Message);
                return View(tournaments);

            }
           
        }

        //// GET: Tournaments/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
                return RedirectToAction(nameof(Index));
            //}

            //var tournaments = _GDGContext.Tournaments.Find(id);
            //if (tournaments == null)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["ActId"] = new SelectList(_context.Activates, "ActId", "ActDescription", tournaments.ActId);
            //ViewData["MemberInfo"] = new SelectList(_context.PersonInfo, "PId", "PAdress", tournaments.MemberInfo);
            //return View(tournaments);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TourId,MemberInfo,TourName,TourDescription,TourDate,MemberImg,MemberLevel,ActId")] Tournaments tournaments)
        //{
        //    if (id != tournaments.TourId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tournaments);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TournamentsExists(tournaments.TourId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ActId"] = new SelectList(_context.Activates, "ActId", "ActDescription", tournaments.ActId);
        //    ViewData["MemberInfo"] = new SelectList(_context.PersonInfo, "PId", "PAdress", tournaments.MemberInfo);
        //    return View(tournaments);
        //}

        //// GET: Tournaments/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tournaments = await _context.Tournaments
        //        .Include(t => t.Act)
        //        .Include(t => t.MemberInfoNavigation)
        //        .FirstOrDefaultAsync(m => m.TourId == id);
        //    if (tournaments == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tournaments);
        //}

        //// POST: Tournaments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tournaments = await _context.Tournaments.FindAsync(id);
        //    _context.Tournaments.Remove(tournaments);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TournamentsExists(int id)
        //{
        //    return _context.Tournaments.Any(e => e.TourId == id);
        //}
    }
}
