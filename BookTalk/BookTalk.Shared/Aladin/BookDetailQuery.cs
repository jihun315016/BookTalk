using Newtonsoft.Json;

namespace BookTalk.Shared.Aladin;

public class BookDetailQuery
{
    #region 검색 조건 / 세팅 값
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; } = "";

    [JsonProperty("itemIdType")]
    public string ItemIdType { get; set; }

    [JsonProperty("itemId")]
    public string ItemId { get; set; }

    // 응답 형식 -> JSON
    [JsonProperty("output")]
    public string Output { get; set; } = "";

    [JsonProperty("optResult")]
    public string OptResult { get; set; } = "";

    [JsonProperty("cover")]
    public string Cover { get; set; } = "";
    #endregion

    #region 응답 정보
    [JsonProperty("item")]
    public List<Book> Item { get; set; } = new List<Book>();
    #endregion
}
