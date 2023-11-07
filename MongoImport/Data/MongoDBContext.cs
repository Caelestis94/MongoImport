using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoImport.Data;


public class MongoDbContext
{
	
	public MongoClient Client { get; }

	public string ConnectionString { get; } = "mongodb://localhost:27017/";

	public MongoDbContext(string connectionString)
	{
		if (connectionString != null)
		{
      ConnectionString = connectionString;
    }
		Client = new MongoClient(ConnectionString);
	}
	
}