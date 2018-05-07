using FWA.Api.Models;
using FWA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FWA.Api.Helpers
{
    public class QueryBuilder
    {

        public static Dictionary<string, bool> FilterQuery(SearchCriteria q)
        {
            Dictionary<string, bool> objs = new Dictionary<string, bool>();
            
            objs.Add("title", false);
            objs.Add("year", false);
            objs.Add("genres", false);

            if (!string.IsNullOrEmpty(q.title))
                objs["title"] = true;
            if (q.year != null)
                objs["year"] = true;
            if (q.Genre != null)
                objs["genres"] = true;

            return objs;
        }
    }
}