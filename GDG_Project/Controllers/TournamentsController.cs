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
    public class TournamentsController : Controller
    {
        private GDGContext _gDGContext;
        private ISessionTracing _sessionTracing;

        public TournamentsController(GDGContext gDGContext, ISessionTracing sessionTracing)
        {
            _gDGContext = gDGContext;
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
            var Model = (from t in _gDGContext.Tournaments
                              join p in _gDGContext.PersonInfo
                              on t.MemberInfo equals p.PId
                              select p).ToList();

            return View(Model);
        }
        public ActionResult ViewTour(int? id)
        {
            if (id == null)
            {
                return Redirect(nameof(Index));
            }
            var Model = _gDGContext.Tournaments.AsNoTracking().Where(x => x.MemberInfo == id).Include(x=>x.MemberInfoNavigation).ToList();
            if (Model == null)
            {
                return Redirect(nameof(Index));
            }
            ViewBag.personInfo = _gDGContext.PersonInfo.AsNoTracking().Where(x => x.PId == id).FirstOrDefault();
            return View(Model);
        }

        //// GET: Tournaments/Details/5
        //public async Task<IActionResult> Details(int? id)
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

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            ViewData["ActId"] = new SelectList(_gDGContext.Activates, "ActId", "ActName");
            ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x=>x.PType!=x.EmployeesLabel), "PId", "PName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (Tournaments tournaments)
        {
            if (ModelState.IsValid)
            {
                _gDGContext.Add(tournaments);
                _gDGContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActId"] = new SelectList(_gDGContext.Activates, "ActId", "ActName", tournaments.ActId);
            ViewData["MemberInfo"] = new SelectList(_gDGContext.PersonInfo.Where(x => x.PType != x.EmployeesLabel), "PId", "PName", tournaments.MemberInfo);
            return View(tournaments);
        }

        //// GET: Tournaments/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tournaments = await _context.Tournaments.FindAsync(id);
        //    if (tournaments == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ActId"] = new SelectList(_context.Activates, "ActId", "ActDescription", tournaments.ActId);
        //    ViewData["MemberInfo"] = new SelectList(_context.PersonInfo, "PId", "PAdress", tournaments.MemberInfo);
        //    return View(tournaments);
        //}

        //// POST: Tournaments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
