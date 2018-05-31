using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        // GET: Movies

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            List<Movie> movies = _context.Movies.ToList();
            IEnumerable<GenreTypes> genretypes = _context.GenreTypes.ToList();

            MovieListViewModel movieslistviewmodel = new MovieListViewModel { MovieList = movies, GenreTypes = genretypes };

            return View("Index",movieslistviewmodel);
        }
        
        public ActionResult New()
        {
            IEnumerable<GenreTypes> genretypes = _context.GenreTypes.ToList();
            MovieFormViewModel movieformvm = new MovieFormViewModel { genreTypes = genretypes };

            return View("MovieForm", movieformvm);
        }

        public ActionResult Edit(int movieid)
        {
            var movieindb = _context.Movies.SingleOrDefault(m => m.Id == movieid);
            var genretypes = _context.GenreTypes.ToList();

            if (movieindb == null || genretypes == null)
            {
                return HttpNotFound();
            }

            MovieFormViewModel movieformvm = new MovieFormViewModel(movieindb)
            {
                genreTypes = genretypes
            };


            return View("MovieForm", movieformvm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                MovieFormViewModel movieformvm = new MovieFormViewModel(movie) { genreTypes = _context.GenreTypes.ToList() };
                return View("MovieForm", movieformvm);
            }

            movie.DateAdded = DateTime.Now;
            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var movieindb = _context.Movies.SingleOrDefault(m => m.Id == movie.Id);
                movieindb.Name = movie.Name;
                movieindb.ReleaseDate = movie.ReleaseDate;
                movieindb.DateAdded = movie.DateAdded;
                //movieindb.Genre = movie.Genre;
                movieindb.NumberInStock = movie.NumberInStock;


            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            

            return RedirectToAction("Index");
        }

        [Route("movies/detail/{id}")]
        public ActionResult Details(int id)
        {


            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            var genretypes = _context.GenreTypes.ToList();

            MovieFormViewModel movieformvm = new MovieFormViewModel(movie) { genreTypes = genretypes };

            return View("Details", movieformvm);
        }

        [Route("movies/released/{year:regex(\\d{4})}/{month:range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month.ToString("00"));
        }
        
    }
}