namespace FitnessTracker.Application.Repositories
{
    public interface IWorkoutsRepository
    {
        Task<Workout?> GetByIdAsync(string id);
        Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId);
        Task Add(Workout workout);
        Task Update(Workout workout);
        Task Delete(string id);
        Task AddPhoto(string id,  string photo);
        Task AddExercise(string id, Exercise exercise);
    }
}
