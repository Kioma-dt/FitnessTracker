namespace FitnessTracker.Shared.Exceptions.InternalServerError
{
    public class PhotoStorageException
        : ApiException
    {
        public PhotoStorageException(string details)
            : base(502, "Errors in photo storage server", details)
        { }
    }
}
