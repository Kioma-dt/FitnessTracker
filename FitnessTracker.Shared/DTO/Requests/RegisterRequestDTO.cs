using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record RegisterRequestDTO
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
