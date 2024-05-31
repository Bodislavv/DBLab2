using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoviesDomain.Model;
using MoviesInfrastructure;

namespace MoviesInfrastructure.Controllers
{
    public class FilmsController : Controller
    {
        private readonly MoviesDbContext _context;

        public FilmsController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            return View(await _context.Films.ToListAsync());
        }



        // Знайти список фільмів із заданим середнім рейтингом, або вище
        [HttpPost]
        public async Task<IActionResult> HighlyRated(int minRating)
        {
            var query = await _context.Films
                .Where(f => f.Reviews.Any())
        .Select(f => new
        {
            Film = f,
            AverageRating = f.Reviews.Average(r => r.Rating)
        })
        .Where(x => x.AverageRating >= minRating)
        .Select(x => x.Film)
        .ToListAsync();

            return View("Index", query);
        }

        // Знайти фільми з певним актором
        [HttpPost]
        public async Task<IActionResult> FilmsWithActor(string actorName)
        {
            var query = await _context.Films
                .Where(f => f.FilmActors.Any())
                .Where(f => f.FilmActors.Any(fa => fa.Actor.Name == actorName))
                .ToListAsync();

            return View("Index", query);
        }


        //Знайти фільми, які містять всіх тих і тільки тих акторів, що і заданий фільм.
        public async Task<IActionResult> FilmsWithMatchingActors(int filmId)
        {
            string query = @"
        SELECT F.*
        FROM Film F
        WHERE F.Id <> @FilmId
        AND NOT EXISTS (
            SELECT FA1.ActorId
            FROM FilmActor FA1
            WHERE FA1.FilmId = @FilmId
            EXCEPT
            SELECT FA2.ActorId
            FROM FilmActor FA2
            WHERE FA2.FilmId = F.Id
        )
        AND NOT EXISTS (
            SELECT FA2.ActorId
            FROM FilmActor FA2
            WHERE FA2.FilmId = F.Id
            EXCEPT
            SELECT FA1.ActorId
            FROM FilmActor FA1
            WHERE FA1.FilmId = @FilmId
        )";

            List<Film> films = new List<Film>();

            films = await _context.Films
                .FromSqlRaw(query, new SqlParameter("@FilmId", filmId))
                .ToListAsync();

            return View("Index", films);
        }


        // Знайти фільми, що містять всіх тих акторів, що і заданий фільм
        public async Task<IActionResult> FilmsWithAllActors(int filmId)
        {
            string query = @"
        SELECT F.*
        FROM Film F
        WHERE NOT EXISTS (
           SELECT FA1.ActorId
           FROM FilmActor FA1
           WHERE FA1.FilmId = @FilmId
           EXCEPT
           SELECT FA2.ActorId
           FROM FilmActor FA2
           WHERE FA2.FilmId = F.Id
        )
        AND F.Id <> @FilmId";

            var films = await _context.Films
                .FromSqlRaw(query, new SqlParameter("@FilmId", filmId))
                .ToListAsync();

            return View("Index", films);
        }




        //Знайти фільми, які не містять жодного актора, що має заданий фільм.
        public async Task<IActionResult> FilmsWithoutActorsFromFilm(int filmId)
        {
            string query = @"
        SELECT F.*
        FROM Film F
            WHERE NOT EXISTS (
            SELECT *
            FROM FilmActor FA1
            WHERE FA1.FilmId = @FilmId
                AND FA1.ActorId IN (
            SELECT FA2.ActorId
            FROM FilmActor FA2
            WHERE FA2.FilmId = F.Id
            )
        );";

            var films = await _context.Films
                .FromSqlRaw(query, new SqlParameter("@FilmId", filmId))
                .ToListAsync();

            return View("Index", films);
        }


        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Runtime,ReleaseDate,Genre,Director,Description,Country,BoxOffice,Id")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Runtime,ReleaseDate,Genre,Director,Description,Country,BoxOffice,Id")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.Id == id);
        }

    }
}
