using FitnessTracker.Shared.DTO;
using System.Linq.Expressions;
namespace FitnessTracker.Application.Repositories
{
    public interface IWorkoutsRepository
    {
        Task<Workout?> GetByIdAsync(string id);
        Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId, 
            int page = 1,
            int pageSize = 10,
            Expression<Func<Workout, bool>>? filter = null,
            string? orderBy = null,
            bool? descending = null);
        Task AddAsync(Workout workout);
        Task<Workout> UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO);
        Task DeleteAsync(string id);
        Task AddPhotoAsync(string id,  string photo);
        Task AddExerciseAsync(string id, Exercise exercise);
    }
}
