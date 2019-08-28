using FutbolSys.Domain;
using FutbolSys.Web.Helpers;
using FutbolSys.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FutbolSys.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        /*
         * Métodos Predicciones
         */

        public async Task<ActionResult> Predictions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            return View(match);
        }

        public async Task<ActionResult> CreatePrediction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserId", "FullName");
            var view = new Prediction { MatchId = match.MatchId, Points = 0, Match = match };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePrediction(Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Predictions.Add(prediction);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("Predictions/{0}", prediction.MatchId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            var match = await db.Matches.FindAsync(prediction.MatchId);
            prediction.Match = match;
            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserId", "FullName", prediction.UserId);
            return View(prediction);
        }

        public async Task<ActionResult> EditPrediction(int? id)
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

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserId", "FullName", prediction.UserId);
            return View(prediction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPrediction(Prediction prediction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prediction).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("Predictions/{0}", prediction.MatchId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.FirstName).ThenBy(u => u.LastName), "UserId", "FullName", prediction.UserId);
            return View(prediction);
        }

        public async Task<ActionResult> DeletePrediction(int? id)
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

            db.Predictions.Remove(prediction);
            var response = await DBHelper.SaveChanges(db);

            if (response.Succeeded)
            {
                return RedirectToAction(string.Format("Predictions/{0}", prediction.MatchId));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(prediction);
        }

        /*
         * Métodos Partidos
         */

        public async Task<ActionResult> CreateMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name");

            var view = new MatchView { DateId = date.DateId };
            return View(view);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(MatchView view)
        {
            if (ModelState.IsValid)
            {
                view.StatusId = 1;
                var localDateTime = Convert.ToDateTime(string.Format("{0} {1}", view.DateString, view.TimeString));
                view.DateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime);
                var match = ToMatch(view);
                db.Matches.Add(match);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("DetailsDate/{0}", view.DateId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            var date = await db.Dates.FindAsync(view.DateId);

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LocalLeagueId);
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == view.LocalLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.LocalId);
            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.VisitorLeagueId);
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == view.VisitorLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name", view.TournamentGroupId);

            return View(view);
        }

        private Match ToMatch(MatchView view)
        {
            return new Match
            {
                DateId = view.DateId,
                DateTime = view.DateTime,
                LocalId = view.LocalId,
                MatchId = view.MatchId,
                StatusId = view.StatusId,
                TournamentGroupId = view.TournamentGroupId,
                VisitorId = view.VisitorId,
            };
        }

        public async Task<ActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", match.Local.LeagueId);
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == match.Local.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", match.LocalId);
            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", match.Visitor.LeagueId);
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == match.Visitor.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", match.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == match.Date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name", match.TournamentGroupId);
            ViewBag.StatusId = new SelectList(db.Status.OrderBy(s => s.Name), "StatusId", "Name", match.StatusId);

            var view = ToView(match);
            return View(view);
        }

        private MatchView ToView(Match match)
        {
            return new MatchView
            {
                Date = match.Date,
                DateId = match.DateId,
                DateString = string.Format("{0:dd/MM/yyyy}", match.DateTime.ToLocalTime()),
                DateTime = match.DateTime.ToLocalTime(),
                Local = match.Local,
                LocalGoals = match.LocalGoals,
                LocalId = match.LocalId,
                LocalLeagueId = match.Local.LeagueId,
                MatchId = match.MatchId,
                Status = match.Status,
                StatusId = match.StatusId,
                TimeString = string.Format("{0:hh:mm tt}", match.DateTime.ToLocalTime()),
                TournamentGroup = match.TournamentGroup,
                TournamentGroupId = match.TournamentGroupId,
                Visitor = match.Visitor,
                VisitorGoals = match.VisitorGoals,
                VisitorId = match.VisitorId,
                VisitorLeagueId = match.Visitor.LeagueId,
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMatch(MatchView view)
        {
            if (ModelState.IsValid)
            {
                var localDateTime = Convert.ToDateTime(string.Format("{0} {1}", view.DateString, view.TimeString));
                view.DateTime = TimeZoneInfo.ConvertTimeToUtc(localDateTime);
                var match = ToMatch(view);
                db.Entry(match).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("DetailsDate/{0}", view.DateId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            var date = await db.Dates.FindAsync(view.DateId);
            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LocalLeagueId);
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId == view.LocalLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.LocalId);
            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.VisitorLeagueId);
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId == view.VisitorLeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.VisitorId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId == date.TournamentId).OrderBy(tg => tg.Name), "TournamentGroupId", "Name", view.TournamentGroupId);
            ViewBag.StatusId = new SelectList(db.Status.OrderBy(s => s.Name), "StatusId", "Name", view.StatusId);
            return View(view);
        }        

        public async Task<ActionResult> DeleteMatch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var match = await db.Matches.FindAsync(id);

            if (match == null)
            {
                return HttpNotFound();
            }

            db.Matches.Remove(match);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction(string.Format("DetailsDate/{0}", match.DateId));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(match);
        }

        /*
         * Métodos Equipos
         */

        public async Task<ActionResult> CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name");
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "TeamId", "Name");
            var view = new TournamentTeamView { TournamentGroupId = tournamentGroup.TournamentGroupId };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);
                db.TournamentTeams.Add(tournamentTeam);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == view.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.TeamId);
            return View(view);
        }

        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentTeam = await db.TournamentTeams.FindAsync(id);

            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", tournamentTeam.Team.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == tournamentTeam.Team.LeagueId), "TeamId", "Name", tournamentTeam.Team.TeamId);
            var view = ToView(tournamentTeam);
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);
                db.Entry(tournamentTeam).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(t => t.Name), "LeagueId", "Name", view.LeagueId);
            ViewBag.TeamId = new SelectList(db.Teams.Where(t => t.LeagueId == view.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.TeamId);

            return View(view);
        }

        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentTeam = await db.TournamentTeams.FindAsync(id);

            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            db.TournamentTeams.Remove(tournamentTeam);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction(string.Format("DetailsGroup/{0}", tournamentTeam.TournamentGroupId));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(tournamentTeam);
        }

        private TournamentTeam ToTournamentTeam(TournamentTeamView view)
        {
            return new TournamentTeam
            {
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesTied = view.MatchesTied,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
                Position = view.Position,
                Team = view.Team,
                TeamId = view.TeamId,
                TournamentGroup = view.TournamentGroup,
                TournamentGroupId = view.TournamentGroupId,
                TournamentTeamId = view.TournamentTeamId,
            };
        }

        private TournamentTeamView ToView(TournamentTeam tournamentTeam)
        {
            return new TournamentTeamView
            {
                AgainstGoals = tournamentTeam.AgainstGoals,
                FavorGoals = tournamentTeam.FavorGoals,
                MatchesLost = tournamentTeam.MatchesLost,
                MatchesPlayed = tournamentTeam.MatchesPlayed,
                MatchesTied = tournamentTeam.MatchesTied,
                MatchesWon = tournamentTeam.MatchesWon,
                Points = tournamentTeam.Points,
                Position = tournamentTeam.Position,
                Team = tournamentTeam.Team,
                TeamId = tournamentTeam.TeamId,
                TournamentGroup = tournamentTeam.TournamentGroup,
                TournamentGroupId = tournamentTeam.TournamentGroupId,
                TournamentTeamId = tournamentTeam.TournamentTeamId,
            };
        }

        /*
         * Métodos Grupos
         */

        public async Task<ActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroup);
        }

        public async Task<ActionResult> CreateGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = new TournamentGroup
            {
                TournamentId = tournament.TournamentId
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(tournamentGroup);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(tournamentGroup);
        }

        public async Task<ActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentGroup).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(tournamentGroup);
        }

        /*
         * Métodos Fechas
         */

        public async Task<ActionResult> DetailsDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            return View(date);
        }

        public async Task<ActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournamentGroup = await db.TournamentGroups.FindAsync(id);
            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            db.TournamentGroups.Remove(tournamentGroup);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction(string.Format("Details/{0}", tournamentGroup.TournamentId));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(tournamentGroup);
        }

        public async Task<ActionResult> CreateDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = new Date
            {
                TournamentId = tournament.TournamentId
            };


            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Dates.Add(date);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(date);
        }

        public async Task<ActionResult> EditDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return HttpNotFound();
            }
            return View(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Entry(date).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
            }

            return View(date);
        }

        public async Task<ActionResult> DeleteDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return HttpNotFound();
            }

            db.Dates.Remove(date);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction(string.Format("Details/{0}", date.TournamentId));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(date);
        }


        /*
         * Métodos Torneos
         */

        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Tournaments";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var tournament = ToTournament(view);
                tournament.Logo = pic;
                db.Tournaments.Add(tournament);
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            return View(view);
        }

        private Tournament ToTournament(TournamentView view)
        {
            return new Tournament
            {
                IsActive = view.IsActive,
                Logo = view.Logo,
                Name = view.Name,
                Order = view.Order,
                TournamentId = view.TournamentId,
                TournamentGroups = view.TournamentGroups,
            };
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            var view = ToView(tournament);
            return View(view);
        }

        private TournamentView ToView(Tournament tournament)
        {
            return new TournamentView
            {
                IsActive = tournament.IsActive,
                Logo = tournament.Logo,
                Name = tournament.Name,
                Order = tournament.Order,
                TournamentId = tournament.TournamentId,
                TournamentGroups = tournament.TournamentGroups,
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Tournaments";

                if (view.LogoFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.LogoFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var tournament = ToTournament(view);
                tournament.Logo = pic;
                db.Entry(tournament).State = EntityState.Modified;
                var response = await DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }
            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            db.Tournaments.Remove(tournament);
            var response = await DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(tournament);
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
