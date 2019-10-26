using HD.BluJournal.DTOs;

namespace HD.BluJournal.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
  }
}