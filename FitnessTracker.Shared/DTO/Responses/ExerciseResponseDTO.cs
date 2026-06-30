namespace FitnessTracker.Shared.DTO.Responses
{
    public record ExerciseResponseDTO
    (
        string Name,
        List<SetResponseDTO> Sets
    );
}
