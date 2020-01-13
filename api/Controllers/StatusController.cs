using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using HD.Journally.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using System.Net;

namespace HD.Journally
{
  public class StatusController
  {
    private readonly ITokenService _tokenService;

    public StatusController(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [FunctionName("Status")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    public static IActionResult GetStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")]
        HttpRequest req)
    {
      if (req is null)
      {
        throw new ArgumentNullException(nameof(req));
      }

      return new OkObjectResult("API is functional.");
    }
  }
}
