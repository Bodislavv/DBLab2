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
    public class QueryController : Controller
    {
        private readonly MoviesDbContext _context;

        public QueryController(MoviesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["FilmId"] = new SelectList(_context.Films, "FilmId", "FilmName");
            return View();
        }
    }
}
