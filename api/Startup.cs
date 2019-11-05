using System;
using HD.Journally.Models;
using HD.Journally.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(HD.Journally.Startup))]

namespace HD.Journally
{
  class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      string SqlConnection =
        Environment.GetEnvironmentVariable(Constants.ConnectionStringKey);

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