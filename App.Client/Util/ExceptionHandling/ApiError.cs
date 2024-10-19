namespace App.Client.Util.ExceptionHandling;

public class ApiError
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public List<ValidationError> Errors { get; set; }
}

public class ValidationError
{
    public string Property { get; set; }
    public List<string> Errors { get; set; }
}


