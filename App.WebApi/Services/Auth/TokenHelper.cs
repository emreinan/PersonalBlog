﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Data.Contexts;
using App.WebApi.Model;
using App.Data.Entities;

namespace App.WebApi.Services.Auth;

public class TokenHelper(IConfiguration configuration, AppDbContext authDbContext)
{
    public  string CreateAccessToken(User user)
    {

        var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()
            ?? throw new InvalidOperationException("TokenOptions cant found in configuration");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("IsActive", user.IsActive.ToString()),
            new Claim("ProfilePhotoUrl", user.ProfilePhotoUrl?.ToString() ?? string.Empty)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(tokenOptions.AccessTokenExpiration);

        var jwt = new JwtSecurityToken(
            tokenOptions.Issuer,
            tokenOptions.Audience,
            expires: expires,
            notBefore: DateTime.UtcNow,
            claims: claims,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(jwt);
        return accessToken;
    }

    public  async Task<RefreshToken> CreateRefreshTokenAsync(User user)
    {
        string refreshToken = Guid.NewGuid().ToString();
        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["TokenOptions:RefreshTokenExpiration"])),
        };

        await authDbContext.RefreshTokens.AddAsync(newRefreshToken);
        await authDbContext.SaveChangesAsync();
        return newRefreshToken;
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken)
    {
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.ReasonRevoked = "Replaced by new token";
        var newRefreshToken = await CreateRefreshTokenAsync(user);
        await authDbContext.SaveChangesAsync();
        return newRefreshToken;
    }

    public async Task RevokedOldRefreshTokens(User user)
    {
        user.RefreshTokens.ToList().ForEach(x => x.Revoked = DateTime.Now);
        await authDbContext.SaveChangesAsync();
    }


}
