using FitnessTracker.Entities.Abstractions;

namespace FitnessTracker.Entities
{
    public class User
        : IDocument
    {
        public string? Id { get; set; }
        public DateTime CreatedAt { get;  set; }
        public string? Name { get; set; }
        public string? PasswordHash { get; set; }

        public List<Workout> Workouts { get; set; } = new();

        public User()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
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
