using System.ComponentModel.DataAnnotations;

namespace ECMA.Web.Models;

public sealed class RegisterViewModel
{
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = "";

    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = "";

    [DataType(DataType.Password), Display(Name = "Mật khẩu")]
    public string Password { get; set; } = "";

    [DataType(DataType.Password), Display(Name = "Xác nhận mật khẩu")]
    public string ConfirmPassword { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    [RegularExpression("^[A-Za-z][A-Za-z0-9_]{3,19}$", ErrorMessage = "Tên đăng nhập không hợp lệ")]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu không hợp lệ")]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*[0-9])\\S+$", ErrorMessage = "Mật khẩu không hợp lệ")]
    [DataType(DataType.Password), Display(Name = "Mật khẩu")]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public sealed class ProfileViewModel
{
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; init; } = "";

    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = "";

    [Display(Name = "Vai trò")]
    public string Role { get; init; } = "";
}

public sealed class ChangePasswordViewModel
{
    [DataType(DataType.Password), Display(Name = "Mật khẩu hiện tại")]
    public string CurrentPassword { get; set; } = "";

    [DataType(DataType.Password), Display(Name = "Mật khẩu mới")]
    public string NewPassword { get; set; } = "";

    [DataType(DataType.Password), Display(Name = "Xác nhận mật khẩu mới")]
    public string ConfirmNewPassword { get; set; } = "";
}
