using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HD.Journally.Helpers
{
  public static class HttpCodeHelper
  {
    public static IActionResult InvalidPayload()
    {
      return Return400("Entry returned is invalid, make sure "
        + "payload is constructed correctly.");
    }

    public static IActionResult EmptyRequestBody()
    {
      return Return400("No content included in request.");
    }

    public static IActionResult Return400(string message)
    {
      return new BadRequestObjectResult(message);
    }

    public static IActionResult Return500()
    {
      return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
  }
}