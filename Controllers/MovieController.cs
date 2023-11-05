using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IReviewService _reviewService;

    public MovieController(IMovieService movieService, IReviewService reviewService)
    {
     _movieService = movieService;
     _reviewService = reviewService;
    }


    
    public  IActionResult ShowMovies()
    {
          var movies = _movieService.GetMovies();
          if(movies!=null)
          { 
            return View("ShowMovies",movies);
          }
      return NotFound("Doesn't Exists");
    }

    
    public IActionResult ShowMoviesByGenre(string genre)
   {
    if(!string.IsNullOrEmpty(genre))
    {
        var movies = _movieService.FilterMoviesByGenre(genre);
        if(movies!=null)
       { 
         return View("ShowMovies",movies);

        }  

    }
  
       
    
    return Redirect("ShowMovies");
    
 }

    public IActionResult ShowMoviesByYear(string year)
    {
      var movies = _movieService.FilterMoviesByReleaseYear(year);
      if(movies!=null)
      {
        return View("ShowMovies",movies);
      }
      return NotFound();
    }
    public IActionResult Details(string title)
    {
      var movie = _movieService.GetMovieByTitle(title);
      if(movie!=null)
      {
       ViewBag.reviews = _reviewService.GetReviewsByMovieTitle(movie.Title);
       return View(movie);
    
      }
      return Content("No movie exists");
    }

    public IActionResult AddMovie()
    {
      return View();
    }

[HttpPost]
public async Task<ActionResult> AddMovie(OmdbMovie movie )
{
  string apiKey = "c8b13f";
  var client = new RestClient("http://www.omdbapi.com");
      var request = new RestRequest();
      request.AddQueryParameter("i",movie.imdbID);
      request.AddQueryParameter("apiKey", apiKey);
      var response = client.Execute(request);
      if(response.IsSuccessful)
      {
          var movie1 = JsonConvert.DeserializeObject<OmdbMovie>(response.Content);
          movie.Poster=movie1.Poster;
          _movieService.InsertMovie(movie);
         
      }  
    return RedirectToAction("ShowMovies","Movie");
}




public IActionResult SearchMethod(string query)
  {
   try
   {
       var apiKey = "c8b13f";
       TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
       string title= textInfo.ToTitleCase( query);             
       var movie = _movieService.GetMovieByTitle(title) ;
   
     if(movie!=null)
    { 
       ViewBag.reviews = _reviewService.GetReviewsByMovieTitle(title);
       return View(movie);
    }
    else
    {
        var client = new RestClient("http://www.omdbapi.com");
        var request = new RestRequest();
        request.AddParameter("apiKey", apiKey);
        request.AddParameter("t", title);
        var response = client.Execute(request);
        if(response.IsSuccessful)
      {
           var movie1 = JsonConvert.DeserializeObject<OmdbMovie>(response.Content);
           _movieService.InsertMovie(movie1);
           return View(movie1);                    
      }
       
          return NoContent();
      
    }        
   }
    catch(Exception ex)
    {
    Console.WriteLine(ex.Message);
     return NoContent();
    }       
    
}
   
}

 

    // public async Task<IActionResult> FilterByGenre( string genre)
    // {
    //     var movies = _movies.FilterMoviesByGenre(genre);
    //     ViewBag.Movies = movies;
    //     return RedirectToAction("ShowMovies","Movie");
    // }
