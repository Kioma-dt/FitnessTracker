using FitnessTracker.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record RegisterRequestDTO
    (
        [property: Required]
        [property: StringLength(128, MinimumLength = 3)]
        [property: NoWhiteSpaces(ErrorMessage = "UserName must not contain whitespaces(replce them with underscores or smth else)")]
        string UserName,

        [property: Required]
        [property: StringLength(128, MinimumLength = 6)]
        [property: NoWhiteSpaces(ErrorMessage = "Password must not contain whitespaces(replce them with underscores or smth else)")]
        [property: RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
                            ErrorMessage = "Password should contain lowercase, uppercase, speciacl characters and digit")]
        string Password
    );
}
