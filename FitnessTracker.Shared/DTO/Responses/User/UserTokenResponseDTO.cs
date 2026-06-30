namespace FitnessTracker.Shared.DTO.Responses.User
{
    public record UserTokenResponseDTO
    (
        string Token,
        UserResponseDTO User
    );
}
