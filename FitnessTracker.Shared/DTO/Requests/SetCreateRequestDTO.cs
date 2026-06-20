using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record SetCreateRequestDTO
    (
        [Required]
        [Range(0, 2500d)]       // World record for weight is 2422 kg
        double Weight,

        [Required]
        [Range(1, 12000)]       // World record for push ups in a row 10 507 
        int Reps
    );
}
