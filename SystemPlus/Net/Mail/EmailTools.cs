using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemPlus.Net.Mail
{
    public static class EmailTools
    {
        /// <summary>
        /// Gets the part of email address after @
        /// </summary>
        public static string GetEmailDomain(string email)
        {
            int index = email.IndexOf("@", StringComparison.Ordinal);

            if (index < 0)
                throw new ArgumentException("Email does not contain '@'");

            return email.Substring(index + 1);
        }

        /// <summary>
        /// Is uri a mailto: uri
        /// </summary>
        public static bool IsEmailUri(string uri)
        {
            return uri.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks is a string is a valid email address
        /// </summary>
        public static bool VerifyEmail(string email)
        {
            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }

    }
}
