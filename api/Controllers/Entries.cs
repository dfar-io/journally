using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using HD.Journally.Models;
using System.Linq;
using System.Threading.Tasks;
using HD.Journally.Helpers;
using System.IO;
using Newtonsoft.Json;
using HD.Journally.Services;
using Microsoft.Extensions.Logging;

namespace HD.Journally.Controllers
{
  public class Entries
  {
    private readonly Context _context;
    private readonly ITokenService _tokenService;
    public Entries(Context context, ITokenService tokenService)
    {
      _context = context;
      _tokenService = tokenService;
    }

    [FunctionName("GetEntries")]
    public IActionResult Get(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "entries")]
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
        log.LogWarning($"Authorization error when calling /entries: {ex.Message}");
        return new UnauthorizedResult();
      }

      var entries = _context.Entries.ToArray();
      return new OkObjectResult(entries);
    }

    [FunctionName("PostEntry")]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "entries")]
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
        log.LogWarning($"Authorization error when calling /entries: {ex.Message}");
        return new UnauthorizedResult();
      }

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
