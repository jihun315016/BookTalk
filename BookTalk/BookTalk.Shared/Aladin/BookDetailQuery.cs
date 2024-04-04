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
    public string Output { get; set; } = "js";

    [JsonProperty("optResult")]
    public string OptResult { get; set; } = "ebookList,usedList,reviewList";

    #endregion

    #region 응답 정보
    public Book Book { get; set; }
    #endregion
}
