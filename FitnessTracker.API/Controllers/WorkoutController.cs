using FitnessTracker.API.Cache;
using FitnessTracker.Application.Interfaces.Cache;
using FitnessTracker.Application.UseCases.Workout.Queries;
using FitnessTracker.Application.UseCases.Workout.Commands;
using FitnessTracker.API.Authorization;
using FitnessTracker.Shared.DTO.Queries.Workout;
using FitnessTracker.Shared.DTO.Requests.Workout;
using FitnessTracker.Shared.DTO.Responses.Workout;
using FitnessTracker.Shared.DTO.Application.Workout;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("workouts")]
    public class WorkoutController
        : ControllerBase
    {
        IWorkoutOwnerAuthorizationService _workoutOwnerAuthorizationService;
        IMediator _mediator;
        IMapper _mapper;

        public WorkoutController(
            IWorkoutOwnerAuthorizationService workoutOwnerAuthorizationService, 
            IMediator mediator, 
            IMapper mapper)
        {
            _workoutOwnerAuthorizationService = workoutOwnerAuthorizationService;
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet(Name = "GetAllWorkouts")]
        [ProducesResponseType(typeof(PagedResponseDTO<WorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            [FromQuery] WorkoutFiltersQueryDTO filtersQuery,
            [FromQuery] WorkoutOrderingQueryDTO orderingQuery,
            [FromQuery] WorkoutPagesQueryDTO pagesQuery) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var filters = filtersQuery.ToList();

            WorkoutOrderingDTO? ordering = null;
            if (orderingQuery.OrderBy is not null)
            {
                ordering = new WorkoutOrderingDTO(orderingQuery.OrderBy.Value, 
                    orderingQuery.Descending ?? false);
            }

            var workouts = await _mediator.Send(new GetAllWorkoutsForUserQeury(
                userId,
                pagesQuery.Page,
                pagesQuery.PageSize,
                filters,
                ordering));

            var totalWorkouts = await _mediator.Send(new GetTotalWorkoutsForUserQeury(
                userId,
                filters));

            var workoutsResult = _mapper.Map<IEnumerable<WorkoutResponseDTO>>(workouts);

            return Ok(new PagedResponseDTO<WorkoutResponseDTO>(
                workoutsResult.ToList(),
                pagesQuery.Page,
                pagesQuery.PageSize,
                totalWorkouts.Total
                ));
        }

        [Authorize]
        [HttpPost(Name = "CreateWorkout")]
        [ProducesResponseType(typeof(WorkoutResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateWorkout([FromBody] WorkoutCreateRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.SetUserId(userId);

            var workout = await _mediator.Send(new CreateWorkoutCommand(
                _mapper.Map<WorkoutCreateDTO>(request)));

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            return Created($"api/v1/workouts/{workout.Id}", workoutResponse);
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(typeof(WorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);

            var workout = await _mediator.Send(new GetWorkoutByIdQuery(id));

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            var currentETag = await _mediator.Send(new GetWorkoutETagQuery(
                id));

            if (ETagHelper.IsNotModified(Request, currentETag.ETag))
                return StatusCode(StatusCodes.Status304NotModified);

            ETagHelper.SetETag(Response, currentETag.ETag);

            return Ok(workoutResponse);
        }

        [Authorize]
        [HttpPut("{id}", Name = "FullUpdateWorkout")]
        [ProducesResponseType(typeof(WorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WorkoutResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromRoute] string id,
            [FromBody] WorkoutPutRequestDTO request)
        {
            var isWorkoutExists = await _mediator.Send(new IsWorkoutWithIdExistsQuery(id));
            if (!isWorkoutExists)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var workout = request.MapToWorkoutCreateDTO(id, userId);

                await _mediator.Send(new CreateWorkoutCommand(workout));

                var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

                return Created($"api/v1/workouts/{workout.Id}", workoutResponse);

            }

            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);

            var workoutUpdated = await _mediator.Send(new UpdateWorkoutCommand(
                id,
                _mapper.Map<WorkoutUpdateDTO>(request)));

            var workoutUpdatedResponse = _mapper.Map<WorkoutResponseDTO>(workoutUpdated);

            return Ok(workoutUpdatedResponse);
        }

        [Authorize]
        [HttpPatch("{id}", Name = "PartialUpdateWorkout")]
        [ProducesResponseType(typeof(WorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch([FromRoute] string id,
            [FromBody] WorkoutPatchRequestDTO request)
        {
            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);
            
            var currentETag = await _mediator.Send(new GetWorkoutETagQuery(
                id));
            
            ETagHelper.ValidateIfMatch(Request, currentETag.ETag);
            
            var workoutUpdated = await _mediator.Send(new UpdateWorkoutCommand(id,
                _mapper.Map<WorkoutUpdateDTO>(request)));

            var workoutUpdatedResponse = _mapper.Map<WorkoutResponseDTO>(workoutUpdated);

            return Ok(workoutUpdatedResponse);
        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteWorkout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<NoContent> Delete([FromRoute] string id)
        {
            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);

            await _mediator.Send(new DeleteWorkoutCommand(id));

            return TypedResults.NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/exercises", Name = "AddExerciseToWorkout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddExercise([FromRoute] string id, 
            [FromBody] ExerciseCreateRequestDTO request)
        {
            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);

            await _mediator.Send(new AddExerciseCommand(
                id,
                _mapper.Map<ExerciseCreateDTO>(request)));

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/photos", Name = "AddPhotoToWorkout")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddPhoto(
            string id, 
            IFormFile file)
        {
            await _workoutOwnerAuthorizationService.CheckWorkoutOwner(id, User);

            await using var imageStream = file.OpenReadStream();

            await _mediator.Send(new AddProgressPhotoCommand(
                id,
                imageStream));

            return NoContent();
        }
    }
}
