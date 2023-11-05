public interface IReviewService
{
    void AddReview(OmdbMovieReview review);

    void DeleteReview(string id);

    List<OmdbMovieReview>GetReviewsByMovieTitle(string title);
  
}