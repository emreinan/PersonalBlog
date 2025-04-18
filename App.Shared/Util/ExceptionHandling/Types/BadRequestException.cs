namespace App.Shared.Util.ExceptionHandling.Types;

public class BadRequestException(string message) : Exception(message);
