

using Microsoft.Extensions.Configuration;
using MongoDotnet.Base;
using MongoDotnet.Infrastructure;
using MongoDotnet.Models;
using MongoDotnet.Services;
using SimpleInjector;

static class Application {

    public static Container? Container { get; private set; }

    static void ConfigureContainer(IConfiguration configuration)
    {
        Container = new Container();
        Container.RegisterSingleton<IStorageProvider>(()=> new MongoDbProvider(configuration));
        Container.Register<IStorageService, StorageService>(Lifestyle.Singleton);
        Container.Verify();
    }

    public static void Init(IConfiguration configuration)
    {
        ConfigureContainer(configuration);

        test();
    }

    static void test()
    {
        var storageService = Container?.GetInstance<IStorageService>() ?? throw new InvalidOperationException("Container not initialized");
        var movies = storageService.GetAll<Movie>();
        foreach (var movie in movies)
        {
            Console.WriteLine(movie.Title);
        } 
    }


}