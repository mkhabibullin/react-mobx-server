using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbRepository;
using MongoDbRepository.Helper;
using System.Runtime.Serialization;

namespace TimeReport.Domain
{
    [CollectionName("Users")]
    public class User : Entity
    {
        [DataMember]
        [BsonElement("name")]
        public string Name { get; set; }

        [DataMember]
        [BsonElement("blog")]
        public string Blog { get; set; }

        [DataMember]
        [BsonElement("age")]
        public int Age { get; set; }

        [DataMember]
        [BsonElement("location")]
        public string Location { get; set; }
    }
}
