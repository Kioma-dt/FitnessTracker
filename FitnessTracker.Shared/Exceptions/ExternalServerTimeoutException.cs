namespace FitnessTracker.Shared.Exceptions
{
    public class ExternalServerTimeoutException
        : ApiException
    {
        public ExternalServerTimeoutException(string details)
            : base(504, "Errors in photo storage server", details)
        { }
    }
}
