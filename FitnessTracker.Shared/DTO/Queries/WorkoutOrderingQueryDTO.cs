using FitnessTracker.Shared.Enums;

namespace FitnessTracker.Shared.DTO.Queries
{
    public record WorkoutOrderingQueryDTO
    {
        //public WorkoutOrderingQueryDTO(WorkoutOrderingType? orderBy,
        //    bool? descending)
        //{
        //    OrderBy = orderBy;
        //    Descending = descending;
        //}

        public WorkoutOrderingType? OrderBy { get; set; }
        public bool? Descending { get; set; }
    }
}
