using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Requests.Workout
{
    public record ExerciseCreateRequestDTO
    {
        public ExerciseCreateRequestDTO(
            string name, 
            List<SetCreateRequestDTO> sets)
        {
            Name = name;
            Sets = sets;
        }

        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Exercise should contain at least 1 set")]
        [MaxLength(500)]
        public List<SetCreateRequestDTO> Sets { get; set; }
    };
}
