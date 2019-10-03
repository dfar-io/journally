using System.Reflection;
using AzureFunctions.Extensions.Swashbuckle;
using HD.BluJournal;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(SwashBuckleStartup))]
namespace HD.BluJournal
{
  internal class SwashBuckleStartup : IWebJobsStartup
  {
    public void Configure(IWebJobsBuilder builder)
    {
      builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
    }
  }
}