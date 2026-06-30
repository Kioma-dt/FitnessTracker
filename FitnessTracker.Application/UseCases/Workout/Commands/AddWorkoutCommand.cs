using FitnessTracker.Application.Interfaces.Repositories;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record AddWorkoutCommand
    (
        WorkoutCreateDTO Workout
    )
        : IRequest<WorkoutDTO>;

    public class AddWorkoutCommandHandler
        : IRequestHandler<AddWorkoutCommand, WorkoutDTO>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public AddWorkoutCommandHandler(
            IWorkoutsRepository workoutsRepository, 
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutDTO> Handle(
            AddWorkoutCommand request, 
            CancellationToken cancellationToken)
        {
            if (request.Workout.UserId is null)
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            var workout = _mapper.Map<Entities.Workout>(request.Workout);

            await _workoutsRepository.AddAsync(workout);

            return _mapper.Map<WorkoutDTO>(workout);
        }
    }
}
