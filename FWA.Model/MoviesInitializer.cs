using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FWA.Model
{
    /// <summary>
    /// Initialize the Database with Sample Data
    /// </summary>
    public class MoviesInitializer : CreateDatabaseIfNotExists<MoviesContext>
    {
        private List<Genre> genres = new List<Genre>();
        private List<Movie> movies = new List<Movie>();
        private List<User> users = new List<User>();
        private List<Rating> ratings = new List<Rating>();

        public void DatabaseInitialization()
        {
            using (MoviesContext db = new MoviesContext())
            {
                db.Database.Initialize(false);
            }
        }

        protected override void Seed(MoviesContext context)
        {
            AddData();
        }


        public void AddData()
        {
            users = GetUsers();
            genres = GetGenres();
            movies = GetMovies();
            ratings = GetRatings();

            try
            {
                using (MoviesContext db = new MoviesContext())
                {
                    db.Users.AddRange(users);
                    db.Movies.AddRange(movies);
                    db.Ratings.AddRange(ratings);
                    db.Genres.AddRange(genres);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            string usersFilePath = AssemblyDirectory + "\\App_Data\\csv\\users.csv";

            using (var reader = new StreamReader(usersFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    User user = new User()
                    {
                        Id = Convert.ToInt32(values[0]),
                        FullName = values[1]
                    };

                    users.Add(user);
                }
            }
            return users;
        }

        public List<Genre> GetGenres()
        {
            List<Genre> genres = new List<Genre>();

            string genresFilePath = AssemblyDirectory + "\\App_Data\\csv\\genres.csv";

            using (var reader = new StreamReader(genresFilePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    Genre genre = new Genre()
                    {
                        Id = Convert.ToInt32(values[0]),
                        Type = values[1]
                    };

                    genres.Add(genre);

                }
            }

            return genres;
        }

        public List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            string moviesFilePath = AssemblyDirectory + "\\App_Data\\csv\\movies.csv";
            try
            {
                using (var reader = new StreamReader(moviesFilePath))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        int year = Convert.ToInt32(values[1].Substring(values[1].Length - 5, 4));
                        List<MovieGenre> movieGenres = new List<MovieGenre>();
                        string[] genresArray = values[2].Split('|');
                        
                        Movie movie = new Movie()
                        {
                            Id = Convert.ToInt32(values[0]),
                            Title = values[1],
                            Year = year
                        };

                        foreach (var item in genresArray)
                        {
                            movieGenres.Add(new MovieGenre()
                            {
                                GenreId = genres.Where(i => i.Type == item).FirstOrDefault().Id,
                                MovieId = movie.Id
                            });
                        }
                        movie.MovieGenres = movieGenres;

                        movies.Add(movie);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return movies;
        }

        public List<Rating> GetRatings()
        {
            List<Rating> ratings = new List<Rating>();

            string ratingsFilePath = AssemblyDirectory + "\\App_Data\\csv\\ratings.csv";
            try
            {
                using (var reader = new StreamReader(ratingsFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        Rating rating = new Rating()
                        { 
                            Value = Convert.ToDouble(values[2]),
                            MovieId = Convert.ToInt32(values[0]),
                            UserId = Convert.ToInt32(values[1])
                        };

                        ratings.Add(rating);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           

            return ratings;
        }

    }
}
