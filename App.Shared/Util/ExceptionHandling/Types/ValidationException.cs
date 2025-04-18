namespace App.Shared.Util.ExceptionHandling.Types;

public class ValidationException(string message) : Exception(message);
