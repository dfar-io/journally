using System;
using Newtonsoft.Json;

namespace HD.Journally.DTOs
{
  public class UpdateEntryRequest
  {
    [JsonRequired]
    public string Content { get; set; }
  }
}