using System.Text;
using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Exceptions;
using ECMA.Domain.Interfaces;
using ECMA.Domain.Results;
using ECMA.Domain.Validation;
using EMCA.Application.DTOs;

namespace EMCA.Application.Services;

public sealed class AuthService(
    IUnitOfWork unitOfWork,
    IPasswordService passwords,
    ISecurityStampGenerator stamps)
{
    private const string LoginFailure = "Không thể đăng nhập. Kiểm tra thông tin hoặc trạng thái tài khoản";

    public async Task<Result<UserProfile>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var username = request.Username.Normalize(NormalizationForm.FormC).Trim();
        var fullName = request.FullName.Normalize(NormalizationForm.FormC).Trim();
        if (!UsernameValidator.IsValid(username)) return Result<UserProfile>.Failure(Error.Validation("Username", "Tên đăng nhập không hợp lệ"));
        if (!FullNameValidator.IsValid(fullName)) return Result<UserProfile>.Failure(Error.Validation("FullName", "Họ tên phải dài từ 2 đến 100 ký tự"));
        if (!PasswordValidator.IsValid(request.Password)) return Result<UserProfile>.Failure(Error.Validation("Password", "Mật khẩu không hợp lệ"));
        if (request.Password != request.ConfirmPassword) return Result<UserProfile>.Failure(Error.Validation("ConfirmPassword", "Xác nhận mật khẩu không khớp"));

        username = UsernameValidator.Normalize(username);
        if (await unitOfWork.Users.UsernameExistsAsync(username, cancellationToken))
            return DuplicateUsername();

        var user = new User(username, passwords.HashPassword(request.Password), stamps.GenerateStamp(), fullName, UserRole.Attendee, UserStatus.Active);
        unitOfWork.Users.Add(user);
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DuplicateUsernameException)
        {
            return DuplicateUsername();
        }

        return Result<UserProfile>.Success(ToProfile(user));
    }

    public async Task<Result<UserProfile>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var username = request.Username.Normalize(NormalizationForm.FormC).Trim();
        if (!UsernameValidator.IsValid(username) || string.IsNullOrEmpty(request.Password))
            return Result<UserProfile>.Failure(Error.Validation("Login", LoginFailure));

        var user = await unitOfWork.Users.FindByUsernameAsync(UsernameValidator.Normalize(username), cancellationToken);
        if (user is null || user.Status != UserStatus.Active)
            return Result<UserProfile>.Failure(Error.Unauthorized(LoginFailure));

        var verification = passwords.VerifyPassword(request.Password, user.PasswordHash);
        if (verification == PasswordVerificationResult.Failed)
            return Result<UserProfile>.Failure(Error.Unauthorized(LoginFailure));

        if (verification == PasswordVerificationResult.VerifiedNeedsRehash)
        {
            await unitOfWork.Users.TryUpdatePasswordAsync(
                user.Id,
                user.PasswordHash,
                user.SecurityStamp,
                passwords.HashPassword(request.Password),
                user.SecurityStamp,
                cancellationToken);
        }

        return Result<UserProfile>.Success(ToProfile(user));
    }

    public async Task<Result<UserProfile>> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.FindByIdAsync(userId, cancellationToken);
        return user is null ? Result<UserProfile>.Failure(Error.NotFound("Không tìm thấy dữ liệu")) : Result<UserProfile>.Success(ToProfile(user));
    }

    public async Task<Result<UserProfile>> UpdateProfileAsync(Guid userId, string fullName, CancellationToken cancellationToken = default)
    {
        fullName = fullName.Normalize(NormalizationForm.FormC).Trim();
        if (!FullNameValidator.IsValid(fullName))
            return Result<UserProfile>.Failure(Error.Validation("FullName", "Họ tên phải dài từ 2 đến 100 ký tự"));

        var user = await unitOfWork.Users.FindByIdAsync(userId, cancellationToken);
        if (user is null) return Result<UserProfile>.Failure(Error.NotFound("Không tìm thấy dữ liệu"));
        user.UpdateFullName(fullName);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<UserProfile>.Success(ToProfile(user));
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.CurrentPassword)) return Result.Failure(Error.Validation("CurrentPassword", "Mật khẩu hiện tại là bắt buộc"));
        if (!PasswordValidator.IsValid(request.NewPassword)) return Result.Failure(Error.Validation("NewPassword", "Mật khẩu mới không hợp lệ"));
        if (request.NewPassword != request.ConfirmNewPassword) return Result.Failure(Error.Validation("ConfirmNewPassword", "Xác nhận mật khẩu không khớp"));

        var user = await unitOfWork.Users.FindByIdAsync(userId, cancellationToken);
        if (user is null) return Result.Failure(Error.NotFound("Không tìm thấy dữ liệu"));
        if (passwords.VerifyPassword(request.CurrentPassword, user.PasswordHash) == PasswordVerificationResult.Failed)
            return Result.Failure(Error.Validation("CurrentPassword", "Mật khẩu hiện tại không đúng"));
        if (passwords.VerifyPassword(request.NewPassword, user.PasswordHash) != PasswordVerificationResult.Failed)
            return Result.Failure(Error.Validation("NewPassword", "Mật khẩu mới phải khác mật khẩu hiện tại"));

        bool updated = await unitOfWork.Users.TryUpdatePasswordAsync(
            user.Id,
            user.PasswordHash,
            user.SecurityStamp,
            passwords.HashPassword(request.NewPassword),
            stamps.GenerateStamp(),
            cancellationToken);
        return updated
            ? Result.Success()
            : Result.Failure(Error.Validation("CurrentPassword", "Mật khẩu hiện tại không đúng"));
    }

    private static Result<UserProfile> DuplicateUsername() => Result<UserProfile>.Failure(Error.Validation("Username", "Tên đăng nhập đã tồn tại"));
    private static UserProfile ToProfile(User user) => new(user.Id, user.Username, user.FullName, user.Role.ToString());
}
