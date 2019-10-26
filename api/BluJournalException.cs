using System;
using System.Globalization;

namespace HD.BluJournal
{
  public class BluJournalException : Exception
  {
    public BluJournalException() : base() { }

    public BluJournalException(string message) : base(message) { }

    public BluJournalException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
  }
}