using System;
using HD.BluJournal.Models;
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
        Environment.GetEnvironmentVariable("SqlConnectionString");

      builder.Services.AddDbContext<Context>(
        options => options.UseSqlServer(SqlConnection)
      );
    }
  }
}