using System.Collections.Generic;
using HD.BluJournal.Models;

namespace HD.BluJournal.Services
{
  public interface IUserService
  {
    User Authenticate(string username, string password);
    IEnumerable<User> GetAll();
  }
}