
using Microsoft.EntityFrameworkCore;

namespace HD.Journally.Models
{
  public class Context : DbContext
  {
    public Context(DbContextOptions<Context> options)
      : base(options)
    { }

    public DbSet<Entry> Entries { get; set; }
    public DbSet<User> Users { get; set; }
  }
}