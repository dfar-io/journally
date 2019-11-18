using System;
using HD.Journally.Models;
using HD.Journally.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: FunctionsStartup(typeof(HD.Journally.Startup))]

namespace HD.Journally
{
  class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      string SqlConnection =
        Environment.GetEnvironmentVariable(Constants.ConnectionStringKey);

      if (SqlConnection == null)
      {
        throw new ArgumentNullException(
          $"Environment variable {Constants.ConnectionStringKey} not set.");
      }

      // add EF context
      builder.Services.AddDbContext<Context>(
        options => options.UseSqlServer(SqlConnection)
      );

      // dependency injection
      builder.Services.AddScoped<ITokenService, TokenService>();
      builder.Services.AddScoped<IUserService, UserService>();
      builder.Services.AddScoped<IEntryService, EntryService>();

      // sets all JSON payload properties to
      //   lowercase
      //   hide null values from payloads
      builder.Services.AddMvcCore()
                      .AddJsonOptions(
                        options =>
                        {
                          options.SerializerSettings.ContractResolver =
                            new LowercaseContractResolver();
                          options.SerializerSettings.NullValueHandling =
                            NullValueHandling.Ignore;
                        }
                      )
                      .AddJsonFormatters();
    }
  }
}