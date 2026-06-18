namespace FitnessTracker.Shared.Exceptions
{
    public class WrongFilterValueFormatException
        : ApiException
    {
        public WrongFilterValueFormatException(string details)
            : base(400, "Format of query parametr is wrong", details)
        { }
    }
}
