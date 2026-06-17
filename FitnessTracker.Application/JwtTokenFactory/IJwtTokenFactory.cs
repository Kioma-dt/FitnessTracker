namespace FitnessTracker.Application.JwtTokenFactory
{
    public interface IJwtTokenFactory
    {
        string Create(User user);
    }
}
