using System;
using HD.Journally.Models;
using HD.Journally.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
      ConfigureDatabase(builder);
      SetDependencyInjection(builder);

      var jwtSecret = Environment.GetEnvironmentVariable(Constants.JwtSecretKey);
      if (string.IsNullOrWhiteSpace(jwtSecret))
      {
        throw new ArgumentNullException(
          $"Environment variable {Constants.JwtSecretKey} not set.");
      }

      builder.Services.AddCors(options =>
        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
      );
    }

    private void ConfigureDatabase(IFunctionsHostBuilder builder)
    {
      string SqlConnection =
                    Environment.GetEnvironmentVariable(
                      Constants.ConnectionStringKey);

      if (SqlConnection == null)
      {
        throw new ArgumentNullException(
          $"Environment variable {Constants.ConnectionStringKey} not set.");
      }

      builder.Services.AddDbContext<Context>(
        options => options.UseSqlServer(SqlConnection));

      var optionsBuilder = new DbContextOptionsBuilder<Context>();
      optionsBuilder.UseSqlServer(SqlConnection);

      using var context = new Context(optionsBuilder.Options);
      try
      {
        context.Database.Migrate();
      }
      catch (Exception e)
      {
        throw new Exception(
          $"Error when migrating database: {e.Message}");
      }
    }

    private void SetDependencyInjection(IFunctionsHostBuilder builder)
    {
      builder.Services.AddScoped<ITokenService, TokenService>();
      builder.Services.AddScoped<IUserService, UserService>();
      builder.Services.AddScoped<IEntryService, EntryService>();
    }
  }
}