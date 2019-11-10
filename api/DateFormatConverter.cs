using Newtonsoft.Json.Converters;

namespace HD.Journally
{
  public class DateFormatConverter : IsoDateTimeConverter
  {
    public DateFormatConverter(string format)
    {
      DateTimeFormat = format;
    }
  }
}