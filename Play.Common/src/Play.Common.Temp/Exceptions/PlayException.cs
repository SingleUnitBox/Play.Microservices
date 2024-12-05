namespace Play.Common.Temp.Exceptions;

public class PlayException : Exception
{
    public string Message { get; set; }

    public PlayException(string message)
    {
        Message = message;
    }
}