namespace HD.BluJournal.DTOs
{
  public class UserDto
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public bool IsValid
    {
      get
      {
        return !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password);
      }
    }
  }
}