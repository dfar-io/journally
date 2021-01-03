using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HD.Journally.Extensions;
using HD.Journally.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HD.Journally.Services
{
  public class TokenService : ITokenService
  {
    private readonly string JwtSecret;
    private readonly byte[] JwtKey;

    public TokenService()
    {
      JwtSecret = Environment.GetEnvironmentVariable(Constants.JwtSecretKey);

      if (JwtSecret == null)
      {
        throw new JournallyException(
          $"{Constants.JwtSecretKey} environment variable not configured.");
      }

      JwtKey = Encoding.ASCII.GetBytes(JwtSecret);
    }

    public string GenerateToken(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Email, user.Email)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(JwtKey),
          SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public string GetEmailFromBearerToken(HttpRequest request)
    {
      if (!request.Headers.ContainsKey("JournallyAuthorization") ||
          !request.Headers.TryGetValue("JournallyAuthorization", out var token) ||
          !token.First().Contains("Bearer"))
      {
        throw new JournallyException("No Bearer token provided.");
      }

      var accessToken = token.First().Split(" ", 2).Last();

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(JwtKey),
        ValidateIssuer = false,
        ValidateAudience = false
      };

      JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
      ClaimsPrincipal user;
      try
      {
        user = handler.ValidateToken(
        accessToken,
        tokenValidationParameters,
        out SecurityToken validatedToken);
      }
      catch
      {
        throw new JournallyException("Invalid Bearer token provided.");
      }

      return user.Claims.First().Value.ToString();
    }
  }
}