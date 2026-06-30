namespace FitnessTracker.Shared.Exceptions.NotFound
{
    public class EntityNotFoundException
        : ApiException
    {
        public EntityNotFoundException(string details)
            : base(404, "No Entity", details)
        { }
    }
}
