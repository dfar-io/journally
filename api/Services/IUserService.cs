using System.Collections.Generic;
using HD.BluJournal.Models;

namespace HD.BluJournal.Services
{
  public interface IUserService
  {
    User Authenticate(string email, string password);
    User Create(User user, string password);
  }
}