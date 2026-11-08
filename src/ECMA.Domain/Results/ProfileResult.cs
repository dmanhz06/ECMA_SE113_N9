using ECMA.Domain.Entities;

namespace ECMA.Domain.Results;

public class ProfileResult : Result<User>
{
    public ProfileResult(bool isSuccess, User? user, Error? error)
        : base(isSuccess, user, error)
    {
    }

    public static ProfileResult Success(User user) => new(true, user, null);
    public static ProfileResult Failure(Error error) => new(false, null, error);
}