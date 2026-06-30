using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record CreateWorkoutCommand
    (
        WorkoutCreateDTO Workout
    )
        : IRequest<WorkoutDTO>;

    public class CreateWorkoutCommandHandler
        : IRequestHandler<CreateWorkoutCommand, WorkoutDTO>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public CreateWorkoutCommandHandler(
            IWorkoutsRepository workoutsRepository, 
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutDTO> Handle(
            CreateWorkoutCommand request, 
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
