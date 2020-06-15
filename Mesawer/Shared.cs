using System;

namespace Mesawer
{
    internal static class Shared
    {
        public const string Url = "https://mesawer-terminal.weebly.com";
        public const string WebApi = "https://mesawer-terminal.getsandbox.com/version";
        public const string DownloadUrl = Url + "/download.html";

        public const char DataSeparator = ';';
        public const string Version = "2.1";
        public const string InvalidSyntax = "Invalid Syntax.\nCheck out the documentation for more info on " + Url;
        public const string UnknownArguments = "Unknown Arugments. Make sure that you typed the right command";

        public static bool Contains(this string str, string[] substrings)
        {

            foreach (var cmp in substrings)
            {
                if (cmp == null)
                {
                    throw new ArgumentNullException($"{nameof(substrings)} cannot be null.");
                }

                if (str.Contains(cmp))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var first = str.Substring(0, 1).ToUpper();
            if (str.Length == 1) return first;

            return first + str.ToLower().Substring(1);
        }

        public static bool IsNullOrWhitespace(this string str)
        {
            return str == null || str.Trim().Equals(string.Empty);
        }
    }
}
