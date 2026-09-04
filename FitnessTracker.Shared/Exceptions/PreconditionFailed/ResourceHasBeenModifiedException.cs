namespace FitnessTracker.Shared.Exceptions.PreconditionFailed;

public class ResourceHasBeenModifiedException
    : ApiException
{
    public ResourceHasBeenModifiedException(string details)
        : base(412, "Resource Has Been Modified", details)
    { }
}