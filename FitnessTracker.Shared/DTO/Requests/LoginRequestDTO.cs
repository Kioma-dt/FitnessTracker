using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record LoginRequestDTO
    (
        [Required]
        [StringLength(128, MinimumLength = 3)]
        string UserName,

        [Required]
        [StringLength(128, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
                            ErrorMessage = "Password should contain lowercase, uppercase, speciacl characters and digit")]
        string Password
    );
}
