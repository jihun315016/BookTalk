﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Aladin;

public class AladinBookQuery
{
    #region 검색 조건
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("author")]
    public string Author { get; set; }

    // 조회 유형(신간 전체 리스트, 베스트 셀러 ...)
    [JsonProperty("queryType")]
    public string QueryType { get; set; }

    // 카테고리 ID
    [JsonProperty("categoryId")]
    public string CategoryId { get; set; }

    // Mall
    [JsonProperty("searchTarget")]
    public string SearchTarget { get; set; }

    // 응답 형식 -> JSON
    [JsonProperty("output")]
    public string Output { get; set; }

    [JsonProperty("start")]
    public int Start { get; set; }

    [JsonProperty("maxResult")]
    public int MaxResult { get; set; }
    #endregion

    [JsonProperty("cover")]
    public string Cover { get; set; }

    #region 응답 정보
    [JsonProperty("item")]
    public List<AladinBook> Item { get; set; }
    #endregion
}
