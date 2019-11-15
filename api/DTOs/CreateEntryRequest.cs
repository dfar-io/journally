using System;
using Newtonsoft.Json;

namespace HD.Journally.DTOs
{
  public class CreateEntryRequest
  {
    [JsonRequired]
    public DateTime DateTime { get; set; }
    [JsonRequired]
    public string Content { get; set; }
    public string Title { get; set; }
  }
}