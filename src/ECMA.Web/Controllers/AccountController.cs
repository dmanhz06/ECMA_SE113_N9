using System.Security.Claims;
using ECMA.Domain.Interfaces;
using ECMA.Web.Models;
using ECMA.Web.Services;
using EMCA.Application.DTOs;
using EMCA.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECMA.Web.Controllers;

[Route("account")]
public sealed class AccountController(AuthService auth, IUnitOfWork unitOfWork, AuthSessionStore sessions) : Controller
{
    [AllowAnonymous, HttpGet("register")]
    public IActionResult Register(string? returnUrl = null) =>
        User.Identity?.IsAuthenticated == true
            ? Redirect(DefaultDestination())
            : View(new RegisterViewModel { ReturnUrl = ReturnUrlPolicy.Canonicalize(returnUrl) });

    [AllowAnonymous, HttpPost("register"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (User.Identity?.IsAuthenticated == true) return Redirect(DefaultDestination());
        model.ReturnUrl = ReturnUrlPolicy.Canonicalize(model.ReturnUrl);
        if (!ModelState.IsValid) return BadRequestView(nameof(Register), model);

        var result = await auth.RegisterAsync(
            new RegisterRequest(model.Username, model.FullName, model.Password, model.ConfirmPassword),
            HttpContext.RequestAborted);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.Error?.Code ?? string.Empty, result.Error?.Message ?? "Dữ liệu không hợp lệ");
            return BadRequestView(nameof(Register), model);
        }

        TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập";
        return RedirectToAction(nameof(Login), new { returnUrl = model.ReturnUrl });
    }

    [AllowAnonymous, HttpGet("login")]
    public IActionResult Login(string? returnUrl = null) =>
        User.Identity?.IsAuthenticated == true
            ? Redirect(DefaultDestination())
            : View(new LoginViewModel { ReturnUrl = ReturnUrlPolicy.Canonicalize(returnUrl) });

    [AllowAnonymous, HttpPost("login"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl = ReturnUrlPolicy.Canonicalize(model.ReturnUrl);
        if (!ModelState.IsValid) return BadRequestView(nameof(Login), model);

        var result = await auth.LoginAsync(new LoginRequest(model.Username, model.Password), HttpContext.RequestAborted);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Không thể đăng nhập. Kiểm tra thông tin hoặc trạng thái tài khoản");
            return View(model);
        }

        var user = await unitOfWork.Users.FindByIdAsync(result.Value!.Id, HttpContext.RequestAborted);
        if (user is null || user.Status != ECMA.Domain.Enums.UserStatus.Active) return View(model);
        var principal = AuthCookieService.CreatePrincipal(user.Id, user.Username, user.FullName, user.Role.ToString(), user.SecurityStamp, sessions.Start());
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return LocalRedirect(AuthorizedReturnUrl(model.ReturnUrl, user.Role.ToString()) ?? DefaultDestination(user.Role.ToString()));
    }

    [Authorize, HttpPost("logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        sessions.End(User.FindFirstValue("session_id"));
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [Authorize, HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var result = await auth.GetProfileAsync(CurrentUserId(), HttpContext.RequestAborted);
        if (!result.IsSuccess) return NotFound("Không tìm thấy dữ liệu");
        return View(new ProfileViewModel { Username = result.Value!.Username, FullName = result.Value.FullName, Role = result.Value.Role });
    }

    [Authorize, HttpPost("profile"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile([Bind(nameof(ProfileViewModel.FullName))] ProfileViewModel model)
    {
        var profile = await auth.GetProfileAsync(CurrentUserId(), HttpContext.RequestAborted);
        if (!profile.IsSuccess) return NotFound("Không tìm thấy dữ liệu");
        model = new ProfileViewModel
        {
            Username = profile.Value!.Username,
            FullName = model.FullName,
            Role = profile.Value.Role
        };

        if (!ModelState.IsValid) return BadRequestView(nameof(Profile), model);
        var result = await auth.UpdateProfileAsync(CurrentUserId(), model.FullName, HttpContext.RequestAborted);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.Error?.Code ?? nameof(model.FullName), result.Error?.Message ?? "Dữ liệu không hợp lệ");
            return BadRequestView(nameof(Profile), model);
        }
        TempData["SuccessMessage"] = "Cập nhật họ tên thành công";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize, HttpGet("change-password")]
    public IActionResult ChangePassword() => View(new ChangePasswordViewModel());

    [Authorize, HttpPost("change-password"), ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return BadRequestView(nameof(ChangePassword), model);
        var result = await auth.ChangePasswordAsync(CurrentUserId(),
            new ChangePasswordRequest(model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword),
            HttpContext.RequestAborted);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(result.Error?.Code ?? string.Empty, result.Error?.Message ?? "Dữ liệu không hợp lệ");
            return BadRequestView(nameof(ChangePassword), model);
        }
        sessions.End(User.FindFirstValue("session_id"));
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại";
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous, HttpGet("access-denied")]
    public IActionResult AccessDenied() => View();

    private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string DefaultDestination() => DefaultDestination(User.FindFirstValue(ClaimTypes.Role));
    private static string DefaultDestination(string? role) => role is "Admin" or "Organizer" ? "/events/manage" : "/";
    private static string? AuthorizedReturnUrl(string? returnUrl, string role) =>
        returnUrl is "/account/profile" or "/account/change-password" || returnUrl?.StartsWith("/account/profile?", StringComparison.Ordinal) == true ||
        returnUrl?.StartsWith("/account/change-password?", StringComparison.Ordinal) == true || role is "Attendee" or "Organizer" or "Admin"
            ? returnUrl
            : null;
    private ViewResult BadRequestView(string view, object model)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        return View(view, model);
    }
}