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

namespace HD.BluJournal
{
  public static class AuthenticateUser
  {
    [FunctionName("AuthenticateUser")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "users/authenticate")] HttpRequest req,
        ILogger log)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      UserDto data = JsonConvert.DeserializeObject<UserDto>(requestBody);
      if (!data.IsValid)
        return HttpCodeHelper.InvalidPayload();

      log.LogInformation("Authenticate passed in user.");

      return (ActionResult)new OkObjectResult(new
      {
        Email = data.Email,
        Token = "testToken"
      });
    }
  }
}
