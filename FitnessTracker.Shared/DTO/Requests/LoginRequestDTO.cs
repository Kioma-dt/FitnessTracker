using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record LoginRequestDTO
    {
        [property: Required]
        [property: StringLength(128)]
        public string UserName { get; set; }

        [property: Required]
        [property: StringLength(128)]
        public string Password { get; set; }
    }
}
