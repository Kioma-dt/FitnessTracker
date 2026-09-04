using FitnessTracker.Application.Interfaces.Cache;
using FitnessTracker.Entities.Abstractions;

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
            string stamp;
            if (value is Document document)
            {
                stamp = document.UpdatedAt.ToString();
            }
            else
            {
                stamp = JsonSerializer.Serialize(value);
            }

            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(stamp)
            );
            
            return Convert.ToBase64String(bytes);
        }
    }
}
