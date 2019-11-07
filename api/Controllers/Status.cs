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
  public class Status
  {
    private readonly ITokenService _tokenService;

    public Status(ITokenService tokenService)
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

    [FunctionName("VerifyToken")]
    [RequestHttpHeader("Authorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "verifyToken")]
        HttpRequest req,
      ILogger log)
    {
      string authenticatedEmail;
      try
      {
        authenticatedEmail = _tokenService.GetEmailFromBearerToken(req);
      }
      catch (JournallyException ex)
      {
        log.LogWarning($"Authorization error when calling /verifyToken: {ex.Message}");
        return new UnauthorizedResult();
      }

      return new OkObjectResult($"Authenticated user is: {authenticatedEmail}");
    }
  }
}
