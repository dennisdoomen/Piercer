using System;
using System.Linq;
using System.Web.Http;

namespace Piercer.Middleware
{
    /// <summary>
    ///     Provides APIs for returning run-time information about the host process
    /// </summary>
    [RoutePrefix(("piercer"))]
    public class PiercerController : ApiController
    {
        /// <summary>
        ///     Returns all the run-time assemblies of the host process.
        /// </summary>
        [Route("assemblies")]
        [HttpGet]
        public string[] GetAssemblies()
        {
            var query =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                select assembly.FullName;

            return query.ToArray();
        }
    }
}