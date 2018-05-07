using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWA.Api.Models.ViewModels
{
    public class MovieViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public int yearOfRelease { get; set; }
        public double runningTime { get; set; }
        public Nullable<float> averageRating { get; set; }
    }
}