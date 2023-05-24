using MongoDbBasic;
using MongoDbConsoleBasic;
using MongoDbConsoleBasic.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<MongoDbConfig>(context.Configuration.GetSection(nameof(MongoDbConfig)));
        services.AddSingleton<IEventStoreRepository, EventStoreRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
