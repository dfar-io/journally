using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HD.Journally.Models
{
  public class ContextFactory : IDesignTimeDbContextFactory<Context>
  {
    public Context CreateDbContext(string[] args)
    {
      string SqlConnection =
        Environment.GetEnvironmentVariable(Constants.ConnectionStringKey);

      if (SqlConnection == null)
      {
        throw new ArgumentNullException(
          $"Environment variable {Constants.ConnectionStringKey} not set.");
      }

      var optionsBuilder = new DbContextOptionsBuilder<Context>();
      optionsBuilder.UseSqlServer(SqlConnection);

      return new Context(optionsBuilder.Options);
    }
  }
}