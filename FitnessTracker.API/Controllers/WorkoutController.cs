using FitnessTracker.API.Cache;
using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.StreamImageChecker;
using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Shared.DTO.Queries;

using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("workouts")]
    public class WorkoutController(IWorkoutsRepository workoutsRepository,
        IAuthorizationService authorizationService,
        IETagGenerator eTagGenerator,
        IMapper mapper)
        : ControllerBase
    {
        IWorkoutsRepository _workoutsRepository = workoutsRepository;
        IAuthorizationService _authorizationService = authorizationService;
        IETagGenerator _eTagGenerator = eTagGenerator;
        IMapper _mapper = mapper;

        [Authorize]
        [HttpGet(Name = "GetAllWorkouts")]
        [ProducesResponseType(typeof(PagedResponseDTO<WorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
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

            if (userId is null) 
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            var filters = filtersQuery.ToList();

            WorkoutOrderingDTO? ordering = null;
            if (orderingQuery.OrderBy is not null)
            {
                ordering = new WorkoutOrderingDTO(orderingQuery.OrderBy.Value, 
                    orderingQuery.Descending ?? false);
            }

            var totalWorkouts = await _workoutsRepository.GetTotalCountByUserAsync(userId, filters);

            var workouts = await _workoutsRepository.GetAllByUserIdAsync(
                userId, 
                pagesQuery.Page,
                pagesQuery.PageSize,
                filters,
                ordering);

            var workoutsResult = _mapper.Map<IEnumerable<WorkoutResponseDTO>>(workouts);

            var currentETag = _eTagGenerator.Generate(workoutsResult);

            if (ETagHelper.IsNotModified(Request, currentETag))
                return StatusCode(StatusCodes.Status304NotModified);

            ETagHelper.SetETag(Response, currentETag);

            return Ok(new PagedResponseDTO<WorkoutResponseDTO>(
                workoutsResult.ToList(),
                pagesQuery.Page,
                pagesQuery.PageSize,
                totalWorkouts
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

            if (userId is null)
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            request.SetUserId(userId);

            var workout = _mapper.Map<Workout>(request);

            await _workoutsRepository.AddAsync(workout);

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            return CreatedAtRoute($"api/v1/workouts/{workout.Id}", workoutResponse);
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
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if(!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            var currentETag = _eTagGenerator.Generate(workout);

            if (ETagHelper.IsNotModified(Request, currentETag))
                return StatusCode(StatusCodes.Status304NotModified);

            ETagHelper.SetETag(Response, currentETag);

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
            [FromBody] WorkoutUpdateRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                throw new NoInfoInJWTTokenExeption("No user id in JWT token");
            }

            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                workout = _mapper.Map<Workout>(request);

                workout.Id = id;

                await _workoutsRepository.AddAsync(workout);

                var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

                return CreatedAtRoute($"api/v1/workouts/{workout.Id}", workoutResponse);

            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutUpdateDTO = _mapper.Map<WorkoutUpdateDTO>(request);

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id, workoutUpdateDTO);

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
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutUpdateDTO = _mapper.Map<WorkoutUpdateDTO>(request);

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id,workoutUpdateDTO);

            var workoutUpdatedResponse = _mapper.Map<WorkoutResponseDTO>(workoutUpdated);

            return Ok(workoutUpdatedResponse);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<NoContent> Delete([FromRoute] string id)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            await _workoutsRepository.DeleteAsync(id);

            return TypedResults.NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/exercises")]
        public async Task<NoContent> AddExercise([FromRoute] string id, 
            [FromBody] ExerciseCreateRequestDTO request)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var exercise = _mapper.Map<Exercise>(request);

            await _workoutsRepository.AddExerciseAsync(id, exercise);

            return TypedResults.NoContent();
        }

        [Authorize]
        [HttpPatch("{id}/photos")]
        [Consumes("multipart/form-data")]
        public async Task<NoContent> AddPhoto([FromServices] IPhotosRemoteStorage photosRemoteStorage,
            [FromServices] IStreamImageChecker streamImageChecker,
            string id, 
            IFormFile file)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                workout,
                "WorkoutOwner");

            if (!authorizationResult.Succeeded)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                throw new UnsuportedFileFormatException("File should be image");
            }

            using(var streamCheck = file.OpenReadStream()) 
            {
                if (!(await streamImageChecker.IsSteamImage(streamCheck)))
                {
                    throw new UnprocessableImageException("Erros occuried while decoding image");
                }
            }

            using(var stream = file.OpenReadStream())          
            {
                var url = await photosRemoteStorage.Upload(stream);

                await _workoutsRepository.AddPhotoAsync(id, url);

                return TypedResults.NoContent();
            }
        }
    }
}
