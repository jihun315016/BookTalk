using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Common;


public class ResponseMessage
{
    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }

    [JsonProperty("validationError")]
    public ValidationError ValidationError { get; set; } = new ValidationError();
}



public class ResponseMessage<T>
{
    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }

    [JsonProperty("validationError")]
    public ValidationError ValidationError { get; set; } = new ValidationError();


    public void InitializeResponseMessage(string errorMessage, T data)
    {
        if (string.IsNullOrEmpty(this.ErrorCode))
        {
            this.ErrorCode = UserStatusCode.UndefinedError.ToString();
        }

        this.ErrorMessage = errorMessage;
        this.Data = data;
    }
}
