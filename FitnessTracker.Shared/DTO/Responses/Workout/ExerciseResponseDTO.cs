namespace FitnessTracker.Shared.DTO.Responses.Workout
{
    public record ExerciseResponseDTO
    (
        string Name,
        List<SetResponseDTO> Sets
    );
}
