using System;
using HD.BluJournal.Models;
using HD.BluJournal.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
    }
  }
}