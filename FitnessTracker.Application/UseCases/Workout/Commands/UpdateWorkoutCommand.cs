using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Responses;

namespace FitnessTracker.Application.UseCases.Workout.Commands
{
    public record UpdateWorkoutCommand
    (
        string Id,
        WorkoutUpdateDTO WorkoutUpdateInfo
    )
        : IRequest<WorkoutDTO>;

    public class UpdateWorkoutCommandHandler
        : IRequestHandler<UpdateWorkoutCommand, WorkoutDTO>
    {
        IWorkoutsRepository _workoutsRepository;
        IMapper _mapper;

        public UpdateWorkoutCommandHandler(
            IWorkoutsRepository workoutsRepository,
            IMapper mapper)
        {
            _workoutsRepository = workoutsRepository;
            _mapper = mapper;
        }

        public async Task<WorkoutDTO> Handle(
            UpdateWorkoutCommand request, 
            CancellationToken cancellationToken)
        {
            var workout = await _workoutsRepository.GetByIdAsync(request.Id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {request.Id}");
            }

            var workoutUpdated = await _workoutsRepository.UpdateAsync(
                request.Id, 
                request.WorkoutUpdateInfo);

            return _mapper.Map<WorkoutDTO>(workoutUpdated);
        }
    }
}
