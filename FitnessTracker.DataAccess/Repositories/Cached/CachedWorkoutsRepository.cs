using System.Text;
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

    public async Task<IEnumerable<Workout>> GetAllByUserIdAsync(
        string userId, 
        int page = 1, 
        int pageSize = 10,
        IEnumerable<WorkoutFilterDTO>? filters = null,
        WorkoutOrderingDTO? ordeing = null)
    {
        var key = BuildRedisStringGetAll(userId, page, pageSize, filters, ordeing);
        
        var cached = await _cache.GetStringAsync(key);

        if (cached is not null)
        {
            return JsonSerializer.Deserialize<List<Workout>>(cached)
                ?? new List<Workout>();
        }
        
        var workouts = await _workoutsRepository.GetAllByUserIdAsync(
            userId, 
            page, 
            pageSize, 
            filters, 
            ordeing);

        await _cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(workouts));
        
        return workouts;
    }

    static string BuildRedisStringGetAll(
        string userId,
        int page,
        int pageSize,
        IEnumerable<WorkoutFilterDTO>? filters,
        WorkoutOrderingDTO? ordering)
    {
        var resultString = new StringBuilder("workouts");
        resultString.Append($":{userId}");
        resultString.Append($":page={page}");
        resultString.Append($":pageSize={pageSize}");

        if (filters is not null)
        {
            foreach (var filter in  filters)
            {
                resultString.Append($":{filter.FilterType}={filter.FilterValue}");
            }
        }

        if (ordering is not null)
        {
            resultString.Append($":orderBy={ordering.OrderBy}:descending={ordering.Descending}");
        }

        return resultString.ToString();
    }

    public async Task<int> GetTotalCountByUserAsync(
        string userId, 
        IEnumerable<WorkoutFilterDTO>? filters = null)
    {
        return await _workoutsRepository.GetTotalCountByUserAsync(userId, filters);
    }

    public async Task AddAsync(Workout workout)
    {
        await _workoutsRepository.AddAsync(workout);

        await _cache.RemoveAsync($"workout:{workout.Id}");
    }

    public async Task<Workout> UpdateAsync(
        string id, 
        WorkoutUpdateDTO workoutUpdateDTO)
    {
        var workout = await _workoutsRepository.UpdateAsync(id, workoutUpdateDTO);

        await _cache.RemoveAsync($"workout:{id}");

        return workout;
    }

    public async Task DeleteAsync(string id)
    {
        await _workoutsRepository.DeleteAsync(id);

        await _cache.RemoveAsync($"workout:{id}");
    }

    public async Task AddPhotoAsync(
        string id, 
        string photo)
    {
        await _workoutsRepository.AddPhotoAsync(id, photo);
        
        await _cache.RemoveAsync($"workout:{id}");
    }

    public async Task AddExerciseAsync(string id, Exercise exercise)
    {
        await _workoutsRepository.AddExerciseAsync(id, exercise);
        
        await _cache.RemoveAsync($"workout:{id}");
    }
}