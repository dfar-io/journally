using Newtonsoft.Json;

namespace HD.Journally.DTOs
{
  public class RegisterUserRequest
  {
    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
    public string Password { get; set; }
  }
}