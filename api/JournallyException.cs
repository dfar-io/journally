using System;
using System.Globalization;

namespace HD.Journally
{
  public class JournallyException : Exception
  {
    public JournallyException() : base() { }

    public JournallyException(string message) : base(message) { }

    public JournallyException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
  }
}