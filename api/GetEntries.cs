using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HD.BluJournal.Models;
using System.Linq;
using System.Net;

namespace HD.BluJournal
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
        HttpRequest req,
        ILogger log)
    {
      var entries = _context.Entries.ToArray();
      return new OkObjectResult(entries);
    }
  }
}
