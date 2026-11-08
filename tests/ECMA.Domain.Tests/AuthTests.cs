using ECMA.Domain.Entities;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using ECMA.Domain.Results;
using ECMA.Domain.Validation;
using EMCA.Application.DTOs;
using EMCA.Application.Services;
using Moq;
using Xunit;

namespace ECMA.Domain.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IPasswordService> _mockPasswords = new();
    private readonly Mock<ISecurityStampGenerator> _mockStamps = new();

    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _service = new AuthService(_mockUnitOfWork.Object, _mockPasswords.Object, _mockStamps.Object);
    }

    [Fact]
    public async Task RegisterAsync_ValidRequest_CreatesUser()
    {
        var request = new RegisterRequest("testuser", "Test User", "Password123", "Password123");
        _mockStamps.Setup(s => s.GenerateStamp()).Returns("stamp123");
        _mockUnitOfWork.Setup(u => u.Users.UsernameExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _service.RegisterAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("testuser", result.Value.Username);
        Assert.Equal("Test User", result.Value.FullName);
        Assert.Equal(UserRole.Attendee.ToString(), result.Value.Role);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateUsername_ReturnsFailure()
    {
        var request = new RegisterRequest("testuser", "Test User", "Password123", "Password123");
        _mockUnitOfWork.Setup(u => u.Users.UsernameExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _service.RegisterAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Username", result.Error.Code);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsUserProfile()
    {
        var user = new User("testuser", "hash", "stamp", "Test User", UserRole.Attendee, UserStatus.Active);
        _mockUnitOfWork.Setup(u => u.Users.FindByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswords.Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Verified);

        var request = new LoginRequest("testuser", "Password123");
        var result = await _service.LoginAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("testuser", result.Value.Username);
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ReturnsFailure()
    {
        var user = new User("testuser", "hash", "stamp", "Test User", UserRole.Attendee, UserStatus.Inactive);
        _mockUnitOfWork.Setup(u => u.Users.FindByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var request = new LoginRequest("testuser", "Password123");
        var result = await _service.LoginAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ChangePasswordAsync_ValidRequest_UpdatesPassword()
    {
        var user = new User("testuser", "oldhash", "stamp", "Test User", UserRole.Attendee, UserStatus.Active);
        _mockUnitOfWork.Setup(u => u.Users.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswords.Setup(p => p.VerifyPassword("OldPassword", "oldhash"))
            .Returns(PasswordVerificationResult.Verified);
        _mockPasswords.Setup(p => p.HashPassword("NewPassword123")).Returns("newhash");
        _mockStamps.Setup(s => s.GenerateStamp()).Returns("newstamp");
        _mockUnitOfWork.Setup(u => u.Users.TryUpdatePasswordAsync(
                user.Id, "oldhash", "stamp", "newhash", "newstamp", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new ChangePasswordRequest("OldPassword", "NewPassword123", "NewPassword123");
        var result = await _service.ChangePasswordAsync(user.Id, request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ChangePasswordAsync_ConcurrentChange_ReturnsSafeFailure()
    {
        var user = new User("testuser", "oldhash", "stamp", "Test User", UserRole.Attendee, UserStatus.Active);
        _mockUnitOfWork.Setup(u => u.Users.FindByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockPasswords.Setup(p => p.VerifyPassword("OldPassword", "oldhash")).Returns(PasswordVerificationResult.Verified);
        _mockPasswords.Setup(p => p.HashPassword("NewPassword123")).Returns("newhash");
        _mockStamps.Setup(s => s.GenerateStamp()).Returns("newstamp");
        _mockUnitOfWork.Setup(u => u.Users.TryUpdatePasswordAsync(
                user.Id, "oldhash", "stamp", "newhash", "newstamp", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _service.ChangePasswordAsync(
            user.Id,
            new ChangePasswordRequest("OldPassword", "NewPassword123", "NewPassword123"),
            CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("CurrentPassword", result.Error.Code);
    }

    [Fact]
    public async Task ChangePasswordAsync_WrongCurrentPassword_ReturnsFailure()
    {
        var user = new User("testuser", "oldhash", "stamp", "Test User", UserRole.Attendee, UserStatus.Active);
        _mockUnitOfWork.Setup(u => u.Users.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockPasswords.Setup(p => p.VerifyPassword("WrongPassword", "oldhash"))
            .Returns(PasswordVerificationResult.Failed);

        var request = new ChangePasswordRequest("WrongPassword", "NewPassword123", "NewPassword123");
        var result = await _service.ChangePasswordAsync(user.Id, request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("CurrentPassword", result.Error.Code);
    }

    [Fact]
    public async Task ChangePasswordAsync_MissingCurrentPassword_ReturnsFieldErrorBeforeLookup()
    {
        var result = await _service.ChangePasswordAsync(
            Guid.NewGuid(),
            new ChangePasswordRequest("", "NewPassword123", "NewPassword123"),
            CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("CurrentPassword", result.Error.Code);
        _mockUnitOfWork.Verify(u => u.Users.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockPasswords.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateProfileAsync_ValidRequest_UpdatesFullName()
    {
        var user = new User("testuser", "hash", "stamp", "Old Name", UserRole.Attendee, UserStatus.Active);
        _mockUnitOfWork.Setup(u => u.Users.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _service.UpdateProfileAsync(user.Id, "New Name", CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("New Name", result.Value.FullName);
    }
}

public class AuthValidatorTests
{
    [Fact]
    public void Password_UsesCodePointsAndAsciiCharacterClasses()
    {
        Assert.True(PasswordValidator.IsValid("viet12345"));
        Assert.True(PasswordValidator.IsValid("VIET12345"));
        Assert.True(PasswordValidator.IsValid("A1🔒🔒🔒🔒🔒🔒"));
        Assert.False(PasswordValidator.IsValid("ÀÀ123456"));
        Assert.False(PasswordValidator.IsValid("a١bcdefgh"));
        Assert.False(PasswordValidator.IsValid("a1bc\tdef"));
        Assert.False(PasswordValidator.IsValid("a1bc\u00A0def"));
        Assert.True(PasswordValidator.IsValid("a1bcdefg" + string.Concat(Enumerable.Repeat("🔒", 24))));
        Assert.False(PasswordValidator.IsValid("a1bcdefg" + string.Concat(Enumerable.Repeat("🔒", 25))));
    }

    [Fact]
    public void FullName_UsesNormalizedCodePoints()
    {
        Assert.True(FullNameValidator.IsValid("😀😀"));
        Assert.True(FullNameValidator.IsValid(string.Concat(Enumerable.Repeat("😀", 100))));
        Assert.False(FullNameValidator.IsValid(string.Concat(Enumerable.Repeat("😀", 101))));
        Assert.True(FullNameValidator.IsValid("A\u0301B"));
    }
}