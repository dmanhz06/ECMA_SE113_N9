namespace ECMA.Domain.Results;

public class PasswordChangeResult : Result
{
    public PasswordChangeResult(bool isSuccess, Error? error)
        : base(isSuccess, error)
    {
    }

    public static PasswordChangeResult Success() => new(true, null);
    public static PasswordChangeResult Failure(Error error) => new(false, error);
}