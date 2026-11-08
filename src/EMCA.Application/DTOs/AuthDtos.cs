namespace EMCA.Application.DTOs;

public sealed record RegisterRequest(string Username, string FullName, string Password, string ConfirmPassword);
public sealed record LoginRequest(string Username, string Password);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmNewPassword);
public sealed record UserProfile(Guid Id, string Username, string FullName, string Role);
