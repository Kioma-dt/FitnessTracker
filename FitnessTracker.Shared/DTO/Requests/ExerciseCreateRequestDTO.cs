using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record ExerciseCreateRequestDTO
    (
        [Required]
        [StringLength(128, MinimumLength = 3)]
        string Name,

        List<SetCreateRequestDTO> Sets
    );
}
