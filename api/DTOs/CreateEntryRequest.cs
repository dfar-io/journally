using System;
using Newtonsoft.Json;

namespace HD.Journally.DTOs
{
  public class CreateEntryRequest
  {
    [JsonRequired]
    public DateTime Date { get; set; }
    [JsonRequired]
    public string Content { get; set; }
  }
}