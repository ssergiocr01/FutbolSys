﻿using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FutbolSys.Domain;
using FutbolSys.Web.Models;

namespace FutbolSys.Web.Controllers
{
    public class PredictionsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Predictions
        public async Task<ActionResult> Index()
        {
            var predictions = db.Predictions.Include(p => p.Match).Include(p => p.User);
            return View(await predictions.ToListAsync());
        }

        // GET: Predictions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prediction = await db.Predictions.FindAsync(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            return View(prediction);
        }

        // GET: Predictions/Create
        public ActionResult Create()
        {
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Predictions.Add(prediction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", prediction.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", prediction.UserId);
            return View(prediction);
        }

        // GET: Predictions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prediction = await db.Predictions.FindAsync(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", prediction.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", prediction.UserId);
            return View(prediction);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prediction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", prediction.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", prediction.UserId);
            return View(prediction);
        }

        // GET: Predictions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prediction = await db.Predictions.FindAsync(id);
            if (prediction == null)
            {
                return HttpNotFound();
            }
            return View(prediction);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var prediction = await db.Predictions.FindAsync(id);
            db.Predictions.Remove(prediction);
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
