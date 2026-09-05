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
        var version = await _cache.GetStringAsync($"workouts:{userId}:version");

        if (version is null)
        {
            version = "1";
            await _cache.SetStringAsync($"workouts:{userId}:version", version);
        }
        
        var key = BuildRedisKeyGetAll(userId, page, pageSize, filters, ordeing, version);
        
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

    public async Task<int> GetTotalCountByUserAsync(
        string userId, 
        IEnumerable<WorkoutFilterDTO>? filters = null)
    {
        var version = await _cache.GetStringAsync($"workouts:{userId}:version");

        if (version is null)
        {
            version = "1";
            await _cache.SetStringAsync($"workouts:{userId}:version", version);
        }
        
        var key = BuildRedisKeyGetTotalCount(userId, filters, version);
        
        var cached = await _cache.GetStringAsync(key);

        if (cached is not null)
        {
            return int.Parse(cached);
        }

        var count = await _workoutsRepository.GetTotalCountByUserAsync(userId, filters);
        
        await _cache.SetStringAsync(
            key,
            count.ToString());
        
        return count;
    }

    public async Task AddAsync(Workout workout)
    {
        await _workoutsRepository.AddAsync(workout);

        await _cache.RemoveAsync($"workout:{workout.Id}");

        var version = await _cache.GetStringAsync($"workouts:{workout.UserId}:version");
        version = version is not null ? (int.Parse(version) + 1).ToString() : "1";
        await _cache.SetStringAsync($"workouts:{workout.UserId}:version", version);
    }

    public async Task<Workout> UpdateAsync(
        string id, 
        WorkoutUpdateDTO workoutUpdateDTO)
    {
        var workout = await _workoutsRepository.UpdateAsync(id, workoutUpdateDTO);

        await _cache.RemoveAsync($"workout:{id}");
        
        var version = await _cache.GetStringAsync($"workouts:{workout.UserId}:version");
        version = version is not null ? (int.Parse(version) + 1).ToString() : "1";
        await _cache.SetStringAsync($"workouts:{workout.UserId}:version", version);

        return workout;
    }

    public async Task DeleteAsync(string id)
    {
        var workout = await _workoutsRepository.GetByIdAsync(id);
        await _workoutsRepository.DeleteAsync(id);

        await _cache.RemoveAsync($"workout:{id}");
        
        var version = await _cache.GetStringAsync($"workouts:{workout!.UserId}:version");
        version = version is not null ? (int.Parse(version) + 1).ToString() : "1";
        await _cache.SetStringAsync($"workouts:{workout!.UserId}:version", version);
    }

    public async Task AddPhotoAsync(
        string id, 
        string photo)
    {
        var workout = await _workoutsRepository.GetByIdAsync(id);
        await _workoutsRepository.AddPhotoAsync(id, photo);
        
        await _cache.RemoveAsync($"workout:{id}");
        
        var version = await _cache.GetStringAsync($"workouts:{workout!.UserId}:version");
        version = version is not null ? (int.Parse(version) + 1).ToString() : "1";
        await _cache.SetStringAsync($"workouts:{workout!.UserId}:version", version);
    }

    public async Task AddExerciseAsync(string id, Exercise exercise)
    {
        var workout = await _workoutsRepository.GetByIdAsync(id);
        await _workoutsRepository.AddExerciseAsync(id, exercise);
        
        await _cache.RemoveAsync($"workout:{id}");
        var version = await _cache.GetStringAsync($"workouts:{workout!.UserId}:version");
        version = version is not null ? (int.Parse(version) + 1).ToString() : "1";
        await _cache.SetStringAsync($"workouts:{workout!.UserId}:version", version);
    }
    
    
    static string BuildRedisKeyGetAll(
        string userId,
        int page,
        int pageSize,
        IEnumerable<WorkoutFilterDTO>? filters,
        WorkoutOrderingDTO? ordering,
        string version)
    {
        var resultString = new StringBuilder("workouts");
        resultString.Append($":{userId}");
        resultString.Append($":v{version}");
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
    
    static string BuildRedisKeyGetTotalCount(
        string userId,
        IEnumerable<WorkoutFilterDTO>? filters,
        string version)
    {
        var resultString = new StringBuilder("workouts");
        resultString.Append($":{userId}");
        resultString.Append($":v{version}");

        if (filters is not null)
        {
            foreach (var filter in  filters)
            {
                resultString.Append($":{filter.FilterType}={filter.FilterValue}");
            }
        }
        
        return resultString.ToString();
    }
}