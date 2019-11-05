using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HD.Journally.Models
{
  public class ContextFactory : IDesignTimeDbContextFactory<Context>
  {
    public Context CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<Context>();
      optionsBuilder.UseSqlServer(
        Environment.GetEnvironmentVariable(Constants.ConnectionStringKey));

      return new Context(optionsBuilder.Options);
    }
  }
}