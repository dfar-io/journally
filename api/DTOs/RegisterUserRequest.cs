using Newtonsoft.Json;

namespace HD.BluJournal.DTOs
{
  public class RegisterUserRequest
  {
    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
    public string Password { get; set; }
  }
}