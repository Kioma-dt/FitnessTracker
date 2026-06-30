using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;

namespace FitnessTracker.Application.UseCases.Workout.Queries
{
    public record GetTotalWorkoutsForUserQeury
   (
       string? UserId,
       List<WorkoutFilterDTO>? Filters
   )
       : IRequest<WorkoutsTotalDTO>;

    public class GetTotalWorkoutsForUserQeuryHandler
        : IRequestHandler<GetTotalWorkoutsForUserQeury, WorkoutsTotalDTO>
    {
        IWorkoutsRepository _workoutsRepository;

        public GetTotalWorkoutsForUserQeuryHandler(
            IWorkoutsRepository workoutsRepository)
        {
            _workoutsRepository = workoutsRepository;
        }

        public async Task<WorkoutsTotalDTO> Handle(
            GetTotalWorkoutsForUserQeury request,
            CancellationToken cancellationToken)
        {
            if (request.UserId is null)
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            var total = await _workoutsRepository.GetTotalCountByUserAsync(
                request.UserId,
                request.Filters);

            return new WorkoutsTotalDTO(total);
        }
    }
}
