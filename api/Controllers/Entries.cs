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
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using System.Net;
using System;
using HD.Journally.DTOs;

namespace HD.Journally.Controllers
{
  public class Entries
  {
    private readonly Context _context;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IEntryService _entryService;
    public Entries(
      Context context,
      ITokenService tokenService,
      IUserService userService,
      IEntryService entryService)
    {
      _context = context;
      _tokenService = tokenService;
      _userService = userService;
      _entryService = entryService;
    }

    [FunctionName("GetEntries")]
    [RequestHttpHeader("Authorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Entry[]))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Get(
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
        log.LogWarning(
          $"Authorization error when calling /entries: {ex.Message}");
        return new UnauthorizedResult();
      }

      var user = await _userService.GetByEmailAsync(authenticatedEmail);
      var entries = await _entryService.GetEntriesFromUserAsync(user);
      return new OkObjectResult(entries);
    }

    [FunctionName("PostEntry")]
    [RequestHttpHeader("Authorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Entry))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "entries")]
        [RequestBodyType(typeof(PostEntryRequest), "Post Entry Request")]
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

      Entry data = null;
      try
      {
        data = JsonConvert.DeserializeObject<Entry>(requestBody);
      }
      catch (Exception e)
      {
        HttpCodeHelper.Return400(e.Message);
      }

      var user = await _userService.GetByEmailAsync(authenticatedEmail);
      data.UserId = user.Id;

      await _context.Entries.AddAsync(data);
      await _context.SaveChangesAsync();

      return new CreatedResult("https://example.com/api/entries/201", new
      {
        data.EntryId,
        data.Date,
        data.Content
      });
    }
  }
}
