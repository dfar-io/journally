using System;
using System.Collections.Generic;
using System.Linq;
using HD.Journally.Extensions;
using HD.Journally.Models;
using Microsoft.Extensions.Logging;

namespace HD.Journally.Services
{
  public class UserService : IUserService
  {
    private readonly Context _context;

    public UserService(Context context)
    {
      _context = context;
    }

    public User Authenticate(string email, string password)
    {
      if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        return null;

      var user = _context.Users.SingleOrDefault(x => x.Email == email);

      if (user == null)
        return null;

      if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        return null;

      return user;
    }

    public User Create(User user, string password)
    {
      if (!user.Email.IsValidEmail())
        throw new JournallyException("Invalid email provided.");

      if (string.IsNullOrWhiteSpace(password))
        throw new JournallyException("Password required.");

      if (_context.Users.Any(x => x.Email == user.Email))
        throw new JournallyException(
          "Email \"" + user.Email + "\" is already taken.");

      CreatePasswordHash(
        password,
        out byte[] passwordHash,
        out byte[] passwordSalt);

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
      if (password == null)
        throw new ArgumentNullException("password");

      if (string.IsNullOrWhiteSpace(password))
      {
        throw new ArgumentException(
          "Value cannot be empty or whitespace only string.", "password");
      }

      if (password.Length < 8)
      {
        throw new ArgumentException(
          "Value must be 8 or more characters.", "password");
      }

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash =
          hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }

    }

    private bool VerifyPasswordHash(
      string password,
      byte[] passwordHash,
      byte[] passwordSalt)
    {
      if (password == null) throw new ArgumentNullException("password");
      if (string.IsNullOrWhiteSpace(password))
      {
        throw new ArgumentException(
          "Value cannot be empty or whitespace only string.", "password");
      }
      if (passwordHash.Length != 64)
      {
        throw new ArgumentException(
          "Invalid length of password hash (64 bytes expected).",
          "passwordHash");
      }

      if (passwordSalt.Length != 128)
      {
        throw new ArgumentException(
          "Invalid length of password salt (128 bytes expected).",
          "passwordHash");
      }

      using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
      {
        var computedHash =
          hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != passwordHash[i]) return false;
        }
      }

      return true;
    }
  }
}