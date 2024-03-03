using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Temps.Aladin;

public class AladinBook
{
    // 제목
    public string Title { get; set; }

    // 작가
    public string Author { get; set; }

    // 출간일
    public DateTime PubDate { get; set; }

    // isbn 10
    public string Isbn10 { get; set; }

    // isbn 13
    public string Isbn13 { get; set; }

    // 이미지 경로
    public string Cover { get; set; }

    // 카테고리 ID
    public string CategoryId { get; set; }

    // 출판사
    public string Publisher { get; set; }
}
