using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbRepository;
using MongoDbRepository.Helper;

namespace TimeReport.Domain
{
    [CollectionName("Users")]
    public class User : Entity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("blog")]
        public string Blog { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }
    }
}
