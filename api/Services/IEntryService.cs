using System.Collections.Generic;
using System.Threading.Tasks;
using HD.Journally.Models;

namespace HD.Journally.Services
{
  public interface IEntryService
  {
    Task<IEnumerable<Entry>> GetEntriesFromUserAsync(User user);

    Task<Entry> GetEntryByIdAsync(User user, int entryId);

    Task UpdateEntryAsync(int entryId, Entry entry);

    Task DeleteEntryAsync(int entryId);

    Task AddEntryAsync(Entry entry);
  }
}