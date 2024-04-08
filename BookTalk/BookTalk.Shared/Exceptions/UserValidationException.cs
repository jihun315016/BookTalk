namespace BookTalk.Shared.Exceptions;

public class UserValidationException : Exception
{
    public string Key { get; set; }

    public UserValidationException()
    {
        
    }

    public UserValidationException(string message) : base(message)
    {

    }

    public UserValidationException(string key, string message) : base(message) 
    {
        Key = key;
    }

    public UserValidationException(string key, string message, Exception ex) : base(message, ex) 
    {
        Key = key;
    }
}
