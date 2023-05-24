using MongoDbConsoleBasic.Models;
using MongoDbConsoleBasic.Repositories;
using System.Net.Http.Headers;

namespace MongoDbBasic
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public Worker(ILogger<Worker> logger, IEventStoreRepository eventStoreRepository, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _eventStoreRepository = eventStoreRepository;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                //Insert
                EventModel eventModel = new() { Id = Guid.Empty.ToString(), Name = "Hello World" };
                await _eventStoreRepository.SaveAsync(eventModel).ConfigureAwait(false);
                _logger.Log(LogLevel.Information, $"Saved Model : {eventModel}");

                //Get
                EventModel savedModel = (await _eventStoreRepository.FindById(Guid.Empty.ToString()).ConfigureAwait(false)).FirstOrDefault() ?? throw new ArgumentNullException(eventModel?.ToString());
                _logger.Log(LogLevel.Information, $"Get Model : {savedModel}");

                //Update
                eventModel.Name = "Hello World Updated";
                await _eventStoreRepository.UpdateAsync(eventModel).ConfigureAwait(false);
                _logger.Log(LogLevel.Information, $"Updated Model : {savedModel}");

                //Delete
                await _eventStoreRepository.DeleteByIdAsync(eventModel.Id).ConfigureAwait(false);
                _logger.Log(LogLevel.Information, $"Deleted Model : {savedModel}");

                _hostApplicationLifetime.StopApplication();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }
}