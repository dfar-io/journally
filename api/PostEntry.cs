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
using System.Linq;
using HD.BluJournal.Helpers;

namespace HD.BluJournal
{
  public class PostEntry
  {
    private readonly Context _context;
    public PostEntry(Context context)
    {
      _context = context;
    }

    [FunctionName("PostEntry")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "entries")]
            HttpRequest req,
        ILogger log)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      Entry data = JsonConvert.DeserializeObject<Entry>(requestBody);
      if (!data.IsValid)
      {
        return HttpCodeHelper.InvalidPayload();
      }
      var date = data.Date.ToString("yyyy-MM-dd");

      var existingEntry = _context.Entries
        .Where(e => e.Date == data.Date).FirstOrDefault();
      if (existingEntry == null)
      {
        await _context.Entries.AddAsync(data);
      }
      else
      {
        existingEntry.Content = data.Content;
      }

      await _context.SaveChangesAsync();
      var id = existingEntry == null ? data.EntryId : existingEntry.EntryId;

      return new CreatedResult("https://example.com/api/entries/201", new
      {
        id = id,
        date = date,
        content = data.Content
      });
    }
  }
}
