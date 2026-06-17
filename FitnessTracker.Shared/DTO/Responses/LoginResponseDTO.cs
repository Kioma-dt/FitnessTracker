namespace FitnessTracker.Shared.DTO.Responses
{
    public record LoginResponseDTO
    (
        string Token,
        string UserId,
        string UserName
    );
}
