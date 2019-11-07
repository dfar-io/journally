using System.Collections.Generic;
using HD.Journally.Models;
using Microsoft.AspNetCore.Http;

namespace HD.Journally.Services
{
  public interface ITokenService
  {
    string GenerateToken(User user);
    string GetEmailFromBearerToken(HttpRequest request);
  }
}