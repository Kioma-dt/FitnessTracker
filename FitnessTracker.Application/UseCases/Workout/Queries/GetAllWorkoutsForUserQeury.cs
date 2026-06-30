using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;
using FitnessTracker.Shared.Exceptions.InternalServerError;

namespace FitnessTracker.Application.UseCases.Workout.Queries
{
    public record GetAllWorkoutsForUserQeury
    (
        string? UserId,
        int Page,
        int PageSize,
        List<WorkoutFilterDTO>? Filters,
        WorkoutOrderingDTO? Ordering
    )
        : IRequest<IEnumerable<WorkoutDTO>>;

    public class GetAllWorkoutsForUserQeuryHandler
        : IRequestHandler<GetAllWorkoutsForUserQeury, IEnumerable<WorkoutDTO>>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public GetAllWorkoutsForUserQeuryHandler(
            IWorkoutsRepository workoutsRepository, 
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkoutDTO>> Handle(
            GetAllWorkoutsForUserQeury request, 
            CancellationToken cancellationToken)
        {
            if (request.UserId is null)
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            var workouts = await _workoutsRepository.GetAllByUserIdAsync(
                request.UserId,
                request.Page,
                request.PageSize,
                request.Filters,
                request.Ordering);

            return _mapper.Map<IEnumerable<WorkoutDTO>>(workouts);

        }
    }
}
