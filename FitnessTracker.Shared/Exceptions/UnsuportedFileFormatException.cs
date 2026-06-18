namespace FitnessTracker.Shared.Exceptions
{
    public class UnsuportedFileFormatException
        : ApiException
    {
        public UnsuportedFileFormatException(string details)
            : base(415, "Wrong File Format", details)
        { }
    }
}
