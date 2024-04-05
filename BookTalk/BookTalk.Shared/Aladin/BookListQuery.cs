using Newtonsoft.Json;

namespace BookTalk.Shared.Aladin;

public class BookListQuery
{
    #region 검색 조건 / 세팅 값
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; } = "";

    [JsonProperty("query")]
    public string Query { get; set; } = "";

    [JsonProperty("author")]
    public string Author { get; set; } = "";

    // 조회 유형(신간 전체 리스트, 베스트 셀러, 제목 ...)
    [JsonProperty("queryType")]
    public string QueryType { get; set; } = "";

    // 카테고리 ID
    [JsonProperty("categoryId")]
    public string CategoryId { get; set; } = "0";

    // Mall
    [JsonProperty("searchTarget")]
    public string SearchTarget { get; set; } = "";

    // 응답 형식 -> JSON
    [JsonProperty("output")]
    public string Output { get; set; } = "";

    [JsonProperty("start")]
    public int Start { get; set; } = 1;

    [JsonProperty("maxResult")]
    public int MaxResult { get; set; } = default(int);

    [JsonProperty("cover")]
    public string Cover { get; set; } = "";

    // 검색어 
    [JsonProperty("keyword")]
    public string Keyword { get; set; } = "";

    [JsonProperty("page")]
    public int Page { get; set; } = 1;

    [JsonProperty("minPage")]
    public int MinPage { get; set; } = 1;

    [JsonProperty("maxPage")]
    public int MaxPage { get; set; } = default(int);
    #endregion

    #region 응답 정보

    [JsonProperty("totalResults")]
    public int TotalResults { get; set; } = default;

    [JsonProperty("item")]
    public List<Book> Item { get; set; } = new List<Book>();

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; } = "";

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = "";
    #endregion
}
