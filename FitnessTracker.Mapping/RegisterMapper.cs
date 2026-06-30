namespace FitnessTracker.Mapping
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

            config.NewConfig<WorkoutCreateRequestDTO, Workout>()
                .Map(x => x.Duration, dto => TimeSpan.FromMinutes(dto.DurationInMinutes))
                .Map(x => x.UserId, dto => dto.GetUserId())
                .Ignore(x => x.CreatedAt)
                .Ignore(x => x.Id!)
                .Ignore(x => x.User!)
                .RequireDestinationMemberSource(true);
            config.NewConfig<ExerciseCreateRequestDTO, Exercise>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<SetCreateRequestDTO, Set>()
                .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutUpdateRequestDTO, WorkoutUpdateDTO>()
                .Map(x => x.Duration, dto => TimeSpan.FromMinutes(dto.DurationInMinutes))
                .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutPatchRequestDTO, WorkoutUpdateDTO>()
                    .Map(
                            x => x.Duration,
                            dto => dto.DurationInMinutes.HasValue
                                ? TimeSpan.FromMinutes(dto.DurationInMinutes.Value)
                                : new TimeSpan?()
                    )
                    .RequireDestinationMemberSource(true);


            config.NewConfig<User, UserDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<UserDTO, UserResponseDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<UserTokenDTO, UserTokenResponseDTO>()
                .RequireDestinationMemberSource(true);

        }
    }
}
