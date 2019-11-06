using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using HD.Journally.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HD.Journally.Helpers;
using System.IO;
using Newtonsoft.Json;

namespace HD.Journally.Controllers
{
  public class Entries
  {
    private readonly Context _context;
    public Entries(Context context)
    {
      _context = context;
    }


    [FunctionName("GetEntries")]
    public IActionResult Get(
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

    [FunctionName("PostEntry")]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "entries")]
            HttpRequest req)
    {
      if (req.ContentLength <= 0)
        return HttpCodeHelper.EmptyPOSTBody();

      string requestBody;
      using (StreamReader readStream = new StreamReader(req.Body))
      {
        requestBody = await readStream.ReadToEndAsync();
      }

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
        id,
        date,
        content = data.Content
      });
    }
  }
}
