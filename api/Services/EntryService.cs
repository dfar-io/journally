using System;
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
      return await _context.Entries.Where(e => e.User == user)
                                   .OrderByDescending(e => e.DateTime)
                                   .ToListAsync();
    }

    public async Task<Entry> GetEntryByIdAsync(User user, int entryId)
    {
      return await _context.Entries.FirstOrDefaultAsync(
        e => e.User == user && e.EntryId == entryId);
    }

    public async Task UpdateEntryAsync(int entryId, Entry entry)
    {
      var oldEntry = _context.Entries.FirstOrDefault(e => e.EntryId == entryId);

      if (oldEntry == null)
      {
        throw new ArgumentException(
          $"Entry with id {entryId} not found when attempting update.");
      }

      oldEntry.Content = entry.Content;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteEntryAsync(int entryId)
    {
      var entry = _context.Entries.FirstOrDefault(e => e.EntryId == entryId);

      if (entry == null)
      {
        throw new ArgumentException(
          $"Entry with id {entryId} not found when attempting update.");
      }

      _context.Entries.Attach(entry);
      _context.Entries.Remove(entry);

      await _context.SaveChangesAsync();
    }

    public async Task AddEntryAsync(Entry entry)
    {
      await _context.Entries.AddAsync(entry);
      await _context.SaveChangesAsync();
    }
  }
}