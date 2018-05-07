using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWA.Model
{
    public class Rating : BaseModel
    {
        /// <summary>
        /// Rating Value
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// User who rated this Movie
        /// </summary>
        public int UserId { get; set; }

        //Id Of The Movie
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
