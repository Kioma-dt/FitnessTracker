using FitnessTracker.Shared.Enums;
using FitnessTracker.Shared.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutFiltersQueryDTO
        : IValidatableObject
        
    {
        //public WorkoutFiltersQueryDTO(DateTime? fromDate,
        //    DateTime? toDate,
        //    int? minDurationMinutes,
        //    int? maxDurationMinutes,
        //    WorkoutType? workoutType)
        //{
        //    FromDate = fromDate;
        //    ToDate = toDate;
        //    MinDurationMinutes = minDurationMinutes;
        //    MaxDurationMinutes = maxDurationMinutes;
        //    WorkoutType = workoutType;
        //}

        [NotFutureDate(ErrorMessage = "FromDate sould not be in future")]
        [UtcDate(ErrorMessage = "FromDate should be in UTC format")]
        public DateTime? FromDate { get; set; }

        [NotFutureDate(ErrorMessage = "ToDate sould not be in future")]
        [UtcDate(ErrorMessage = "FromDate should be in UTC format")]
        public DateTime? ToDate { get; set; }

        [Range(1, 1440, ErrorMessage = "MinDurationMinutes should be greater 0 and fits in 24 hours(less than 1440)")]
        public int? MinDurationMinutes { get; set; }

        [Range(1, 1440, ErrorMessage = "MaxDurationMinutes should be greater 0 and fits in 24 hours(less than 1440)")]
        public int? MaxDurationMinutes { get; set; }

        public WorkoutType? WorkoutType { get; set; }

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
                    WorkoutType.Value.ToString()));
            }

            return list;
        }
    }
}
