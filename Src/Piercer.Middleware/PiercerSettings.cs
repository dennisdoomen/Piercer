namespace Piercer.Middleware
{
    public class PiercerSettings
    {
        public PiercerSettings()
        {
            Route = "/api";
        }

        public string Route { get; private set; }

        public PiercerSettings AtRoute(string route)
        {
            Route = route;
            return this;
        }
    }
}