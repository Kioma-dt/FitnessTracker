namespace FitnessTracker.Shared.Exceptions
{
    public class UnprocessableImageException
        : ApiException
    {
        public UnprocessableImageException(string details)
            : base(422, "Image is unprocessable", details)
        { }
    }
}
