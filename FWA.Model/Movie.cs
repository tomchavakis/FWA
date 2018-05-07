using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWA.Model
{
    public class Movie : BaseModel
    {
        public Movie()
        {
            MovieGenres = new HashSet<MovieGenre>();
            Ratings = new HashSet<Rating>();
        }

        /// <summary>
        /// The Title of the Movie
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Released Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Set of Genre(s)
        /// </summary>
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }


        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
