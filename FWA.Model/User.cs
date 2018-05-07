using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWA.Model
{
    public class User : BaseModel
    {
        public User()
        {
            Ratings = new HashSet<Rating>();
        }

        /// <summary>
        /// User FullName
        /// </summary>
        public string FullName { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
