using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookTalk.Shared.Aladin;
using BookTalk.Shared.Common;

namespace BookTalk.BusinessLogic.Interfaces
{
    public interface IAladinService
    {
        Task<ResponseMessage<AladinBookQuery>> GetBooks(string url);
    }
}
