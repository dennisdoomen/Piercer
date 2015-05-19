using System;
using System.Linq;

namespace Piercer
{
    /// <summary>
    ///     Provides APIs for returning run-time information about the host process
    /// </summary>
    public class Diagnostics
    {
        /// <summary>
        ///     Returns all the run-time assemblies of the host process.
        /// </summary>
        public string[] GetAssemblies()
        {
            var query =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                select assembly.FullName;

            return query.ToArray();
        }
    }
}