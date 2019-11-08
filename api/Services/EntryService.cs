using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HD.Journally.Models;
using Microsoft.EntityFrameworkCore;

namespace HD.Journally.Services
{
  public class EntryService : IEntryService
  {
    private readonly Context _context;

    public EntryService(Context context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Entry>> GetEntriesFromUserAsync(User user)
    {
      return await _context.Entries.Where(e => e.User == user).ToListAsync();
    }
  }
}