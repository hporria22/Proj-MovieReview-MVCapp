using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDbDatabase 
{
    private readonly IConfiguration _configuration;
   private readonly IMongoDatabase _database;
   

    public MongoDbDatabase(IConfiguration configuration)
    {
        _configuration = configuration;
        var client = new MongoClient(_configuration["MongoDbSettings:ConnectionString"]);
        _database = client.GetDatabase(_configuration["MongoDbSettings:DatabaseName"]);
    }
    public IMongoCollection<OmdbMovie> _movies => _database.GetCollection<OmdbMovie>(_configuration["MongoDbSettings:MovieCollection"]);
    public IMongoCollection<UserDto> _users => _database.GetCollection<UserDto>(_configuration["MongoDbSettings:UsersCollection"]);
    public IMongoCollection<OmdbMovieReview> _reviews => _database.GetCollection<OmdbMovieReview>(_configuration["MongoDbSettings:ReviewCollection"]);
}

  