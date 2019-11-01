using System;
using HD.BluJournal.Models;
using HD.BluJournal.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(HD.BluJournal.Startup))]

namespace HD.BluJournal
{
  class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      string SqlConnection =
        Environment.GetEnvironmentVariable("BLUJOURNAL_CONN_STR");

      builder.Services.AddDbContext<Context>(
        options => options.UseSqlServer(SqlConnection)
      );

      // dependency injection
      builder.Services.AddScoped<IUserService, UserService>();

      // sets all JSON payload properties to lowercase
      builder.Services.AddMvcCore()
                      .AddJsonOptions(
                        options => options.SerializerSettings.ContractResolver =
                        new LowercaseContractResolver())
                      .AddJsonFormatters();
    }
  }
}