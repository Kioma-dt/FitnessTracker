using System.Text.Json;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;
using Microsoft.Extensions.Caching.Distributed;

namespace FitnessTracker.DataAccess.Repositories.Cached;

public class CachedWorkoutsRepository
    : IWorkoutsRepository
{
    readonly WorkoutsRepository _workoutsRepository;
    readonly  IDistributedCache _cache;

    public CachedWorkoutsRepository(
        WorkoutsRepository workoutsRepository,
        IDistributedCache cache)
    {
        _workoutsRepository = workoutsRepository;
        _cache = cache;
    }
    
    public async Task<Workout?> GetByIdAsync(string id)
    {
        
        var cached = await _cache.GetStringAsync($"workout:{id}");

        if (cached is not null)
        {
            return JsonSerializer.Deserialize<Workout?>(cached);
        }
        
        var workout = await _workoutsRepository.GetByIdAsync(id);

        if (workout is not null)
        {
            await _cache.SetStringAsync(
                $"workout:{id}",
                JsonSerializer.Serialize(workout));
        }
        
        return workout;
    }

    public Task<IEnumerable<Workout>> GetAllByUserIdAsync(string userId, int page = 1, int pageSize = 10, IEnumerable<WorkoutFilterDTO>? filters = null,
        WorkoutOrderingDTO? ordeing = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalCountByUserAsync(string userId, IEnumerable<WorkoutFilterDTO>? filters = null)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Workout workout)
    {
        throw new NotImplementedException();
    }

    public Task<Workout> UpdateAsync(string id, WorkoutUpdateDTO workoutUpdateDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task AddPhotoAsync(string id, string photo)
    {
        throw new NotImplementedException();
    }

    public Task AddExerciseAsync(string id, Exercise exercise)
    {
        throw new NotImplementedException();
    }
}