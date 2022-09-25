using System;
using System.Text.RegularExpressions;

namespace TripYatri.Core.Base.Constants
{
    public static class RegExConstants
    {
        public const string EmailPattern = @"^[a-z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-z0-9-]+(?:\.[a-z0-9-]+)*$";
        public static readonly Regex Email = new Regex(
            EmailPattern,
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromSeconds(5));

        public const string GuidPattern = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";

        public static readonly Regex Guid = new Regex(
            GuidPattern,
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromSeconds(5));

        public const string AccountIdPattern = @"^a[a-z0-9]{17,19}$";
        public static readonly Regex AccountId = new Regex(
            AccountIdPattern,
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromSeconds(5));

        public const string DatePattern =
            @"\d{4}-[01]{1}\d{1}-[0-3]{1}\d{1}T[0-2]{1}\d{1}:[0-6]{1}\d{1}:[0-6]{1}\d{1}[+|-][0-1][0-9]:[0-5][0-9]$";
    }
}