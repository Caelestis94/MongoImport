using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoImport.Data;


public class MongoDbContext
{
	
	public MongoClient Client { get; }

	public MongoDbContext()
	{
		Client = new MongoClient("mongodb://localhost:27017/TP2DB");
	}
	
}