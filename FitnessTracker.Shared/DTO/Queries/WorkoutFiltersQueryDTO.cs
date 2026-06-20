using FitnessTracker.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutFiltersQueryDTO(
            
            DateTime? FromDate,
            DateTime? ToDate,

            [Range(0, 1440)]
            int? MinDurationMinutes,

            [Range(0, 1440)]
            int? MaxDurationMinutes,

            WorkoutType? WorkoutType
        )
        : IValidatableObject
        
    {
        public List<WorkoutFilterDTO> ToList()
        {
            var list = new List<WorkoutFilterDTO>();

            if(FromDate.HasValue)
            {
                list.Add(new WorkoutFilterDTO(
                    WorkoutFilterType.FromDate,
                    FromDate.Value.ToUniversalTime().ToString("O")));
            }

            if (ToDate.HasValue)
            {
                list.Add(new WorkoutFilterDTO(
                    WorkoutFilterType.ToDate, 
                    ToDate.Value.ToUniversalTime().ToString("O")));
            }

            if (MinDurationMinutes.HasValue)
            {
                list.Add(new WorkoutFilterDTO(
                    WorkoutFilterType.MinDuration, 
                    TimeSpan.FromMinutes(MinDurationMinutes.Value).ToString()));
            }

            if (MaxDurationMinutes.HasValue)
            {
                list.Add(new WorkoutFilterDTO(
                    WorkoutFilterType.MaxDuration,
                    TimeSpan.FromMinutes(MaxDurationMinutes.Value).ToString()));
            }

            if (WorkoutType.HasValue)
            {
                list.Add(new WorkoutFilterDTO(
                    WorkoutFilterType.WorkoutType,
                    WorkoutType.ToString()));
            }

            return list;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
            {
                yield return new ValidationResult(
                    "FromDate must be less than or equal to ToDate",
                    new[] { nameof(FromDate), nameof(ToDate) });
            }

            if (MinDurationMinutes.HasValue && MaxDurationMinutes.HasValue && MinDurationMinutes > MaxDurationMinutes)
            {
                yield return new ValidationResult(
                    "MinDurationMinutes must be less than or equal to MaxDuration",
                    new[] { nameof(MinDurationMinutes), nameof(MaxDurationMinutes) });
            }
        }
    }
}
