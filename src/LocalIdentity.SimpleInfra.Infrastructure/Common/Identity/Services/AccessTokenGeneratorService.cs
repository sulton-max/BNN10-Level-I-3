﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Constants;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AccessTokenGeneratorService(IOptions<JwtSettings> jwtSettings) : IAccessTokenGeneratorService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public AccessToken GetToken(User user)
    {
        var accessToken = new AccessToken
        {
            Id = Guid.NewGuid()
        };
        var jwtToken = GetJwtToken(user, accessToken);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        accessToken.Token = token;

        return accessToken;
    }

    public Guid GetTokenId(string accessToken)
    {
        var tokenValue = accessToken.Split(' ')[1];
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenValue);
        var tokenId = token.Claims.FirstOrDefault(c => c.Type == ClaimConstants.AccessTokenId)?.Value;

        if (string.IsNullOrEmpty(tokenId))
            throw new ArgumentException("Invalid access token");

        return Guid.Parse(tokenId);
    }

    private JwtSecurityToken GetJwtToken(User user, AccessToken accessToken)
    {
        var claims = GetClaims(user, accessToken);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            signingCredentials: credentials
        );
    }

    private List<Claim> GetClaims(User user, AccessToken accessToken)
    {
        return new List<Claim>()
        {
            new(ClaimTypes.Email, user.EmailAddress),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimConstants.UserId, user.Id.ToString()),
            new(ClaimConstants.AccessTokenId, accessToken.Id.ToString()),
        };
    }
}