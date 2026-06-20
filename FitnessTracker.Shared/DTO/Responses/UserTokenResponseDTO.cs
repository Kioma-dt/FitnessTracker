namespace FitnessTracker.Shared.DTO.Responses
{
    public record UserTokenResponseDTO
    (
        string Token,
        UserResponseDTO User
    );
}
