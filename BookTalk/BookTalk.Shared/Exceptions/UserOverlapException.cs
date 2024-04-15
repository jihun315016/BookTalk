namespace BookTalk.Shared.Exceptions;

public class UserOverlapException : Exception
{
    public UserOverlapException()
    {

    }

    public UserOverlapException(string message) : base(message)
    {

    }
}
