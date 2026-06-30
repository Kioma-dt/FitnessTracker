using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.Exceptions.NotFound;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record DeleteWorkoutCommand
    (
        string Id
    )
        : IRequest;

    public class DeleteWorkoutCommandHandler
        : IRequestHandler<DeleteWorkoutCommand>
    {
        IWorkoutsRepository _workoutsRepository;

        public DeleteWorkoutCommandHandler(IWorkoutsRepository workoutsRepository)
        {
            _workoutsRepository = workoutsRepository;
        }

        public async Task Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.Id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {request.Id}");
            }

            await _workoutsRepository.DeleteAsync(request.Id);
        }
    }
}
