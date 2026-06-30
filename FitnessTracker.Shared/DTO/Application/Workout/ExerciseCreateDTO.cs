namespace FitnessTracker.Shared.DTO.Application.Workout
{
    public record ExerciseCreateDTO
    (
        string Name,
        List<SetCreateDTO> Sets
    );
}
