namespace FitnessTracker.Shared.Exceptions
{
    public class ExternalServerAccessException
        : ApiException
    {
        public ExternalServerAccessException(string details)
            : base(500, "Errors while attempting to access internal api", details)
        { }
    }
}
