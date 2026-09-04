namespace FitnessTracker.Shared.Exceptions.PreconditionRequired;

public class NoIfMatchException
    : ApiException
{
    public NoIfMatchException(string details)
        : base(428, "No If Match", details)
    { }
}