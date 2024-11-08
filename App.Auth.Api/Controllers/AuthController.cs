﻿using App.Auth.Api.Services;
using App.Shared.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await authService.LoginAsync(loginDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await authService.RegisterAsync(registerDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery]VerifyEmailDto verifyEmailDto)
        {
            var result = await authService.VerifyEmailAsync(verifyEmailDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await authService.ForgotPasswordAsync(forgotPasswordRequest);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var result = await authService.ResetPasswordAsync(resetPasswordRequest);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await authService.RefrehsTokenAsync(refreshTokenRequest);
            if (!result.IsSuccess)
                return BadRequest(result);
            var dto = result.Value;
            return Ok(dto);
        }

        private Guid GetUserId()
        {
            if (!User.Identity.IsAuthenticated)
                return Guid.Empty;
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

    }
}
