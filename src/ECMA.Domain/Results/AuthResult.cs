using ECMA.Domain.Entities;

namespace ECMA.Domain.Results;

public class AuthResult : Result<User>
{
    public AuthResult(bool isSuccess, User? user, Error? error)
        : base(isSuccess, user, error)
    {
    }

    public static AuthResult Success(User user) => new(true, user, null);
    public static AuthResult Failure(Error error) => new(false, null, error);
}