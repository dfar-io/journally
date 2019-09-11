using System.Collections.Generic;
using System.Linq;
using HD.BluJournal.Models;

namespace HD.BluJournal.Services
{ }
public class UserService : IUserService
{
  // users hardcoded for simplicity, store in a db with hashed passwords in production applications
  private List<User> _users = new List<User>
        {
            new User { Id = 1, Email = "test@test.com", Password = "test" }
        };

  private readonly AppSettings _appSettings;

  public UserService(IOptions<AppSettings> appSettings)
  {
    _appSettings = appSettings.Value;
  }

  public User Authenticate(string email, string password)
  {
    var user = _users.SingleOrDefault(x => x.Email == email && x.Password == password);

    if (user == null) { return null; }

    // authentication successful so generate jwt token
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
        {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
        }),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    user.Token = tokenHandler.WriteToken(token);

    // remove password before returning
    user.Password = null;

    return user;
  }

  public IEnumerable<User> GetAll()
  {
    // return users without passwords
    return _users.Select(x =>
    {
      x.Password = null;
      return x;
    });
  }
}
}