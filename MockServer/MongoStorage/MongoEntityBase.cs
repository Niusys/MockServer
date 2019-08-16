using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Niusys.Extensions.Storage.Mongo;
using System;

namespace MockServer.MongoStorage
{
    [BsonIgnoreExtraElements(true), Serializable]
    public class MongoEntityBase : IMongoEntity<ObjectId>
    {
        public MongoEntityBase()
        {

        }

        #region Properties
        [BsonId, BsonIgnoreIfDefault, BsonIgnoreIfNull]
        public ObjectId Sysid { get; set; }
        #endregion
    }
}
