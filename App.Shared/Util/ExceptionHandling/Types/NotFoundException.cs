namespace App.Shared.Util.ExceptionHandling.Types;

public class NotFoundException(string message) : Exception(message);
