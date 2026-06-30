namespace FitnessTracker.Application.Interfaces.Cache
{
    public interface IETagGenerator
    {
        string Generate(object value);
    }
}
