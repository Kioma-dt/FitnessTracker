namespace FitnessTracker.Shared.Exceptions
{
    public class EntityAlreadyExistsException
        : ApiException
    {
        public EntityAlreadyExistsException(string details)
            : base(412, "Entity Exists", details)
        { }
    }
}
