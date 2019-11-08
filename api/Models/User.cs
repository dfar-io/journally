using System.Collections.Generic;

namespace HD.Journally.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public List<Entry> Entries { get; set; }
  }
}