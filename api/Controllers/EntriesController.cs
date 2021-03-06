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
  [ApiController]
  public class EntryController
  {
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IEntryService _entryService;
    public EntryController(
      ITokenService tokenService,
      IUserService userService,
      IEntryService entryService)
    {
      _tokenService = tokenService;
      _userService = userService;
      _entryService = entryService;
    }

    [FunctionName("GetEntries")]
    [RequestHttpHeader("JournallyAuthorization", isRequired: true)]
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

    [FunctionName("GetEntry")]
    [RequestHttpHeader("JournallyAuthorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Entry))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetEntry(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "entries/{entryId}")]
        HttpRequest req,
        ILogger log,
        int entryId)
    {
      string authenticatedEmail;
      try
      {
        authenticatedEmail = _tokenService.GetEmailFromBearerToken(req);
      }
      catch (JournallyException ex)
      {
        log.LogWarning(
          $"Authorization error when calling /entries/{entryId}: {ex.Message}");
        return new UnauthorizedResult();
      }

      var user = await _userService.GetByEmailAsync(authenticatedEmail);
      var entry = await _entryService.GetEntryByIdAsync(user, entryId);
      return new OkObjectResult(entry);
    }

    [FunctionName("UpdateEntry")]
    [RequestHttpHeader("JournallyAuthorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Put(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "entries/{entryId}")]
        [RequestBodyType(typeof(UpdateEntryRequest), "UpdateEntryRequest")]
        HttpRequest req,
        ILogger log,
        int entryId)
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
        return HttpCodeHelper.EmptyRequestBody();

      User user = await _userService.GetByEmailAsync(authenticatedEmail);
      Entry entry = await _entryService.GetEntryByIdAsync(user, entryId);

      if (entry == null)
      {
        return new NotFoundResult();
      }

      string requestBody;
      using (StreamReader readStream = new StreamReader(req.Body))
      {
        requestBody = await readStream.ReadToEndAsync();
      }

      Entry data;
      try
      {
        data = JsonConvert.DeserializeObject<Entry>(requestBody);
      }
      catch (Exception e)
      {
        return HttpCodeHelper.Return400(e.Message);
      }

      if (data.DateTime == null)
      {
        return HttpCodeHelper.Return400("Date provided cannot be null.");
      }

      if (data.Content == null)
      {
        return HttpCodeHelper.Return400("Content provided cannot be null.");
      }

      await _entryService.UpdateEntryAsync(entryId, data);

      return new NoContentResult();
    }

    [FunctionName("CreateEntry")]
    [RequestHttpHeader("JournallyAuthorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Entry))]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Post(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "entries")]
        [RequestBodyType(typeof(CreateEntryRequest), "CreateEntryRequest")]
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
        return HttpCodeHelper.EmptyRequestBody();

      string requestBody;
      using (StreamReader readStream = new StreamReader(req.Body))
      {
        requestBody = await readStream.ReadToEndAsync();
      }

      Entry data;
      try
      {
        data = JsonConvert.DeserializeObject<Entry>(requestBody);
      }
      catch (Exception e)
      {
        return HttpCodeHelper.Return400(e.Message);
      }

      if (data.DateTime == null)
      {
        return HttpCodeHelper.Return400("Date provided cannot be null.");
      }

      if (data.Content == null)
      {
        return HttpCodeHelper.Return400("Content provided cannot be null.");
      }

      var user = await _userService.GetByEmailAsync(authenticatedEmail);
      data.UserId = user.Id;

      await _entryService.AddEntryAsync(data);

      return new CreatedResult("https://example.com/api/entries/201", data);
    }

    [FunctionName("DeleteEntry")]
    [RequestHttpHeader("JournallyAuthorization", isRequired: true)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Delete(
        [HttpTrigger(
          AuthorizationLevel.Anonymous,
          "delete",
          Route = "entries/{entryId}")]
        HttpRequest req,
        ILogger log,
        int entryId)
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

      User user = await _userService.GetByEmailAsync(authenticatedEmail);
      Entry entry = await _entryService.GetEntryByIdAsync(user, entryId);

      if (entry == null)
      {
        return new NotFoundResult();
      }

      await _entryService.DeleteEntryAsync(entryId);

      return new NoContentResult();
    }
  }
}
