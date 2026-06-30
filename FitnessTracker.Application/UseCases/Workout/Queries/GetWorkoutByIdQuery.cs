using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.Workout;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.UseCases.Workout.Queries
{
    public record GetWorkoutByIdQuery
    (
        string Id
    )
        : IRequest<WorkoutDTO>;

    public class GetWorkoutByIdQueryHandler
        : IRequestHandler<GetWorkoutByIdQuery, WorkoutDTO>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public GetWorkoutByIdQueryHandler(
            IWorkoutsRepository workoutsRepository,
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutDTO> Handle(
            GetWorkoutByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.Id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {request.Id}");
            }

            return _mapper.Map<WorkoutDTO>(workout);
        }
    }
}
