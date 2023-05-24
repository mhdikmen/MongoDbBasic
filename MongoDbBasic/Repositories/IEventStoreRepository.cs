using MongoDbConsoleBasic.Models;

namespace MongoDbConsoleBasic.Repositories
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @event);
        Task<List<EventModel>> FindById(string id);
        Task<List<EventModel>> FindAllAsync();
        Task UpdateAsync(EventModel @event);
        Task DeleteByIdAsync(string id);
    }
}
