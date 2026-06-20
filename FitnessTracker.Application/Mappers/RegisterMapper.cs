using FitnessTracker.Shared.DTO.Responses;
using Mapster;

namespace FitnessTracker.Application.Mappers
{
    public class RegisterMapper
        : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Workout, WorkoutResponseDTO>()
                .Map(dto => dto.DurationInMinutes, x => (int)x.Duration.TotalMinutes)
                .RequireDestinationMemberSource(true);
            config.NewConfig<Exercise, ExerciseResponseDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<Set, SetResponseDTO>()
                .RequireDestinationMemberSource(true);
        }
    }
}
