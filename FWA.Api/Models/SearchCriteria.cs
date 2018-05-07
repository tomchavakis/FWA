using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWA.Api.Models
{
    public class SearchCriteria
    {
        public int[] Genre { get; set; }

        public string title { get; set; }

        public int? year { get; set; }
    }
}