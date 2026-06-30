namespace FitnessTracker.Shared.Exceptions.UnsuportedMediaFormat
{
    public class UnsuportedFileFormatException
        : ApiException
    {
        public UnsuportedFileFormatException(string details)
            : base(415, "Wrong File Format", details)
        { }
    }
}
