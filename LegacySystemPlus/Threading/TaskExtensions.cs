using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus.Threading
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Allows an async function to be called and not awaited without the warning.
        /// </summary>
        public static void DoNotAwait(this Task task) { }
    }
}
