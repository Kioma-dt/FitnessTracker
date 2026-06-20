using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record LoginRequestDTO
    (
        [property: Required]
        [property: StringLength(128)]
        string UserName,

        [property: Required]
        [property:StringLength(128)]
        string Password
    );
}
