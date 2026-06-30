using FitnessTracker.Application.Interfaces.Repositories;

namespace FitnessTracker.Application.UseCases.Workout.Queries
{
    public record IsWorkoutWithIdExistsQuery
    (
        string Id
    )
        : IRequest<bool>;

    public class IsWorkoutWithIdExistsQueryHandler
        : IRequestHandler<IsWorkoutWithIdExistsQuery, bool>
    {
        IWorkoutsRepository _workoutsRepository;

        public IsWorkoutWithIdExistsQueryHandler(
            IWorkoutsRepository workoutsRepository)
        {
            _workoutsRepository = workoutsRepository;
        }

        public async Task<bool> Handle(
            IsWorkoutWithIdExistsQuery request,
            CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.Id);

            if (workout is null)
            {
                return false;
            }

            return true;
        }
    }
}
