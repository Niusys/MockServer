using Microsoft.Extensions.Logging;
using MockServer.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Niusys.Extensions.Storage.Mongo;
using System;
using System.Threading.Tasks;

namespace MockServer.MongoStorage
{
    public class MockServerNoSqlRepository<TEntity> : NoSqlBaseRepository<TEntity, MockServerMongoSettings>
        where TEntity : IMongoEntity<ObjectId>
    {
        public MockServerNoSqlRepository(MongodbContext<MockServerMongoSettings> mongoDatabase, ILogger<NoSqlBaseRepository<TEntity, MockServerMongoSettings>> logger) : base(mongoDatabase, logger)
        {
        }
    }
}
