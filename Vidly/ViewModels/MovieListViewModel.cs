using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieListViewModel
    {
        public List<Movie> MovieList { get; set; }
        public IEnumerable<GenreTypes> GenreTypes { get; set; }
    }
}