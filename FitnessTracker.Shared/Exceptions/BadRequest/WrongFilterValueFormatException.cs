namespace FitnessTracker.Shared.Exceptions.BadRequest
{
    public class WrongFilterValueFormatException
        : ApiException
    {
        public WrongFilterValueFormatException(string details)
            : base(400, "Format of filer parameter is wrong", details)
        { }
    }
}
