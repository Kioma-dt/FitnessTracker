using System.Text.Json;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.Options.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace FitnessTracker.DataAccess.Repositories.Cached;

public class CachedUsersRepository
    : IUsersRepository
{
    readonly UsersRepository _usersRepository;
    readonly RedisTTLOptions _ttlOptions;
    readonly  IDistributedCache _cache;
    
    public CachedUsersRepository(
        UsersRepository usersRepository,
        IOptions<RedisTTLOptions> ttlOptions,
        IDistributedCache cache)
    {
        _usersRepository = usersRepository;
        _cache = cache;
        _ttlOptions = ttlOptions.Value;
    }
    
    public async Task AddAsync(User user)
    {
        await _usersRepository.AddAsync(user);

        await _cache.RemoveAsync($"user:{user.Id}");
        await _cache.RemoveAsync($"user:name={user.Name}");
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        var cached = await _cache.GetStringAsync($"user:name={name}");

        if (cached is not null)
        {
            return JsonSerializer.Deserialize<User?>(cached);
        }
        
        var user = await _usersRepository.GetByNameAsync(name);

        if (user is not null)
        {
            await _cache.SetStringAsync(
                $"user:name={name}",
                JsonSerializer.Serialize(user),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_ttlOptions.UserSeconds)
                });
        }
        
        return user;
    }
}