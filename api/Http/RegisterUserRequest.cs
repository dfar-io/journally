using Newtonsoft.Json;

namespace HD.BluJournal.Http
{
  public class RegisterUserRequest
  {
    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
    public string Password { get; set; }
  }
}