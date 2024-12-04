
using Microsoft.Extensions.Configuration;
using MongoDotnet.Models;

namespace MongoDotnet.Infrastructure;


public class MongoDbTests 
{

    private MongoDbProvider? _provider;

    void Setup()
    {
        if (_provider != null)
            return;

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _provider = new MongoDbProvider(configuration);
    }



    [Fact]
    public void Should_check_notnull_mongodbprovider_instance()
    {
        Setup();
        // Assert
        Assert.NotNull(_provider);
    }

    [Fact]
    public void Should_insert_one()
    {
        Setup();
        // Act
        var obj= new Movie { Title = "Test Movie", Year = 2021, Director = "Unknown Director" };
        _provider!.Save(obj);
        
        var movies = _provider.GetAll<Movie>();
        // Assert
        Assert.NotEmpty(movies);
    }




}

