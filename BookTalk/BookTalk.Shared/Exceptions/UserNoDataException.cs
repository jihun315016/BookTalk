namespace BookTalk.Shared.Exceptions;

public class UserNoDataException : Exception
{
    public UserNoDataException()
    {

    }

    public UserNoDataException(string message) : base(message)
    {

    }
}
