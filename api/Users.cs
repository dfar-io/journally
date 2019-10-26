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

      _userService.Authenticate(data.Email, data.Password);

      return (ActionResult)new OkObjectResult(new
      {
        Email = data.Email,
        Token = "testToken"
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
