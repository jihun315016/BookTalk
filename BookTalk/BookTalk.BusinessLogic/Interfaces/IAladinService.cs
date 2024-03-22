using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;

namespace BookTalk.BusinessLogic.Interfaces;

public interface IAladinService
{
    /// <summary>
    /// 도서 리스트 조회
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public Task<AladinBookQuery> GetBooks(string url);
}
