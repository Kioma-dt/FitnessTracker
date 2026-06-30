namespace FitnessTracker.Application.Interfaces
{
    public interface IJwtTokenFactory
    {
        string Create(User user);
    }
}
