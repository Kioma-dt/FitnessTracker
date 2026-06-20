using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests
{
    public record SetCreateRequestDTO
    {
        [Range(0, 2500d)]       // World record for weight is 2422 kg
        public double Weight { get; set; }

        [Range(1, 12000)]                 // World record for push ups in a row 10 507 
        public int Reps { get; set; }
    }
}
