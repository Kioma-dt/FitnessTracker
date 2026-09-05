namespace FitnessTracker.Shared.Options.Redis;

public class RedisTTLOptions
{
    public int WorkoutSeconds { get; set; }
    public int WorkoutsCollectionSeconds { get; set; }
    
    public int UserSeconds { get; set; }
}