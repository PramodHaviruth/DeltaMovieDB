using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeltaMovieDB.Models
{
    public class MovieDataModel
    {
        public String Name { get; set; }
        public int YearOfRelease { get; set; }
        public int Producer { get; set; }
        public String Plot { get; set; }
        public HttpPostedFileBase Poster { get; set; }
        public List<int> ActorsList { get; set; }
    }
}