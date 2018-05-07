using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FWA.Api.Models
{
    public class RatingMovie
    {
        public int MovieId { get; set; }

        [EnumDataType(typeof(RatingValues))]
        public RatingValues Rating { get; set; }
    }
}