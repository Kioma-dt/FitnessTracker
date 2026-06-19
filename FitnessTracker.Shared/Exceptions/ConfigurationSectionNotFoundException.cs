namespace FitnessTracker.Shared.Exceptions
{
    public class ConfigurationSectionNotFoundException
        : ApiException
    {

        public ConfigurationSectionNotFoundException(string details)
            : base(500, "Configuration Section not set on server", details)
        { }
    }
}
