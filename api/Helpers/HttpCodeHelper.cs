using Microsoft.AspNetCore.Mvc;

namespace HD.BluJournal.Helpers
{
  public static class HttpCodeHelper
  {
    public static IActionResult InvalidPayload()
    {
      return Return400("Entry returned is invalid, make sure "
        + "payload is constructed correctly.");
    }

    public static IActionResult EmptyPOSTBody()
    {
      return Return400("No content included in request.");
    }

    public static IActionResult Return400(string message)
    {
      return new BadRequestObjectResult(message);
    }
  }
}