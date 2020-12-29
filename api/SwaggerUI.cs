using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "")]
            HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
    {
      return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "json"));
    }
  }
}