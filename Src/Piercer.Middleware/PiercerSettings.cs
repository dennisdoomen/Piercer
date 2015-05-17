using System.Collections.Generic;

namespace Piercer.Middleware
{
    public class PiercerSettings
    {
        private readonly List<string> ignoredAssemblyNames = new List<string>();

        public PiercerSettings()
        {
            Route = "/api";
        }

        public string Route { get; private set; }

        public List<string> IgnoredAssemblyNames
        {
            get { return ignoredAssemblyNames; }
        }

        public PiercerSettings AtRoute(string route)
        {
            Route = route;
            return this;
        }

        public PiercerSettings Ignoring(string assemblyName)
        {
            ignoredAssemblyNames.Add(assemblyName);
            return this;
        }
    }
}