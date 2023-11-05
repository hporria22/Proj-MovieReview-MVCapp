using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class OmdbMovie
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string ?Title { get; set; }

        [BsonElement("year")]
        public string? Year { get; set; }

        [BsonElement("rated")]
        public string? Rated { get; set; }

        [BsonElement("released")]
        public string? Released { get; set; }

        public string? Runtime { get; set; }
        public string? Genre { get; set; }

        [BsonElement("director")]
        public string? Director { get; set; }
        public string? Writer { get; set; }
        public string? Actors { get; set; }
        public string? Plot { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public string? Awards { get; set; }
        public string? Poster { get; set; }

        [BsonElement("ratings")]
        public List<Rating>? Ratings { get; set; }
        public string? Metascore { get; set; }
        public string? imdbRating { get; set; }
        public string? imdbVotes { get; set; }
        public string? imdbID { get; set; }
        public string? Type { get; set; }
        public string? DVD { get; set; }
        public string? BoxOffice { get; set; }
        public string? Production { get; set; }
        public string? Website { get; set; }
        public string? Response { get; set; }
}
