using DeltaMovieDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace DeltaMovieDB.Controllers
{
    public class HomeController : Controller
    {
        /*Entity Object representing database*/
        private DeltaMovieDBEntities DeltaMovieDBModel = new DeltaMovieDBEntities();

        /*MovieDetails Object is used as a temp storage while moving to update movie view */
        private static MovieDetails movieDetailsObj;

        /*
            Input: None
            Description: Index view controller method
            Output: Index View
        */
        public ActionResult Index()
        {
            Console.Write("Start of method Index");
            Console.Write("End of method Index");
            return View();
        }

        /*
            Input: None
            Description: Add New Movie view controller method
            Output: Add New Movie View
        */
        public ActionResult AddNewMovie()
        {
            Console.Write("Start of method AddNewMovie");
            Console.Write("End of method AddNewMovie");
            return View();
        }

        /*
            Input: None
            Description: Update Movie view controller method
            Output: Update Movie View
        */
        public ActionResult UpdateMovie()
        {
            Console.Write("Start of method UpdateMovie");
            Console.Write("End of method UpdateMovie");
            return View();
        }

        /*
            Input: MovieDetails
            Description: setMovieDetails is setter method used to fill the movieDetailsObj
            Output: void
        */
        public void setMovieDetails(MovieDetails movieDetails)
        {
            Console.Write("Start of method setMovieDetails");

            movieDetailsObj = movieDetails;

            Console.Write("End of method setMovieDetails");

        }

        /*
            Input: None
            Description: getMovieDetails is getter method used to fetch the movieDetailsObj
            in a json form to view
            Output: JsonResult
        */
        public JsonResult getMovieDetails()
        {
            Console.Write("Start of method getMovieDetails");
            Console.Write("End of method getMovieDetails");

            return Json(movieDetailsObj, JsonRequestBehavior.AllowGet);
        }

        /*
            Input: None
            Description: getListOfMovies is a public method which will be called by
            the movie list view to get the list of movies
            Output: JsonResult
        */
        public JsonResult getListOfMovies()
        {
            Console.Write("Start of method getListOfMovies");

            var movieList = callSp_GetMoviesList();

            Console.Write("End of method getListOfMovies");
            return Json(movieList, JsonRequestBehavior.AllowGet);
        }

        /*
            Input: None
            Description: callSp_GetMoviesList is a  method which makes a call to store proc GetMovieList to get the list of movies
            and fills the object MovieDetails and then adds it to the list and returns it.
            Output: List<MovieDetails>
        */
        private List<MovieDetails> callSp_GetMoviesList()
        {
            Console.Write("Start of method callSp_GetMoviesList");

            List<MovieDetails> movieDetailsList = new List<MovieDetails>();

            try
            {
                var movieList = DeltaMovieDBModel.GetMoviesList().ToList();

                foreach(var movie in movieList)
                {
                    var movieDataObj = new MovieDetails();

                    movieDataObj.MovieName = movie.Name;
                    movieDataObj.Id = movie.MovieId;
                    movieDataObj.YearOfRelease = movie.YearOfRelease;
                    movieDataObj.Plot = movie.Plot;
                    movieDataObj.ProducerId = movie.ProducerId;
                    movieDataObj.ProducerName = movie.ProducerName;
                    movieDataObj.ActorsList = callSP_getActorsInMovieList(movieDataObj.Id);

                    var PosterByteArr = movie.Poster;

                    string imgString = Convert.ToBase64String(PosterByteArr);

                    movieDataObj.Poster = "data:image/png;base64," + imgString;

                    movieDetailsList.Add(movieDataObj);
 
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method callSp_GetMoviesList");
            return movieDetailsList;
        }

        /*
            Input: int
            Description: callSP_getActorsInMovieList is a  method which makes a call to store proc GetActorsinMovieList 
            to get actors in a movie and fills the object ActorDataModel and then adds it to the list and returns it.
            Output: List<ActorDataModel>
        */
        private List<ActorDataModel> callSP_getActorsInMovieList(int movieId)
        {
            Console.Write("Start of method callSP_getActorsInMovieList");

            List<ActorDataModel> actorDetailsList = new List<ActorDataModel>();

            try
            {
                var actorsInMovieList = DeltaMovieDBModel.GetActorsinMovieList(movieId).ToList();

                foreach (var actor in actorsInMovieList)
                {
                    var actorDataObj = new ActorDataModel();

                    actorDataObj.Id = actor.ActorId;
                    actorDataObj.Name = actor.Name;
                    actorDataObj.Bio = actor.Bio;
                    actorDataObj.Sex = actor.Sex;
                    actorDataObj.DOB = actor.DOB;

                    actorDetailsList.Add(actorDataObj);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method callSP_getActorsInMovieList");
            return actorDetailsList;
        }

        /*
            Input: MovieDataModel
            Description: addMovieDetails is a  method which converts the poster HttpPostedFileBase to byte arr and 
            calls to callSP_InsertMovieDetails with the movieDataModelObj and poster image byte arr
            and returns with a json response 
            Output: JsonResult
        */
        public JsonResult addMovieDetails(MovieDataModel movieDataModelObj)
        {
            Console.Write("Start of method addMovieDetails");

            MemoryStream memStr = new MemoryStream();
            movieDataModelObj.Poster.InputStream.CopyTo(memStr);
            byte[] posterImage = memStr.ToArray();

            var movieResponseObj = callSP_InsertMovieDetails(movieDataModelObj, posterImage);

            Console.Write("End of method addMovieDetails");
            return Json(movieResponseObj, JsonRequestBehavior.AllowGet);
        }

        /*
            Input: MovieDataModel, byte[]
            Description: callSP_InsertMovieDetails is a  method which calls the store proc InsertMovieDetails
            and with successful insertion gets the movieId and uses it to call callSp_InsertActorMovieDetails with actors list
            and returns retcode of the insertion.
            Output: MovieResponseDataModel
        */
        private MovieResponseDataModel callSP_InsertMovieDetails(MovieDataModel movieDataModelObj, byte[] posterImage)
        {
            Console.Write("Start of method callSP_InsertMovieDetails");

            ObjectParameter Out_MovieId = new ObjectParameter("movieId", typeof(int));

            var movieResponseObj = new MovieResponseDataModel();

            try
            {
                var movieId = DeltaMovieDBModel.InsertMovieDetails(movieDataModelObj.Name, movieDataModelObj.YearOfRelease,
                                                                   movieDataModelObj.Plot, posterImage, movieDataModelObj.Producer,
                                                                   Out_MovieId);

                movieResponseObj.MovieId = int.Parse(Out_MovieId.Value.ToString());

                movieResponseObj.RetCode = callSp_InsertActorMovieDetails(movieDataModelObj.ActorsList, movieResponseObj.MovieId);

            }
            catch(Exception e)
            {
                Console.Write(e);
                movieResponseObj.RetCode = RetCodes.ResponseCodes.Failure.GetHashCode();
            }

            Console.Write("End of method callSP_InsertMovieDetails");
            return movieResponseObj;
        }

        /*
            Input: List<int>, int
            Description: callSp_InsertActorMovieDetails is a  method which calls the store proc InsertActorMovieDetails
            and returns retcode of the insertion.
            Output: int
        */
        private int callSp_InsertActorMovieDetails(List<int> actorsList, int movieId)
        {
            Console.Write("Start of method callSp_InsertActorMovieDetails");

            int retCode;

            try
            {
                foreach (var actorId in actorsList)
                {
                    if (actorId != 0)
                    {
                        DeltaMovieDBModel.InsertActorMovieDetails(actorId, movieId);
                    }
                }

                retCode = RetCodes.ResponseCodes.Success.GetHashCode();
            }
            catch (Exception e)
            {
                Console.Write(e);
                retCode = RetCodes.ResponseCodes.Failure.GetHashCode();
            }

            Console.Write("End of method callSp_InsertActorMovieDetails");
            return retCode;
        }

        /*
           Input: ProducerDataModel
           Description: addProducerDetails is a  method which calls  callSP_InsertProducerDetails
           to insert producer details
           Output: JsonResult
       */
        public JsonResult addProducerDetails(ProducerDataModel producerDataModelObj)
        {
            Console.Write("Start of method addProducerDetails");

            callSP_InsertProducerDetails(producerDataModelObj);

            Console.Write("End of method addProducerDetails");
            return Json(producerDataModelObj, JsonRequestBehavior.AllowGet);
        }

        /*
           Input: ActorDataModel
           Description: addActorDetails is a  method which calls  callSP_InsertActorDetails
           to insert actor details
           Output: JsonResult
       */
        public JsonResult addActorDetails(ActorDataModel actorDataModelObj)
        {
            Console.Write("Start of method addActorDetails");

            callSP_InsertActorDetails(actorDataModelObj);

            Console.Write("End of method addActorDetails");
            return Json(actorDataModelObj, JsonRequestBehavior.AllowGet);
        }

        /*
          Input: ProducerDataModel
          Description: callSP_InsertProducerDetails is a  method which calls the store proc InsertProducerDetails
          to insert producer details in DB
          Output: void
      */
        private void callSP_InsertProducerDetails(ProducerDataModel producerDataModelObj)
        {
            Console.Write("Start of method callSP_InsertProducerDetails");

            try
            {
                DeltaMovieDBModel.InsertProducerDetails(producerDataModelObj.Name, producerDataModelObj.Sex, producerDataModelObj.DOB, producerDataModelObj.Bio);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method callSP_InsertProducerDetails");
        }

        /*
          Input: ActorDataModel
          Description: callSP_InsertActorDetails is a  method which calls the store proc InsertActorDetails
          to insert actor details in DB
          Output: void
        */
        private void callSP_InsertActorDetails(ActorDataModel actorDataModelObj)
        {
            Console.Write("Start of method callSP_InsertActorDetails");

            try
            {
                DeltaMovieDBModel.InsertActorDetails(actorDataModelObj.Name, actorDataModelObj.Sex, actorDataModelObj.DOB, actorDataModelObj.Bio);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method callSP_InsertActorDetails");
        }

        /*
          Input: None
          Description: getActorsList is a  method which calls the store proc GetActorsList
          to fetch actor details from DB
          Output: JsonResult
        */
        public JsonResult getActorsList()
        {
            Console.Write("Start of method getActorsList");

            List<ActorDataModel> ListOfActors = new List<ActorDataModel>();
            try
            {
                var actorsList = DeltaMovieDBModel.GetActorsList().ToList();

                foreach(var actor in actorsList)
                {
                    var actorDataObj = new ActorDataModel();

                    actorDataObj.Id = actor.ActorId;
                    actorDataObj.Name = actor.Name;
                    actorDataObj.Sex = actor.Sex;
                    actorDataObj.DOB = actor.DOB;
                    actorDataObj.Bio = actor.Bio;

                    ListOfActors.Add(actorDataObj);
                }
            }
            catch(Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method getActorsList");
            return Json(ListOfActors, JsonRequestBehavior.AllowGet);
        }

        /*
          Input: None
          Description: getProducersList is a  method which calls the store proc GetProducersList
          to fetch producer details from DB
          Output: JsonResult
        */
        public JsonResult getProducersList()
        {
            Console.Write("Start of method getProducersList");

            List<ProducerDataModel> ListOfProducers = new List<ProducerDataModel>();
            try
            {
                var producersList = DeltaMovieDBModel.GetProducersList().ToList();

                foreach (var producer in producersList)
                {
                    var producerDataObj = new ProducerDataModel();

                    producerDataObj.Id = producer.ProducerId;
                    producerDataObj.Name = producer.Name;
                    producerDataObj.Sex = producer.Sex;
                    producerDataObj.DOB = producer.DOB;
                    producerDataObj.Bio = producer.Bio;

                    ListOfProducers.Add(producerDataObj);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method getProducersList");
            return Json(ListOfProducers, JsonRequestBehavior.AllowGet);
        }

        /*
         Input: UpdateMovieDataModel
         Description: updateMovieDetails is a method which create a byte array of the poster image 
         and calls callSP_UpdateMovieDetails with the movieDataModelObj and poster Image
         and returns response retcode
         Output: JsonResult
       */
        public JsonResult updateMovieDetails(UpdateMovieDataModel movieDataModelObj)
        {
            Console.Write("Start of method updateMovieDetails");

            var posterPicBase64Str = movieDataModelObj.Poster.Split(',')[1];
            var posterPic = Convert.FromBase64String(posterPicBase64Str);

            if(movieDataModelObj.PosterImage != null)
            {
                MemoryStream memStr = new MemoryStream();
                movieDataModelObj.PosterImage.InputStream.CopyTo(memStr);
                posterPic = memStr.ToArray();
            }

            var movieResponseObj = callSP_UpdateMovieDetails(movieDataModelObj, posterPic);

            Console.Write("End of method updateMovieDetails");
            return Json(movieResponseObj, JsonRequestBehavior.AllowGet);
        }

        /*
         Input: UpdateMovieDataModel, byte[]
         Description: callSP_UpdateMovieDetails is a method which calls store proc UpdateMovieDetails
         to update the movie details and returns the reponse retcode
         Output: MovieResponseDataModel
       */
        private MovieResponseDataModel callSP_UpdateMovieDetails(UpdateMovieDataModel movieDataModelObj, byte[] posterPic)
        {
            Console.Write("Start of method callSP_UpdateMovieDetails");

            var movieResponseObj = new MovieResponseDataModel();

            try
            {
                DeltaMovieDBModel.UpdateMovieDetails(movieDataModelObj.Id, movieDataModelObj.MovieName, movieDataModelObj.YearOfRelease,
                                                    movieDataModelObj.Plot, posterPic, movieDataModelObj.Producer);

                movieResponseObj.RetCode = callSp_InsertActorMovieDetails(movieDataModelObj.ActorsList, movieDataModelObj.Id);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            Console.Write("End of method callSP_UpdateMovieDetails");

            return movieResponseObj;
        }
    }
}