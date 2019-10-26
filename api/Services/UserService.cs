using System;
using System.Collections.Generic;
using System.Linq;
using HD.BluJournal.Models;
using Microsoft.Extensions.Logging;

namespace HD.BluJournal.Services
{
  public class UserService : IUserService
  {
    private readonly Context _context;

    public UserService(Context context)
    {
      _context = context;
    }

    public User Authenticate(string username, string password)
    {
      var authenticatedUser = new User();
      authenticatedUser.Email = "authUser@test.com";
      return authenticatedUser;
    }

    public User Create(User user, string password)
    {
      if (string.IsNullOrWhiteSpace(password))
        throw new BluJournalException("Password required.");

      if (_context.Users.Any(x => x.Email == user.Email))
        throw new BluJournalException(
          "Email \"" + user.Email + "\" is already taken.");

      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);

      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;

      _context.Users.Add(user);
      _context.SaveChanges();

      return user;
    }

    private static void CreatePasswordHash(
      string password,
      out byte[] passwordHash,
      out byte[] passwordSalt)
    {
      if (password == null) throw new ArgumentNullException("password");
      if (string.IsNullOrWhiteSpace(password))
      {
        throw new ArgumentException(
          "Value cannot be empty or whitespace only string.", "password");
      }

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash =
          hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}