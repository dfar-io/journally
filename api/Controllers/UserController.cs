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
using System.Net;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HD.Journally.DTOs;

namespace HD.Journally.Controllers
{
  public class UserController
  {
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
      _userService = userService;
      _tokenService = tokenService;
    }

    [FunctionName("AuthenticateUser")]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(AuthenticateUserResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
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
        return HttpCodeHelper.EmptyRequestBody();

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

      User user;
      try
      {
        user = _userService.Authenticate(payload.Email, payload.Password);
      }
      catch (JournallyException ex)
      {
        return HttpCodeHelper.Return400(ex.Message);
      }

      var tokenString = _tokenService.GenerateToken(user);

      var response = new AuthenticateUserResponse
      {
        Email = user.Email,
        Token = tokenString
      };

      return new OkObjectResult(response);
    }

    [FunctionName("RegisterUser")]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(RegisterUserResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Register(
        [HttpTrigger(
          AuthorizationLevel.Anonymous,
          "post",
          Route = "user")]
        [RequestBodyType(typeof(RegisterUserRequest), "Register User Request")]
        HttpRequest req)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyRequestBody();

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
