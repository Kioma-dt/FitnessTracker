namespace FitnessTracker.Shared.Exceptions.BadRequest
{
    public class WrongWorkoutPageFormat
        : ApiException
    {
        public WrongWorkoutPageFormat(string details)
            : base(400, "Format of page parameter is wrong", details)
        { }
    }
}
