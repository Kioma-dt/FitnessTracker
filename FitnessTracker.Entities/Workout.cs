using FitnessTracker.Entities.Abstractions;

namespace FitnessTracker.Entities
{
    public class Workout 
        : IDocument
    {
        public string? Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public string? Title { get; set; }
        public WorkoutType Type { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public DateTime WorkoutDate { get; set; }

        public List<Exercise> Exercises { get; set; } = new();
        public List<string> ProgressPhotos { get; set; } = new();

        public Workout()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public Workout(string userId, 
            string title,
            WorkoutType type,
            TimeSpan duration, 
            int caloriesBurned, 
            DateTime workoutDate, 
            List<Exercise> exercises, 
            List<string> progressPhotos)
            : this()
        {
            UserId = userId;
            Title = title;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            WorkoutDate = workoutDate;
            Exercises = exercises;
            ProgressPhotos = progressPhotos;
        }

        public void AddPhoto(string photo)
        {
            ProgressPhotos.Add(photo);
        }

        public void AddExercise(Exercise exercise)
        {
            Exercises.Add(exercise);
        }
    }

    public class Exercise
    {
        public string? Name { get; set; }
        public List<Set> Sets { get; set; } = new();

        public Exercise() { }

        public Exercise(string name, List<Set> sets)
            :this()
        {
            Name = name;
            Sets = sets;
        }
    }

    public class Set
    {
        public int Reps { get; set; }
        public double Weight { get; set; }

        public Set() { }

        public Set(int reps, double weight)
        {
            Reps = reps;
            Weight = weight;
        }
    }

    public enum WorkoutType
    {
        Strength,
        Cardio,
        Flexibility,
        HIIT,
        CrossFit
    }
}
