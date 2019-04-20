using System.Security.Principal;

namespace SystemPlus.Security
{
    /// <summary>
    /// Extensions and utilities for security related activities
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The name of the current windows user
        /// </summary>
        public static string FullUserName
        {
            get
            {
                using (WindowsIdentity id = WindowsIdentity.GetCurrent())
                {
                    return id.Name;
                }
            }
        }

        /// <summary>
        /// Is current user running with admin privileges
        /// </summary>
        public static bool IsAppRunningWithAdminprivileges()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                if (identity == null)
                    return false;

                WindowsPrincipal wp = new WindowsPrincipal(identity);

                return wp.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}