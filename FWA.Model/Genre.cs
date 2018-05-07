using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWA.Model
{
    public class Genre : BaseModel
    {
        public Genre()
        {
            MovieGenres = new HashSet<MovieGenre>();
        }

        /// <summary>
        /// Type of Genre ex. Adventure
        /// </summary>
        public string Type { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

    }
}
