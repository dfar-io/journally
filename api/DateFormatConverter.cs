using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HD.BluJournal.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace HD.BluJournal
{
  public class DateFormatConverter : IsoDateTimeConverter
  {
    public DateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }
}
