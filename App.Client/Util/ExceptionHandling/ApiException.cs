namespace App.Client.Util.ExceptionHandling;

public class ApiException : Exception
{
    public ApiError ApiError { get; }

    public ApiException(ApiError apiError) : base(apiError?.Detail)
    {
        ApiError = apiError;
    }

    public override string ToString()
    {
        return $"{ApiError?.Title} ({ApiError?.Status}): {ApiError?.Detail}";
    }
}


