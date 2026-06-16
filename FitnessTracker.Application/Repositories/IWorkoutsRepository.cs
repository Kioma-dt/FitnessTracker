using FitnessTracker.Shared.DTO.Repositories;
namespace FitnessTracker.Application.Repositories
{
    public interface IWorkoutsRepository
    {
        Task<Workout?> GetByIdAsync(string id);
        Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId);
        Task AddAsync(Workout workout);
        Task UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO);
        Task DeleteAsync(string id);
        Task AddPhotoAsync(string id,  string photo);
        Task AddExerciseAsync(string id, Exercise exercise);
    }
}
