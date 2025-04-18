namespace App.Shared.Util.ExceptionHandling.Types;

public class DeserializationException : Exception
{
    public DeserializationException(string message, string? responseContent = null)
        : base($"{message}{(responseContent is not null ? $" | Content: {responseContent}" : "")}")
    {
    }
}