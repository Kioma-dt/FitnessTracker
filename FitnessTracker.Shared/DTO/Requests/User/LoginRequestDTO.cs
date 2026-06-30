using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests.User
{
    public record LoginRequestDTO
    {
        public LoginRequestDTO(
            string userName,
            string password)
        {
            UserName = userName;
            Password = password;
        }

        [property: Required]
        [property: StringLength(128)]
        public string UserName { get; set; }

        [property: Required]
        [property: StringLength(128)]
        public string Password { get; set; }
    }
}
