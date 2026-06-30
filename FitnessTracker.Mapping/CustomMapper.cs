namespace FitnessTracker.Mapping
{
    public static class CustomMapperExtensions
    {
        public static WorkoutCreateDTO MapToWorkoutCreateDTO(this WorkoutPutRequestDTO request, string id, string? userId)
        {
            return new WorkoutCreateDTO(
                    id,
                    request.Title,
                    request.Type,
                    userId,
                    TimeSpan.FromMinutes(request.DurationInMinutes),
                    request.CaloriesBurned,
                    request.WorkoutDate,
                    request.Exercises.Select(ex => new ExerciseCreateDTO(
                        ex.Name,
                        ex.Sets.Select(s => new SetCreateDTO(
                            s.Weight,
                            s.Reps)).ToList())
                    ).ToList(),
                    request.ProgressPhotos); ;
        }
    }
}
