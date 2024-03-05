﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Common;

public class ResponseMessage
{
    [JsonProperty("errorCode")]
    public int ErrorCode { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
}


public class ResponseMessage<T>
{
    [JsonProperty("errorCode")]
    public int ErrorCode { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }
}