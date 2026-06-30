namespace FitnessTracker.Shared.Exceptions.InternalServerError
{
    public class NotImplementedFunctionalityException
        : ApiException
    {
        public NotImplementedFunctionalityException(string details)
            : base(500, "Some functionality is currently not implemented", details)
        { }
    }
}
