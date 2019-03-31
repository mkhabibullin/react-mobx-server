using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace MongoDbRepository
{
    [DataContract]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}
