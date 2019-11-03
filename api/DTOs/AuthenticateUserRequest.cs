using Newtonsoft.Json;

namespace HD.BluJournal.DTOs
{
  public class AuthenticateUserRequest
  {
    [JsonRequired]
    public string Email { get; set; }
    [JsonRequired]
    public string Password { get; set; }
  }
}