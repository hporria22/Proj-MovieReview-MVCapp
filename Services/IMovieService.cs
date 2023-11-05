public interface IMovieService
{
    List<OmdbMovie> GetMovies();
    OmdbMovie GetMovieByTitle(string title);
    void InsertMovie(OmdbMovie movie);
    List<OmdbMovie> FilterMoviesByGenre(string genre);
    List<OmdbMovie> FilterMoviesByReleaseYear(string year);
}