namespace ECMA.Domain.Results;

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error Validation(string code, string message) => new(code, message);
    public static Error NotFound(string message) => new("NotFound", message);
    public static Error Conflict(string message) => new("Conflict", message);
    public static Error Unauthorized(string message) => new("Unauthorized", message);
    public static Error BadRequest(string message) => new("BadRequest", message);
    public static Error Forbidden(string message) => new("Forbidden", message);
}