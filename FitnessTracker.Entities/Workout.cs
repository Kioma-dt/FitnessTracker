using FitnessTracker.Entities.Abstractions;
using FitnessTracker.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Entities
{
    public class Workout 
        : Document
    {
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
            :base()
        {
        }

        public Workout(
            string userId, 
            string title,
            WorkoutType type,
            TimeSpan duration, 
            int caloriesBurned, 
            DateTime workoutDate, 
            List<Exercise>? exercises = null, 
            List<string>? progressPhotos = null)
            : this()
        {
            UserId = userId;
            Title = title;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            WorkoutDate = workoutDate.ToUniversalTime();
            Exercises = exercises ?? Exercises;
            ProgressPhotos = progressPhotos ?? ProgressPhotos;
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

        public Exercise(
            string name,
            List<Set>? sets = null)
            :this()
        {
            Name = name;
            Sets = sets ?? Sets;
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
}
