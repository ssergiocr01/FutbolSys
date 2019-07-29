using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FutbolSys.Domain;
using FutbolSys.Web.Models;
using FutbolSys.Web.Helpers;
using System;

namespace FutbolSys.Web.Controllers
{
    public class LeaguesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Leagues";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var league = ToLeague(view);
                league.Logo = pic;
                db.Leagues.Add(league);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private League ToLeague(LeagueView view)
        {
            return new League
            {
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                Teams = view.Teams,
            };
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }

            var view = ToView(league);
            return View(view);
        }

        private LeagueView ToView(League league)
        {
            return new LeagueView
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = league.Teams,
            };
        }

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Leagues";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var league = ToLeague(view);
                league.Logo = pic;
                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Leagues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
