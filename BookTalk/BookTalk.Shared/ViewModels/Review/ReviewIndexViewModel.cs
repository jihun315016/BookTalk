using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.ViewModels.Review;

public class ReviewIndexViewModel
{
    #region 검색조건 세팅
    public IEnumerable<SelectListItem> SearchTypeCombo { get; set; } = new List<SelectListItem>();
    #endregion

    #region 검색조건
    public string QueryType { get; set; }

    public string Keyword { get; set; }
    #endregion

    #region 응답
    public IEnumerable<ReviewViewModel> Items { get; set; }
    #endregion
}
