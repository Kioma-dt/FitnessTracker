namespace FitnessTracker.Shared.Exceptions
{
    public class EntityAlreadyExistsException
        : Exception
    {
        public EntityAlreadyExistsException()
            : base()
        { }

        public EntityAlreadyExistsException(string message)
            : base(message)
        { }
    }
}
