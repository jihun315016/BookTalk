using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Aladin;

public class BookQuery
{
    #region 검색 조건
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
    public string SearchTarget { get; set; } = "Book";

    // 응답 형식 -> JSON
    [JsonProperty("output")]
    public string Output { get; set; } = "js";

    [JsonProperty("start")]
    public int Start { get; set; } = 1;

    [JsonProperty("maxResult")]
    public int MaxResult { get; set; } = 0;

    [JsonProperty("cover")]
    public string Cover { get; set; } = "Big";
    #endregion

    #region 응답 정보
    [JsonProperty("item")]
    public List<Book> Item { get; set; } = new List<Book>();

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; } = "";

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; } = "";
    #endregion
}
