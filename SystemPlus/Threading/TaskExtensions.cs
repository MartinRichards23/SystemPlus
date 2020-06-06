using System.Threading.Tasks;

namespace SystemPlus.Threading
{
    /// <summary>
    /// Extensions for System.Threading.Tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Allows an async function to be called and not awaited without the warning.
        /// </summary>
        public static void DoNotAwait(this Task _) { }
    }
}
