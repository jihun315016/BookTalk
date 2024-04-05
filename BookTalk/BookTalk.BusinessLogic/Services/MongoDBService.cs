using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using MongoDB.Driver;
using System.Net.Http;

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

    public Session CreateOrUpdateSession(string userId)
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0); // 세션 컬렉션
        var session = sessions.Find(s => s.UserId == userId).FirstOrDefault();
        if (session == null)
        {
            sessions.InsertOne(new Session { UserId = userId });
            return sessions.Find(s => s.UserId == userId).FirstOrDefault();
        }
        else
        {
            // 이미 세션이 존재하면 세션 접근 날짜만 업데이트 
            var update = Builders<Session>.Update.Set(s => s.LastAccessed, DateTime.UtcNow);
            sessions.UpdateOne(s => s.UserId == userId, update);
            return session;
        }
    }

    public void DeleteSession(string sessionId)
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0);
        var filter = Builders<Session>.Filter.Eq(s => s.Id, sessionId);
        sessions.DeleteOne(filter);
    }

    public async Task DeleteAllSessions()
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0); // 세션 컬렉션
        await sessions.DeleteManyAsync(FilterDefinition<Session>.Empty);
    }

    public Session GetSession(string sessionId)
    {
        IMongoCollection<Session> sessions = GetCollection<Session>(0); 
        return sessions.Find(s => s.Id == sessionId).FirstOrDefault();
    }
}
