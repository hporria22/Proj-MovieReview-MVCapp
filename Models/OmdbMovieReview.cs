using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class OmdbMovieReview
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id{ get; set; }

    [BsonElement("movieTitle")]
    public string MovieTitle { get; set; }

    [BsonElement("userEmail")]
    public string UserEmail { get;set;}

    [BsonElement("comment")]
    public string Comment { get; set; }

    [BsonElement("reviewDate")]
    public DateTime ReviewDate { get; set; }
}