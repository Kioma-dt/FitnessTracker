namespace FitnessTracker.Application.Interfaces.Authentication
{
    public interface IJwtTokenFactory
    {
        string Create(User user);
    }
}
