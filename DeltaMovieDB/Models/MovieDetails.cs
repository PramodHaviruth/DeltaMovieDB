using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeltaMovieDB.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public String MovieName { get; set; }
        public int YearOfRelease { get; set; }
        public int ProducerId { get; set; }
        public String ProducerName { get; set; }
        public String Plot { get; set; }
        public String Poster { get; set; }
        public List<ActorDataModel> ActorsList { get; set; }

    }
}