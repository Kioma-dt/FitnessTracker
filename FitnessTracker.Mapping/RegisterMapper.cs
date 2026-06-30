using FitnessTracker.Shared.DTO.Authorization;

namespace FitnessTracker.Mapping
{
    public class RegisterMapper
        : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<Workout, WorkoutResponseDTO>()
            //    .Map(dto => dto.DurationInMinutes, x => (int)x.Duration.TotalMinutes)
            //    .RequireDestinationMemberSource(true);
            //config.NewConfig<Exercise, ExerciseResponseDTO>()
            //    .RequireDestinationMemberSource(true);
            //config.NewConfig<Set, SetResponseDTO>()
            //    .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutDTO, WorkoutOwnerAuthorizationDTO>();

            config.NewConfig<WorkoutDTO, WorkoutResponseDTO>()
                .Map(dto => dto.DurationInMinutes, x => (int)x.Duration.TotalMinutes)
                .RequireDestinationMemberSource(true);
            config.NewConfig<ExerciseDTO, ExerciseResponseDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<SetDTO, SetResponseDTO>()
                .RequireDestinationMemberSource(true);

            config.NewConfig<Workout, WorkoutDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<Exercise, ExerciseDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<Set, SetDTO>()
                .RequireDestinationMemberSource(true);

            //config.NewConfig<WorkoutCreateRequestDTO, Workout>()
            //    .Map(x => x.Duration, dto => TimeSpan.FromMinutes(dto.DurationInMinutes))
            //    .Map(x => x.UserId, dto => dto.GetUserId())
            //    .Ignore(x => x.CreatedAt)
            //    .Ignore(x => x.Id!)
            //    .Ignore(x => x.User!)
            //    .RequireDestinationMemberSource(true);
            //config.NewConfig<ExerciseCreateRequestDTO, Exercise>()
            //    .RequireDestinationMemberSource(true);
            //config.NewConfig<SetCreateRequestDTO, Set>()
            //    .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutCreateRequestDTO, WorkoutCreateDTO>()
                .Map(x => x.Duration, dto => TimeSpan.FromMinutes(dto.DurationInMinutes))
                .Map(x => x.UserId, dto => dto.GetUserId())
                .Ignore(x => x.Id)
                .RequireDestinationMemberSource(true);
            config.NewConfig<ExerciseCreateRequestDTO, ExerciseCreateDTO>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<SetCreateRequestDTO, SetCreateDTO>()
                .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutCreateDTO, Workout>()
                .Ignore(x => x.CreatedAt)
                .Map(x => x.Id, dto => dto.Id ?? Guid.NewGuid().ToString())
                .Ignore(x => x.User!)
                .RequireDestinationMemberSource(true);
            config.NewConfig<ExerciseCreateDTO, Exercise>()
                .RequireDestinationMemberSource(true);
            config.NewConfig<SetCreateDTO, Set>()
                .RequireDestinationMemberSource(true);

            config.NewConfig<WorkoutPutRequestDTO, WorkoutCreateDTO>()
                .Map(x => x.Duration, dto => TimeSpan.FromMinutes(dto.DurationInMinutes))
                .Ignore(x => x.Id)
                .Ignore(x => x.UserId)
                .RequireDestinationMemberSource(true);
            config.NewConfig<WorkoutPutRequestDTO, WorkoutUpdateDTO>()
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
