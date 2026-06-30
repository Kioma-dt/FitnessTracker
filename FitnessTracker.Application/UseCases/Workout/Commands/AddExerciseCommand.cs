using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;
using FitnessTracker.Shared.Exceptions.NotFound;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record AddExerciseCommand
    (
        string WokroutId,
        ExerciseCreateDTO Exercise
    )
        : IRequest;

    public class AddExerciseCommandHandler
        : IRequestHandler<AddExerciseCommand>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public AddExerciseCommandHandler(
            IWorkoutsRepository workoutsRepository, 
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task Handle(AddExerciseCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.WokroutId);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {request.WokroutId}");
            }

            await _workoutsRepository.AddExerciseAsync(
                request.WokroutId,
                _mapper.Map<Exercise>(request.Exercise));
        }
    }
}
