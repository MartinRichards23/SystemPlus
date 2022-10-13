using System.Text.RegularExpressions;

namespace SystemPlus.Net.Mail
{
    /// <summary>
    /// Tools for manipulating email addresses
    /// </summary>
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

            return email[(index + 1)..];
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

            Regex emailRegex = new Regex(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,64}$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }
    }
}
