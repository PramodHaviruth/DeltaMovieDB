using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeltaMovieDB.Models
{
    public class ActorDataModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Sex { get; set; }
        public DateTime DOB { get; set; }
        public String Bio { get; set; }

    }
}