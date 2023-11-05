
using MongoDB.Bson;
using MongoDB.Driver;

public class ReviewService : IReviewService
{
 private readonly MongoDbDatabase _db;

public ReviewService(MongoDbDatabase db)
 {
    _db = db;
 }

    public void AddReview(OmdbMovieReview review)
    {
         _db._reviews.InsertOne(review);
    }

    public void DeleteReview(string id)
    {
        _db._reviews.DeleteOne(x => x.Id == id);
    }

    public List<OmdbMovieReview> GetReviewsByMovieTitle(string title)
    {
        return _db._reviews.Find(x => x.MovieTitle==title).ToList();
    }
}