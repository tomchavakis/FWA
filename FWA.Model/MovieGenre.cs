using System.ComponentModel.DataAnnotations.Schema;

namespace FWA.Model
{
    public class MovieGenre : BaseModel
    {
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public int GenreId { get; set; }

        public virtual Movie Movie { get; set; }
    }
}