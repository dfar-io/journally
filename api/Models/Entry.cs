using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HD.Journally.Models
{
  public class Entry
  {
    [Key]
    [JsonProperty("id")]
    public int EntryId { get; set; }

    [JsonProperty("date")]
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    [Required]
    public DateTime Date { get; set; }

    [JsonProperty("content")]
    [Required]
    public string Content { get; set; }

    [JsonIgnore]
    [Required]
    public int UserId { get; set; }

    [JsonIgnore]
    [Required]
    public User User { get; set; }
  }
}