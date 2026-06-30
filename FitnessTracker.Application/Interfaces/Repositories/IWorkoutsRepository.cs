using FitnessTracker.Shared.DTO;
using System.Linq.Expressions;
namespace FitnessTracker.Application.Interfaces.Repositories
{
    public interface IWorkoutsRepository
    {
        Task<Workout?> GetByIdAsync(string id);
        Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId, 
            int page = 1,
            int pageSize = 10,
            IEnumerable<WorkoutFilterDTO>? filters = null,
            WorkoutOrderingDTO? ordeing = null);
        Task<int> GetTotalCountByUserAsync(string userId,
            IEnumerable<WorkoutFilterDTO>? filters = null);
        Task AddAsync(Workout workout);
        Task<Workout> UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO);
        Task DeleteAsync(string id);
        Task AddPhotoAsync(string id,  string photo);
        Task AddExerciseAsync(string id, Exercise exercise);
    }
}
