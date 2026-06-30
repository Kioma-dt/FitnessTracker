using FitnessTracker.Application.Interfaces.Cache;
using FitnessTracker.Application.UseCases.Workout.Queries;
using FitnessTracker.Inrastructure.Cache;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FitnessTracker.API.Authorization
{
    public interface IWorkoutOwnerAuthorizationService
    {
        Task CheckWorkoutOwner(
            string WorkoutId,
            ClaimsPrincipal User);
    }
    public class WorkoutOwnerAuthorizationService
        : IWorkoutOwnerAuthorizationService
    {
        IAuthorizationService _authorizationService;
        IMediator _mediator;
        IMapper _mapper;

        public WorkoutOwnerAuthorizationService(
            IAuthorizationService authorizationService,
            IMediator mediator,
            IMapper mapper)
        {
            _authorizationService = authorizationService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task CheckWorkoutOwner(
            string WorkoutId, 
            ClaimsPrincipal User)
        {
            var workout = await _mediator.Send(new GetWorkoutByIdQuery(
               WorkoutId));

            var workoutOwnerAuthorization = _mapper.Map<WorkoutOwnerAuthorizationDTO>(workout);

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workoutOwnerAuthorization,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {WorkoutId}");
            }
        }
    }
}
