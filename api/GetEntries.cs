using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HD.Journally.Models;
using System.Linq;
using System.Net;

namespace HD.Journally
{
  public class GetEntries
  {
    private readonly Context _context;
    public GetEntries(Context context)
    {
      _context = context;
    }

    [FunctionName("GetEntries")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "entries")]
        HttpRequest req)
    {
      if (req is null)
      {
        throw new System.ArgumentNullException(nameof(req));
      }

      var entries = _context.Entries.ToArray();
      return new OkObjectResult(entries);
    }
  }
}
