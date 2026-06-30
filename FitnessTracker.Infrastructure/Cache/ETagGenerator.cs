using FitnessTracker.Application.Interfaces.Cache;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FitnessTracker.Inrastructure.Cache
{
    public class ETagGenerator
        : IETagGenerator
    {
        public string Generate(object value)
        {
            var json = JsonSerializer.Serialize(value);

            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(json)
            );
            
            return Convert.ToBase64String(bytes);
        }
    }
}
