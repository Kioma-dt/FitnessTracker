namespace FitnessTracker.Shared.DTO.Application.User
{
    public record UserTokenDTO
    (
        string Token,
        UserDTO User
    );
}
