using BookTalk.Shared.Common;
using MongoDB.Driver;

namespace BookTalk.BusinessLogic.Services;

public class MongoDBService
{
    private readonly IMongoDatabase _database;
    private readonly string[] _collections;

    public MongoDBService(string connStr, string database, string[] collections)
    {
        var client = new MongoClient(connStr);

        _database = client.GetDatabase(database);
        _collections = collections;
    }

    public IMongoCollection<T> GetCollection<T>(int index)
    {
        // [_collections index 정보]
        // _collections[0] : 세션
        return _database.GetCollection<T>(_collections[index]);
    }

    public void CreateOrUpdateSession(string userId)
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0); // 세션 컬렉션
        var session = sessions.Find(s => s.UserId == userId).FirstOrDefault();
        if (session == null)
        {
            sessions.InsertOne(new Session { UserId = userId });
        }
        else
        {
            // 이미 세션이 존재하면 세션 접근 날짜만 업데이트
            var update = Builders<Session>.Update.Set(s => s.LastAccessed, DateTime.UtcNow);
            sessions.UpdateOne(s => s.UserId == userId, update);
        }
    }

    public async Task DeleteAllSessions()
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0); // 세션 컬렉션
        await sessions.DeleteManyAsync(FilterDefinition<Session>.Empty);
    }
}
