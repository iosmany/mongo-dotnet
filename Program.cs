using Microsoft.Extensions.Configuration;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);

IConfiguration configuration = builder.Build();

Application.Init(configuration);


// var client = new MongoClient(connectionString);

// var collection = client.GetDatabase(databaseName).GetCollection<BsonDocument>("movies");

// var filter = Builders<BsonDocument>.Filter.Eq("title", "Inception");

// var document = collection.Find(filter).FirstOrDefault();

//Console.WriteLine(document);
