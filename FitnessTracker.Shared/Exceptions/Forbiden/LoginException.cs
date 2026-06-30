namespace FitnessTracker.Shared.Exceptions.Forbiden
{
    public class LoginException
        : ApiException
    {
        public LoginException(string details)
            : base(403, "Can't Log In", details)
        { }
    }
}
