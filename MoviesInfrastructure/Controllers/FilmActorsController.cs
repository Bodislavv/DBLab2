using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesDomain.Model;
using MoviesInfrastructure;

namespace MoviesInfrastructure.Controllers
{
    public class FilmActorsController : Controller
    {
        private readonly MoviesDbContext _context;

        public FilmActorsController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET: FilmActors
        public async Task<IActionResult> Index()
        {
            var moviesDbContext = _context.FilmActors.Include(f => f.Actor).Include(f => f.Film);
            return View(await moviesDbContext.ToListAsync());
        }

        // POST: Знайти усі ролі з певного фільму
        [HttpPost]
        public async Task<IActionResult> RolesByFilmTitle(string filmTitle)
        {
            var query = await _context.Films
                .Where(fa => fa.Title == filmTitle)
                .Include(f => f.FilmActors)
                .ThenInclude(fa => fa.Actor)
                .SelectMany(f => f.FilmActors)
                .ToListAsync();

            return View("Index", query);
        }


        // GET: FilmActors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmActor == null)
            {
                return NotFound();
            }

            return View(filmActor);
        }

        // GET: FilmActors/Create
        public IActionResult Create()
        {
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name");
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title");
            return View();
        }

        // POST: FilmActors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,ActorId,Role,Id")] FilmActor filmActor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmActor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors.FindAsync(id);
            if (filmActor == null)
            {
                return NotFound();
            }
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // POST: FilmActors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmId,ActorId,Role,Id")] FilmActor filmActor)
        {
            if (id != filmActor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmActor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmActorExists(filmActor.Id))
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
            ViewData["ActorId"] = new SelectList(_context.Actors, "Id", "Name", filmActor.ActorId);
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Title", filmActor.FilmId);
            return View(filmActor);
        }

        // GET: FilmActors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmActor = await _context.FilmActors
                .Include(f => f.Actor)
                .Include(f => f.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmActor == null)
            {
                return NotFound();
            }

            return View(filmActor);
        }

        // POST: FilmActors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmActor = await _context.FilmActors.FindAsync(id);
            if (filmActor != null)
            {
                _context.FilmActors.Remove(filmActor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmActorExists(int id)
        {
            return _context.FilmActors.Any(e => e.Id == id);
        }
    }
}
