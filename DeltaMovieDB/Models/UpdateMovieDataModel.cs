using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeltaMovieDB.Models
{
    public class UpdateMovieDataModel
    {
        public int Id { get; set; }
        public String MovieName { get; set; }
        public int YearOfRelease { get; set; }
        public int Producer { get; set; }
        public String Plot { get; set; }
        public String Poster { get; set; }
        public List<int> ActorsList { get; set; }
        public HttpPostedFileBase PosterImage { get; set; }
    }
}