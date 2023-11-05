using Microsoft.AspNetCore.Mvc;
using RestSharp;

public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;

    public ReviewController(IReviewService reviewService, IUserService userService, IMovieService movieService)
    {
     _reviewService = reviewService;   
     _movieService = movieService;
     _userService = userService;
    }

[HttpPost]
public  IActionResult AddReview(OmdbMovieReview review)
{
 if(review!=null)
 {
    var movieTitle  =_movieService.GetMovieByTitle(review.MovieTitle).Title;
    var userEmail = _userService.GetUserByEmail(review.UserEmail).Email;
    OmdbMovieReview review1 = new OmdbMovieReview
    {
      MovieTitle = movieTitle,
      UserEmail  = userEmail,
      ReviewDate = DateTime.Now,
      Comment    = review.Comment,
    };
     _reviewService.AddReview(review1);
    return RedirectToAction("Details","Movie", new{ title = movieTitle});
 }
    
  return Content("Error occured");
 
}

[HttpPost]
public IActionResult DeleteReview(OmdbMovieReview review)
{
  _reviewService.DeleteReview(review.Id);
  return Redirect("/Movie/Details?title=" + review.MovieTitle);
}

}