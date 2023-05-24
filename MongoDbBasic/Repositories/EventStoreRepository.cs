using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbConsoleBasic.Models;

namespace MongoDbConsoleBasic.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindById(string id)
        {
            return await _eventStoreCollection.Find(x => x.Id == id).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }

        public async Task UpdateAsync(EventModel @event)
        {
            await _eventStoreCollection.UpdateOneAsync(
                Builders<EventModel>.Filter.Eq(p => p.Id, @event.Id),
                Builders<EventModel>.Update.Set(z => z.Name, @event.Name)).ConfigureAwait(false);
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _eventStoreCollection.DeleteOneAsync(x => x.Id == id).ConfigureAwait(false);
        }
    }
}
