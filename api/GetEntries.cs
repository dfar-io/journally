using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HD.BluJournal.Models;
using System.Collections.Generic;

namespace HD.BluJournal
{
  public static class GetEntries
  {
    [FunctionName("GetEntries")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "entries")]
        HttpRequest req,
        ILogger log)
    {
      var entries = new List<Entry>();
      for (var i = 0; i <= 5; i++)
      {
        var entry = new Entry();
        entry.Id = i;
        entry.Date = DateTime.Now.AddDays(i);
        entry.Content = "Testing content from the API.";
        entries.Add(entry);
      }

      var jsonOutput = JsonConvert.SerializeObject(entries, Formatting.Indented);
      return new OkObjectResult(jsonOutput);
    }
  }
}
