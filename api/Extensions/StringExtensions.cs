using System;
using System.Net.Mail;

namespace HD.BluJournal.Extensions
{
  public static class StringExtensions
  {
    public static bool IsValidEmail(this string str)
    {
      try
      {
        MailAddress m = new MailAddress(str);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }
  }
}