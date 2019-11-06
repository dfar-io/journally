using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace HD.Journally
{
  public class Status
  {
    [FunctionName("Status")]
    public static IActionResult GetStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")]
        HttpRequest req)
    {
      if (req is null)
      {
        throw new System.ArgumentNullException(nameof(req));
      }

      return new OkObjectResult("API is functional.");
    }
  }
}
