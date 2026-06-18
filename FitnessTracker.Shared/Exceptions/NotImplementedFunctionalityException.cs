namespace FitnessTracker.Shared.Exceptions
{
    public class NotImplementedFunctionalityException
        : ApiException
    {
        public NotImplementedFunctionalityException(string details)
            : base(500, "Some functionality is currently not implemented", details)
        { }
    }
}
