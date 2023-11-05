
using MongoDB.Driver;

public class MovieService : IMovieService
{
    private readonly MongoDbDatabase _db;

    public MovieService(MongoDbDatabase db)
    {
        _db = db;   
    }

    public List<OmdbMovie> FilterMoviesByGenre(string genre)
    {
        
        return _db._movies.Find(movie => movie.Genre.Contains(genre)).ToList();
    }

    public List<OmdbMovie> FilterMoviesByReleaseYear(string year)
    {
        return _db._movies.Find(movie=>movie.Year.Equals(year,StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public OmdbMovie GetMovieByTitle(string title)
    {
         return  _db._movies.Find(x => x.Title ==title).FirstOrDefault();
    }

    public List<OmdbMovie> GetMovies()
    {
         return  _db._movies.Find(x => true).ToList();
    }

    public void InsertMovie(OmdbMovie movie)
    {
       _db._movies.InsertOne(movie);
       
    }
}