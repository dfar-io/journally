using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using System.Net.Http;
using AzureFunctions.Extensions.Swashbuckle;

namespace HD.Journally
{
    public static class SwaggerUI
  {
    [FunctionName("SwaggerUI")]
    [SwaggerIgnore]
    public static Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger")]
            HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
    {
      return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "json"));
    }
  }
}