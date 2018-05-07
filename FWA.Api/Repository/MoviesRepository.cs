using FWA.Api.Helpers;
using FWA.Api.Models;
using FWA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace FWA.Api.Repository
{
    /// <summary>
    ///  A Repository class to manage Movies
    /// </summary>
    public class MoviesRepository : IMovieRepository
    {
        private readonly MoviesContext ctx;

        /// <summary>
        /// Movies Repository Constructor
        /// </summary>
        /// <param name="ctx">Context</param>
        public MoviesRepository(MoviesContext ctx)
        {
            this.ctx = ctx;
        }

        /// <summary>
        /// Search All Movies according to Search Criteria
        /// </summary>
        /// <param name="q">SearchCriteria</param>
        ///  - Title or partial title of the movie;
        ///  - Year of release;
        ///  - Genre(s).
        /// <returns></returns>
        public IEnumerable<Movie> SearchAllMovies(SearchCriteria q)
        {

            List<Movie> list = new List<Movie>();

            Dictionary<string, bool> objs = QueryBuilder.FilterQuery(q);

            int searchItemsLength = objs.Count(i => i.Value == true);
            if (objs["genres"] && searchItemsLength == 1)
            {
                foreach (var item in q.Genre)
                {
                    list.AddRange(ctx.MovieGenres.Where(i => i.GenreId == item).Select(i => i.Movie));
                }
            }

            if (objs["title"])
                list = ctx.Movies.Include("MovieGenres").Include("Ratings").Where(fi => fi.Title.Contains(q.title)).ToList();
            if (objs["year"] && searchItemsLength > 1)
                list = list.Where(fi => fi.Year == q.year).ToList();
            if (objs["year"] && searchItemsLength == 1)
                list = ctx.Movies.Include("MovieGenres").Include("Ratings").Where(fi => fi.Year == q.year).ToList();

            if (objs["genres"] && searchItemsLength > 1)
            {
                foreach (var item in q.Genre)
                {
                    List<Movie> removeItems = new List<Movie>();
                    foreach (var movie in list)
                    {
                        if (movie.MovieGenres.Count(i => i.GenreId == item) == 0)
                            removeItems.Add(movie);
                    }

                    foreach (var removingItem in removeItems)
                    {
                        list.Remove(removingItem);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// get the details of the top 5 movies based on total user average ratings
        /// In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending title alphabetical order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movie> GetTopFiveMovies()
        {
            List<Movie> list = ctx.Movies.OrderByDescending(i => i.Ratings.Average(avg => avg.Value)).ThenBy(i => i.Title).Take(5).ToList();
            return list;
        }

        /// <summary>
        /// get the details of the top 5 movies based on the highest ratings given by a specific user.
        /// In case of a rating draw (e.g. user scored 5 for 2 movies) return them by ascending title alphabetical order.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movie> GetTopFiveMoviesByUser(int userId)
        {
            List<Movie> list = ctx.Ratings.Where(i => i.UserId == userId).OrderByDescending(i => i.Value).ThenBy(i => i.Movie.Title).Select(i => i.Movie).Take(5).ToList();
            return list;
        }

        /// <summary>
        /// Adds a rating to a movie for a certain user
        /// The rating must be an integer between 1 and 5
        /// If the user already had a rating for that movie, the old rating should be updated to the new value
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="model">RatingMovie Model</param>
        /// <returns></returns>
        public bool PostRating(int userId, RatingMovie model)
        {
            var rating = ctx.Ratings.FirstOrDefault(x => x.MovieId == model.MovieId && x.UserId == userId);

            if (rating == null)
            {
                ctx.Ratings.Add(new Rating { MovieId = model.MovieId, UserId = userId, Value = (int)model.Rating });
            }
            else
            {
                rating.Value = (int)model.Rating;
            }
            ctx.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method for checking if the Movie Exists in the database
        /// </summary>
        /// <param name="movieId">Movie Id</param>
        /// <returns>True if exists/False if not exist</returns>
        public bool MovieExists(int movieId)
        {
            bool result = false;
            Movie user = ctx.Movies.Find(movieId);
            if (user != null)
                result = true;
            return result;
        }

        /// <summary>
        /// Method for checking if the user Exists in the database
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True if exists/False if not exist</returns>
        public bool UserExists(int userId)
        {
            bool result = false;
            User user = ctx.Users.Find(userId);
            if (user != null)
                result = true;
            return result;
        }
    }
}