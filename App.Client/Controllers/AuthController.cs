using App.Client.Models;
using App.Client.Services.Auth;
using App.Client.Services.Token;
using App.Shared.Dto.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Client.Controllers;

public class AuthController
    (IAuthService authService, 
    ITokenService tokenService,
    IMapper mapper)
    : Controller
{
    [HttpGet("/Login")]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost("/Login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
            return View(loginViewModel);

        var loginDto = mapper.Map<LoginDto>(loginViewModel);
        var token = await authService.LoginAsync(loginDto);

        tokenService.SetRefreshToken(token.RefreshToken);
        tokenService.SetAccessToken(token.AccessToken);

        TempData["SuccessMessage"] = "Login successfully!";

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/Register")]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost("/Register")]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        var registerDto = mapper.Map<RegisterDto>(registerViewModel);

        var token = await authService.RegisterAsync(registerDto);
        tokenService.SetRefreshToken(token.RefreshToken);
        tokenService.SetAccessToken(token.AccessToken);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/Logout")]
    public IActionResult Logout()
    {
        // Çerezleri silmek için süresini geçmiş bir tarihe ayarlıyoruz. 
        Response.Cookies.Append("access_token", "", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1), 
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
        });

        Response.Cookies.Append("refresh_token", "", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1), 
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
        });

        TempData["SuccessMessage"] = "Logout successfully!";

        return RedirectToAction("Index", "Home");
    }


    [HttpGet("/ForgotPassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("/ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
    {
        if (!ModelState.IsValid)
            return View(forgotPasswordViewModel);

        await authService.ForgotPasswordAsync(forgotPasswordViewModel.Email);

        TempData["SuccessMessage"] = "Please check your email to reset password!";

        return RedirectToAction("Index", "Home");
    }
}
