using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HD.BluJournal.Models
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
    public bool IsValid
    {
      get
      {
        return Date != DateTime.MinValue && !String.IsNullOrWhiteSpace(Content);
      }
    }
  }
}