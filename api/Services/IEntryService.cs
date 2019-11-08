using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HD.Journally.Models;

namespace HD.Journally.Services
{
  public interface IEntryService
  {
    Task<IEnumerable<Entry>> GetEntriesFromUserAsync(User user);
  }
}