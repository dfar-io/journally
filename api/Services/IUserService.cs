using System.Collections.Generic;
using System.Threading.Tasks;
using HD.Journally.Models;

namespace HD.Journally.Services
{
  public interface IUserService
  {
    User Authenticate(string email, string password);
    User Create(User user, string password);
    Task<User> GetByEmailAsync(string email);
  }
}