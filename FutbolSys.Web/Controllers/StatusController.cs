using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FutbolSys.Domain;
using FutbolSys.Web.Models;
using FutbolSys.Web.Helpers;

namespace FutbolSys.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Status
        public async Task<ActionResult> Index()
        {
            return View(await db.Status.ToListAsync());
        }

        // GET: Status/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = await db.Status.FindAsync(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Status status)
        {
            if (ModelState.IsValid)
            {
                db.Status.Add(status);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var status = await db.Status.FindAsync(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            return View(status);
        }

        // GET: Status/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var status = await db.Status.FindAsync(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var status = await db.Status.FindAsync(id);
            db.Status.Remove(status);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                await db.SaveChangesAsync();
                return RedirectToAction("Index"); 
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View();
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
