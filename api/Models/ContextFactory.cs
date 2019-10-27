using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HD.BluJournal.Models
{
  public class ContextFactory : IDesignTimeDbContextFactory<Context>
  {
    public Context CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<Context>();
      optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("BLUJOURNAL_CONN_STR"));

      return new Context(optionsBuilder.Options);
    }
  }
}