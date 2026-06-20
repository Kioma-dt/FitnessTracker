using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record ExerciseCreateRequestDTO(
        [property: Required]
        [property: StringLength(128, MinimumLength = 1)]
        string Name,

        [property: Required]
        [property: MinLength(1, ErrorMessage = "Exercise should contain at least 1 set")]
        [property: MaxLength(500)]
        List<SetCreateRequestDTO> Sets
    );
}
