using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HD.BluJournal.DTOs;
using HD.BluJournal.Helpers;
using HD.BluJournal.Services;
using HD.BluJournal.Models;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace HD.BluJournal
{
  public class Users
  {
    private readonly IUserService _userService;

    public Users(IUserService userService)
    {
      _userService = userService;
    }

    [FunctionName("AuthenticateUser")]
    public async Task<IActionResult> Authenticate(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "user/authenticate")] HttpRequest req,
        ILogger log)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      UserDto data = JsonConvert.DeserializeObject<UserDto>(requestBody);
      if (!data.IsValid)
        return HttpCodeHelper.InvalidPayload();

      var user = _userService.Authenticate(data.Email, data.Password);

      if (user == null)
        return HttpCodeHelper.Return400("Username or password is incorrect");

      var tokenHandler = new JwtSecurityTokenHandler();
      var secret = Environment.GetEnvironmentVariable("BLUJOURNAL_JWT_SECRET");
      if (secret == null)
      {
        log.LogError(
          "BLUJOURNAL_JWT_SECRET environment variable not configured.");
        HttpCodeHelper.Return500();
      }
      var key = Encoding.ASCII.GetBytes(secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Name, user.Id.ToString())
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      return new OkObjectResult(new
      {
        Email = user.Email,
        Token = tokenString
      });
    }

    [FunctionName("RegisterUser")]
    public async Task<IActionResult> Register(
        [HttpTrigger(
          AuthorizationLevel.Anonymous,
          "post",
          Route = "user")]
        HttpRequest req,
        ILogger log)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      UserDto data = JsonConvert.DeserializeObject<UserDto>(requestBody);

      if (!data.IsValid)
        return HttpCodeHelper.InvalidPayload();

      var newUser = new User();
      newUser.Email = data.Email;

      try
      {
        _userService.Create(newUser, data.Password);
      }
      catch (BluJournalException ex)
      {
        return HttpCodeHelper.Return400(ex.Message);
      }

      return new CreatedResult("https://example.com/api/entries/201", new
      {
        id = newUser.Id,
        email = newUser.Email
      });
    }
  }
}
