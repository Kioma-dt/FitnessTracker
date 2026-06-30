using FitnessTracker.Application.Interfaces.Cache;
using FitnessTracker.Application.Interfaces.Repositories;

namespace FitnessTracker.Application.UseCases.Workout.Queries
{
    public record GetWorkoutETagQuery
    (
        string Id
    ) 
        : IRequest<ETagDTO>;
    public class GetWorkoutETagQueryHandler
        : IRequestHandler<GetWorkoutETagQuery, ETagDTO>
    {
        IWorkoutsRepository _workoutsRepository;
        IETagGenerator _eTagGenerator;
        public GetWorkoutETagQueryHandler(
            IWorkoutsRepository workoutsRepository, 
            IETagGenerator eTagGenerator)
        {
            _workoutsRepository = workoutsRepository;
            _eTagGenerator = eTagGenerator;
        }

        public async Task<ETagDTO> Handle(
            GetWorkoutETagQuery request, 
            CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.Id);

            return new ETagDTO(_eTagGenerator.Generate(workout ?? new Entities.Workout()));
        }
    }
}
