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
                .RequireDestinationMemberSource(true);
            config.NewConfig<Exercise, ExerciseResponseDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<Set, SetResponseDTO>()
                .RequireDestinationMemberSource(true);
        }
    }
}
