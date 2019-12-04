using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HD.Journally.Models
{
  public class Entry
  {
    [Key]
    [JsonProperty("id")]
    public int EntryId { get; set; }

    [JsonProperty("datetime")]
    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    public string Content { get; set; }

    public string Title { get; set; }

    [JsonIgnore]
    [Required]
    public int UserId { get; set; }

    [JsonIgnore]
    [Required]
    public User User { get; set; }
  }
}