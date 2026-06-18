namespace FitnessTracker.Shared.Exceptions
{
    public class UnsuportedPhotoFormatException
        : ApiException
    {
        public UnsuportedPhotoFormatException(string details)
            : base(415, "Wrong Photo Format", details)
        { }
    }
}
