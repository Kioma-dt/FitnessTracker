namespace FitnessTracker.Shared.Exceptions.UnpocessableContent
{
    public class UnprocessableImageException
        : ApiException
    {
        public UnprocessableImageException(string details)
            : base(422, "Image is unprocessable", details)
        { }
    }
}
