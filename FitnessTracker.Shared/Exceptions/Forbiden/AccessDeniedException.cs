namespace FitnessTracker.Shared.Exceptions.Forbiden
{
    public class AccessDeniedException
        : ApiException
    {
        public AccessDeniedException(string details)
            : base(403, "Forbiden", details)
        { }
    }
}
