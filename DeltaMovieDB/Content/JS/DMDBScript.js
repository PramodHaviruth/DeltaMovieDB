/// <reference path="angular.js" />

var dmdbApp = angular.module("dmdbModule", []);

dmdbApp.controller("dmdbMovieListController", function ($scope, $http) {

        $http.post("/Home/getListOfMovies")
            .then(function (response) {
                $scope.movieList = response.data;
            });
                    
    $scope.updateMovie = function (movieObj) {
        $http.post("/Home/setMovieDetails", movieObj)
            .then(function (response) {
                $scope.movieList = response.data;
            });

        window.location.pathname = "/Home/UpdateMovie";
    };
});

dmdbApp.controller("dmdbAddNewMovieController", function ($scope, $http) {

    $scope.PosterImage = "/Images/DefaultImage.png";

    $scope.Movie = {};

    $scope.Person = {};

    $scope.Person.Sex = 'Male';

    $scope.CurYear = new Date().getFullYear();

    var CurDate = new Date().getDate();
    var CurMon = new Date().getMonth() + 1;
    var CurYear = new Date().getFullYear();

    var maxDate = CurYear.toString() + "-" + CurMon.toString() + "-" + CurDate.toString();
    document.getElementById("PersonDOB").max = maxDate;

    $scope.getProducersList = function () {
        $http.post("getProducersList")
            .then(function (response) {
                $scope.producersList = response.data;
                if (response.data.length > 0) {
                    $scope.Movie.Producer = response.data[0].Id;
                }
            });
    };

    $scope.getActorsList = function () {
        $http.post("getActorsList")
            .then(function (response) {
                $scope.actorsList = response.data;
            });
    };

    $scope.getProducersList();
    $scope.getActorsList();

    $scope.addNewMovie = function () {

        var yearOfRelease = parseInt($scope.Movie.YearOfRelease);

        if (yearOfRelease < 1950 || yearOfRelease > new Date().getFullYear()) {
            alert('Year of Realease must be less than or equal to ' + new Date().getFullYear()+' or greater than equal to 1950');
            return;
        }

        var movieData = new FormData();

        movieData.append('movieDataModelObj.Name', $scope.Movie.Name);
        movieData.append('movieDataModelObj.YearOfRelease', $scope.Movie.YearOfRelease);
        movieData.append('movieDataModelObj.Producer', $scope.Movie.Producer);
        movieData.append('movieDataModelObj.Plot', $scope.Movie.Plot);
        movieData.append('movieDataModelObj.Poster', $scope.Movie.Poster);

        for (var index = 0; index < $scope.Movie.ActorsList.length; index++) {
            movieData.append('movieDataModelObj.ActorsList[' + index + ']', $scope.Movie.ActorsList[index]);
        }

        $http.post("addMovieDetails", movieData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                if (response.data.RetCode === 0) {
                    alert("Successfully added the Movie Details");
                    window.location.pathname = "/";
                }
                else {
                    alert("Failed to add the Movie Details");
                }
            });
    };

    $scope.imageUpload = function (event) {
        var files = event.target.files;

        var file = files[0];
        var reader = new FileReader();
        reader.onload = $scope.imageIsLoaded;
        reader.readAsDataURL(file);
    };

    $scope.imageIsLoaded = function (e) {
        $scope.$apply(function () {
            $scope.PosterImage = e.target.result;
        });
    };

    $('#AddPersonModal').on('hidden.bs.modal', function (e) {
        $scope.Person = {};
        $scope.Person.Sex = 'Male';
    });

    $scope.AddNewPerson = function () {
        if ($scope.PersonType === 'Producer') {

            var ProducerDetailsObj = { 'producerDataModelObj': $scope.Person };
            $('#AddPersonModal').modal('hide');

            $('#AddPersonModal').on('hidden.bs.modal', function (e) {
                $scope.Person = {};
                $scope.Person.Sex = 'Male';
            });

            $http.post("addProducerDetails", ProducerDetailsObj)
                .then(function (response) {
                    $scope.getProducersList();
                    alert('Succesfully added the producer details');
                });

            $scope.Person = {};
            $scope.Person.Sex = 'Male';
        }
        else if ($scope.PersonType === 'Actor') {

            var ActorDetailsObj = { 'actorDataModelObj': $scope.Person };
            $('#AddPersonModal').modal('hide');

            $('#AddPersonModal').on('hidden.bs.modal', function (e) {
                $scope.Person = {};
                $scope.Person.Sex = 'Male';
            });

            $http.post("addActorDetails", ActorDetailsObj)
                .then(function (response) {
                    $scope.movieList = response.data;
                    $scope.getActorsList();
                    alert('Succesfully added the actor details');
                });
        }
    };

    $scope.OpenProducerModal = function () {
        $scope.PersonType = 'Producer';

        $('#AddPersonModal').modal({ backdrop: 'true' });
    };

    $scope.OpenActorModal = function () {
        $scope.PersonType = 'Actor';
        $('#AddPersonModal').modal({ backdrop: 'true' });

    };

});

dmdbApp.controller("dmdbUpdateMovieController", function ($scope, $http) {

    $scope.Movie = {};

    $scope.Person = {};

    $scope.Person.Sex = 'Male';

    $scope.CurYear = new Date().getFullYear();

    var CurDate = new Date().getDate();
    var CurMon = new Date().getMonth() + 1;
    var CurYear = new Date().getFullYear();

    var maxDate = CurYear.toString() + "-" + CurMon.toString() + "-" + CurDate.toString();
    document.getElementById("PersonDOB").max = maxDate;

    $scope.OpenProducerModal = function () {
        $scope.PersonType = 'Producer';

        $('#AddPersonModal').modal({ backdrop: 'true' });
    };

    $scope.OpenActorModal = function () {
        $scope.PersonType = 'Actor';
        $('#AddPersonModal').modal({ backdrop: 'true' });

    };

    $http.post("/Home/getMovieDetails")
        .then(function (response) {
            $scope.Movie = response.data;

            $scope.getProducersList();
            $scope.getActorsList();
        });

    $scope.getProducersList = function () {
        $http.post("getProducersList")
            .then(function (response) {
                $scope.producersList = response.data;
                $scope.Movie.Producer = $scope.Movie.ProducerId;
            });
    };

    $scope.getActorsList = function () {
        $http.post("getActorsList")
            .then(function (response) {
                $scope.actorsList = response.data;
                $scope.Movie.SelectedActorsList = $scope.Movie.ActorsList;
            });
    };

    $scope.updateMovie = function () {

        var yearOfRelease = parseInt($scope.Movie.YearOfRelease);

        if (yearOfRelease < 1950 || yearOfRelease > new Date().getFullYear()) {
            alert('Year of Realease must be less than or equal to ' + new Date().getFullYear() + ' or greater than equal to 1950');
            return;
        }

        var movieData = new FormData();

        movieData.append('movieDataModelObj.Id', $scope.Movie.Id);
        movieData.append('movieDataModelObj.MovieName', $scope.Movie.MovieName);
        movieData.append('movieDataModelObj.YearOfRelease', $scope.Movie.YearOfRelease);
        movieData.append('movieDataModelObj.Producer', $scope.Movie.Producer);
        movieData.append('movieDataModelObj.Plot', $scope.Movie.Plot);
        movieData.append('movieDataModelObj.Poster', $scope.Movie.Poster);
        movieData.append('movieDataModelObj.PosterImage', $scope.Movie.PosterImage);

        for (var index = 0; index < $scope.Movie.ActorsList.length; index++) {
            movieData.append('movieDataModelObj.ActorsList[' + index + ']', $scope.Movie.ActorsList[index]);
        }

        $http.post("updateMovieDetails", movieData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                if (response.data.RetCode === 0) {
                    alert("Successfully updated the Movie Details");
                    window.location.pathname = "/";
                }
                else {
                    alert("Failed to update the Movie Details");
                }
            });
    }


    $scope.imageUpload = function (event) {
        var files = event.target.files;

        var file = files[0];
        var reader = new FileReader();
        reader.onload = $scope.imageIsLoaded;
        reader.readAsDataURL(file);
    };

    $scope.imageIsLoaded = function (e) {
        $scope.$apply(function () {
            $scope.Movie.Poster = e.target.result;
        });
    };

    $('#AddPersonModal').on('hidden.bs.modal', function (e) {
        $scope.Person = {};
        $scope.Person.Sex = 'Male';
    });

    $scope.AddNewPerson = function () {
        if ($scope.PersonType === 'Producer') {

            var ProducerDetailsObj = { 'producerDataModelObj': $scope.Person };
            $('#AddPersonModal').modal('hide');

            $('#AddPersonModal').on('hidden.bs.modal', function (e) {
                $scope.Person = {};
                $scope.Person.Sex = 'Male';
            });

            $http.post("addProducerDetails", ProducerDetailsObj)
                .then(function (response) {
                    $scope.getProducersList();
                    alert('Succesfully added the producer details');
                });

            $scope.Person = {};
            $scope.Person.Sex = 'Male';
        }
        else if ($scope.PersonType === 'Actor') {

            var ActorDetailsObj = { 'actorDataModelObj': $scope.Person };
            $('#AddPersonModal').modal('hide');

            $('#AddPersonModal').on('hidden.bs.modal', function (e) {
                $scope.Person = {};
                $scope.Person.Sex = 'Male';
            });

            $http.post("addActorDetails", ActorDetailsObj)
                .then(function (response) {
                    $scope.movieList = response.data;
                    $scope.getActorsList();
                    alert('Succesfully added the actor details');
                });
        }
    };

});

dmdbApp.directive("fileread", [function () {
    return {
        scope: {
            fileread: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                scope.$apply(function () {
                    scope.fileread = changeEvent.target.files[0];
                });
            });
        }
    };
}]);

function AllowNumbersOnly(e) {
    var code = e.which ? e.which : e.keyCode;
    if (code > 31 && (code < 48 || code > 57)) {
        e.preventDefault();
    }
}