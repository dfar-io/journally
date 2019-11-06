using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HD.Journally.Helpers;
using HD.Journally.Services;
using HD.Journally.Models;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HD.Journally.DTOs;

namespace HD.Journally.Controllers
{
  public class Users
  {
    private readonly IUserService _userService;

    public Users(IUserService userService)
    {
      _userService = userService;
    }

    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(AuthenticateUserResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    [FunctionName("AuthenticateUser")]
    public async Task<IActionResult> Authenticate(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "user/authenticate")]
        [RequestBodyType(typeof(AuthenticateUserRequest), "Authenticate User Request")]
          HttpRequest req,
        ILogger log)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody;
      using (StreamReader readStream = new StreamReader(req.Body))
      {
        requestBody = await readStream.ReadToEndAsync();
      }

      RegisterUserRequest payload;

      try
      {
        payload = JsonConvert.DeserializeObject<RegisterUserRequest>(requestBody);
      }
      catch (Exception e)
      {
        return HttpCodeHelper.Return400(e.Message);
      }

      var user = _userService.Authenticate(payload.Email, payload.Password);

      if (user == null)
        return HttpCodeHelper.Return400("Username or password is incorrect");

      var tokenHandler = new JwtSecurityTokenHandler();
      var secret = Environment.GetEnvironmentVariable("Journally_JWT_SECRET");
      if (secret == null)
      {
        log.LogError(
          "Journally_JWT_SECRET environment variable not configured.");
        HttpCodeHelper.Return500();
      }
      var key = Encoding.ASCII.GetBytes(secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
            new Claim(ClaimTypes.Email, user.Email)
          }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      var response = new AuthenticateUserResponse
      {
        Email = user.Email,
        Token = tokenString
      };

      return new OkObjectResult(response);
    }

    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(RegisterUserResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    [FunctionName("RegisterUser")]
    public async Task<IActionResult> Register(
        [HttpTrigger(
          AuthorizationLevel.Anonymous,
          "post",
          Route = "user")]
        [RequestBodyType(typeof(RegisterUserRequest), "Register User Request")]
        HttpRequest req)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody;
      using (StreamReader readStream = new StreamReader(req.Body))
      {
        requestBody = await readStream.ReadToEndAsync();
      }

      RegisterUserRequest payload;

      try
      {
        payload = JsonConvert.DeserializeObject<RegisterUserRequest>(requestBody);
      }
      catch (Exception e)
      {
        return HttpCodeHelper.Return400(e.Message);
      }

      var newUser = new User
      {
        Email = payload.Email
      };

      try
      {
        _userService.Create(newUser, payload.Password);
      }
      catch (JournallyException ex)
      {
        return HttpCodeHelper.Return400(ex.Message);
      }

      var response = new RegisterUserResponse
      {
        Email = newUser.Email
      };

      return new CreatedResult("https://example.com/api/entries/201", response);
    }
  }
}
