using MongoDB.Bson;

namespace BookTalk.Shared.Common;

public class Session
{
    // MongoDB에서 키가 _id인 데이터
    public ObjectId Id { get; set; }

    public string UserId { get; set; }

    public DateTime LastAccessed { get; set; } = DateTime.UtcNow;
}
