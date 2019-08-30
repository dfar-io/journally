using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HD.BluJournal.Models
{
  public class Entry
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("date")]
    [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
    public DateTime Date { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
  }
}