using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SystemPlus.Net.Mail
{
    /// <summary>
    /// Tools for manipulating email addresses
    /// </summary>
    [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
    public static class EmailTools
    {
        /// <summary>
        /// Gets the part of email address after @
        /// </summary>
        public static string GetEmailDomain(string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

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
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return uri.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks is a string is a valid email address
        /// </summary>
        public static bool VerifyEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
}
