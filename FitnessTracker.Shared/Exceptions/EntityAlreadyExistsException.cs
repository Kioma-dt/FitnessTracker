namespace FitnessTracker.Shared.Exceptions
{
    public class EntityAlreadyExistsException
        : ApiException
    {
        public EntityAlreadyExistsException(string details)
            : base(409, "Entity Exists", details)
        { }
    }
}
