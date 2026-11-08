using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using ECMA.Domain.Results;
using EMCA.Application.DTOs;

namespace EMCA.Application.Services;

public sealed class SessionValidator(IUserRepository users)
{
    public static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(30);

    public async Task<Result<UserProfile>> ValidateAsync(Guid userId, string securityStamp, DateTime lastActivityUtc, DateTime nowUtc, CancellationToken cancellationToken = default)
    {
        if (nowUtc - lastActivityUtc >= IdleTimeout)
            return Invalid();

        var user = await users.FindByIdAsync(userId, cancellationToken);
        if (user is null || user.Status != UserStatus.Active || !StringComparer.Ordinal.Equals(user.SecurityStamp, securityStamp))
            return Invalid();

        return Result<UserProfile>.Success(new(user.Id, user.Username, user.FullName, user.Role.ToString()));
    }

    private static Result<UserProfile> Invalid() => Result<UserProfile>.Failure(Error.Unauthorized("Vui lòng đăng nhập bằng tài khoản đang hoạt động"));
}