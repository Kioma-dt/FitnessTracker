using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests.User
{
    public record RegisterRequestDTO
    {
        public RegisterRequestDTO(
            string userName, 
            string password)
        {
            UserName = userName;
            Password = password;
        }

        [Required]
        [StringLength(128, MinimumLength = 3)]
        [NoWhiteSpaces(ErrorMessage = "UserName should not contain whitespaces(replce them with underscores or smth else)")]
        public string UserName { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 6)]
        [NoWhiteSpaces(ErrorMessage = "Password should not contain whitespaces(replce them with underscores or smth else)")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
                            ErrorMessage = "Password should contain lowercase, uppercase, speciacl characters and digit")]
        public string Password { get; set; }
    }
}
