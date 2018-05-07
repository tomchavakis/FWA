using FWA.Api.Models;
using FWA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWA.Api.Repository
{
    public interface IMovieRepository
    {
        bool UserExists(int userId);
        bool MovieExists(int movieId);
        IEnumerable<Movie> SearchAllMovies(SearchCriteria q);
        IEnumerable<Movie> GetTopFiveMovies();
        IEnumerable<Movie> GetTopFiveMoviesByUser(int userId);
        bool PostRating(int userId, RatingMovie model);

    }
}