namespace App.Shared.Util.ExceptionHandling.Types;

public class UnauthorizedException(string message) : Exception(message);
