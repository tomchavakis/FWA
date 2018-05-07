using AutoMapper;
using FWA.Api.Filters;
using FWA.Api.Helpers;
using FWA.Api.Models;
using FWA.Api.Models.ViewModels;
using FWA.Api.Repository;
using FWA.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FWA.Api.Controllers
{
    /// <summary>
    /// Movies Controller
    /// </summary>
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiController
    {
        private readonly IMovieRepository movieRepository;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor of MoviesController
        /// </summary>
        /// <param name="movieRepository">IMovieRepository</param>
        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }


        /// <summary>
        /// return the details of movies that pass certain filter criteria provided by the api consumers.
        /// </summary>
        /// <param name="q">Search Criteria :  
        ///  - Title or partial title of the movie;
        ///  - Year of release;
        ///  - Genre(s).
        ///  At least one filter criteria should be provided by the caller
        ///  </param>
        /// <returns>List of Movies</returns>
        [HttpGet]
        [ValidateModelAttribute]
        [Route("search")]
        [ResponseType(typeof(List<MovieViewModel>))]
        public async Task<HttpResponseMessage> SearchMovies([FromUri] SearchCriteria q)
        {
            log.Debug(string.Format("SearchMovies(){0}", JsonConvert.SerializeObject(q, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));

            if (q == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "SearchCriteria Cannot be null");
            }

            try
            {
                IEnumerable<Movie> list = movieRepository.SearchAllMovies(q);

                if (list.ToList().Count == 0)
                {
                    log.Debug(string.Format("SearchMovies()= NotFound{0}", JsonConvert.SerializeObject(q, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No movie is found based on the criteria");
                }

                IEnumerable<MovieViewModel> result = Mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(list);
                return Request.CreateResponse<IEnumerable<MovieViewModel>>(HttpStatusCode.OK, result);
            }
            catch (System.Exception ex)
            {
                log.Debug(string.Format("SearchMovies()= Exception{0}", JsonConvert.SerializeObject(ex, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// return the details of the top 5 movies based on total user average ratings
        /// In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending title alphabetical order
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("topfive")]
        [ResponseType(typeof(List<MovieViewModel>))]
        public async Task<HttpResponseMessage> GetTopFiveMovies()
        {
            log.Debug(string.Format("GetTopFiveMovies()"));

            try
            {
                IEnumerable<Movie> list = movieRepository.GetTopFiveMovies();

                if (list.ToList().Count == 0)
                {
                    log.Debug(string.Format("GetTopFiveMovies()= NotFound"));
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, "No movie is found based on the criteria");
                }

                IEnumerable<MovieViewModel> result = Mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(list);
                return Request.CreateResponse<IEnumerable<MovieViewModel>>(HttpStatusCode.OK, result);
            }
            catch (System.Exception ex)
            {
                log.Debug(string.Format("GetTopFiveMovies()= Exception{0}", JsonConvert.SerializeObject(ex, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// return the details of the top 5 movies based on the highest ratings given by a specific user, provided by the api consumer
        /// In case of a rating draw (e.g. user scored 5 for 2 movies) return them by ascending title alphabetical order.
        /// </summary>
        /// <param name="userId">The Id of the User</param>
        /// <returns>List of Movies</returns>
        [HttpGet]
        [ValidateModelAttribute]
        [Route("user/{userId}/topfive")]
        [ResponseType(typeof(List<MovieViewModel>))]
        public async Task<HttpResponseMessage> GetTopFiveMoviesByUser(int userId)
        {
            log.Debug(string.Format("GetTopFiveMoviesByUser(){0}", JsonConvert.SerializeObject(userId, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));


            try
            {
                bool exist = movieRepository.UserExists(userId);

                if (!exist)
                {
                    log.Debug(string.Format("GetTopFiveMoviesByUser()= User NotFound{0}", JsonConvert.SerializeObject(userId, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, "No user is found based on this userId");
                }

                IEnumerable<Movie> list = movieRepository.GetTopFiveMoviesByUser(userId);

                if (list.ToList().Count == 0)
                {
                    log.Debug(string.Format("GetTopFiveMoviesByUser()= NotFound{0}", JsonConvert.SerializeObject(userId, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, "No ratings are found based on this userId");
                }

                IEnumerable<MovieViewModel> result = Mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(list);
                return Request.CreateResponse<IEnumerable<MovieViewModel>>(HttpStatusCode.OK, result);
            }
            catch (System.Exception ex)
            {
                log.Debug(string.Format("GetTopFiveMoviesByUser()= Exception{0}", JsonConvert.SerializeObject(ex, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Adds a rating to a movie for a certain user
        /// The rating must be an integer between 1 and 5
        /// If the user already had a rating for that movie, the old rating should be updated to the new value
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="model">RatingMovie Model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateModelAttribute]
        [Route("user/{userId}/ratingmovie")]
        public async Task<HttpResponseMessage> PostRating(int userId, RatingMovie model)
        {
            log.Debug(string.Format("PostRating({0}) - {1}", userId, JsonConvert.SerializeObject(model, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));

            if (model == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "SearchCriteria Cannot be null");
            }

            try
            {
                bool userExists = movieRepository.UserExists(userId);
                bool movieExists = movieRepository.MovieExists(model.MovieId);

                if (!userExists || !movieExists)
                {
                    log.Debug(string.Format("PostRating()= User or Movie Not Exists {0}, {1}", userId, JsonConvert.SerializeObject(model, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, "User or Movie not found");
                }

                bool posted = movieRepository.PostRating(userId, model);

                if (!posted)
                {
                    log.Debug(string.Format("PostRating()= NotFound{0}", JsonConvert.SerializeObject(model, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                    return this.Request.CreateResponse(HttpStatusCode.NotFound, "No movie is found based on the criteria");
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception ex)
            {
                log.Debug(string.Format("PostRating()= Exception{0}", JsonConvert.SerializeObject(ex, FWA.Api.Properties.Settings.Default.Tracing ? Formatting.Indented : Formatting.None)));
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
