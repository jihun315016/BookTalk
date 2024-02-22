using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Temps;

public class ResponseMessage
{
    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }
}


public class ResponseMessage<T>
{
    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public T Data { get; set; }
}
