using FitnessTracker.Shared.DTO.Repositories;
using System.Linq.Expressions;
namespace FitnessTracker.Application.Repositories
{
    public interface IWorkoutsRepository
    {
        Task<Workout?> GetByIdAsync(string id);
        Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId, 
            Expression<Func<Workout, bool>>? filter = null);
        Task AddAsync(Workout workout);
        Task<Workout> UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO);
        Task DeleteAsync(string id);
        Task AddPhotoAsync(string id,  string photo);
        Task AddExerciseAsync(string id, Exercise exercise);
    }
}
