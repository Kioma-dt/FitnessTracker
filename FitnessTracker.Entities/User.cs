using FitnessTracker.Entities.Abstractions;

namespace FitnessTracker.Entities
{
    public class User
        : Document
    {
        public string? Name { get; set; }
        public string? PasswordHash { get; set; }

        public List<Workout> Workouts { get; set; } = new();

        public User()
            : base()
        {
        }

        public User(
            string name, 
            string passwordHash)
            : this()
        {
            Name = name;
            PasswordHash = passwordHash;
        }
    }
}
