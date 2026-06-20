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
        IMapper mapper)
        : ControllerBase
    {
        IWorkoutsRepository _workoutsRepository = workoutsRepository;
        IMapper _mapper = mapper;

        [Authorize]
        [HttpGet]
        public async Task<Ok<PagedResponseDTO<WorkoutResponseDTO>>> GetAll([FromServices] IWorkoutFilterExpressionBuilder filterExpressionBuilder,
            [FromQuery] WorkoutFiltersQueryDTO filtersQuery,
            [FromQuery] WorkoutOrderingQueryDTO orderingQuery,
            [FromQuery] WorkoutPagesQueryDTO pagesQuery) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            var filter = filterExpressionBuilder.BuildFilterExpression(filtersQuery.ToList());

            var totalWorkouts = await _workoutsRepository.GetTotalCountByUserAsync(userId);

            var workouts = await _workoutsRepository.GetAllByUserIdAsync(
                userId, 
                pagesQuery.Page,
                pagesQuery.PageSize,
                filter,
                orderingQuery.OrderBy, 
                orderingQuery.Descending);

            var workoutsResult = _mapper.Map<IEnumerable<WorkoutResponseDTO>>(workouts);

            return TypedResults.Ok(new PagedResponseDTO<WorkoutResponseDTO>(
                workoutsResult.ToList(),
                pagesQuery.Page,
                pagesQuery.PageSize,
                totalWorkouts
                ));
        }

        [Authorize]
        [HttpPost]
        public async Task<Created<WorkoutResponseDTO>> CreateWorkout([FromBody] WorkoutCreateRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            request.SetUserId(userId);

            var workout = _mapper.Map<Workout>(request);

            await _workoutsRepository.AddAsync(workout);

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            return TypedResults.Created("Smth", workoutResponse);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> GetById([FromRoute] string id)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

            return TypedResults.Ok(workoutResponse);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<Results<Ok<WorkoutResponseDTO>, Created<WorkoutResponseDTO>>> Put([FromRoute] string id,
            [FromBody] WorkoutUpdateRequestDTO request)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout is null)
            {
                //throw new EntityNotFoundException($"No workout with id: {id}");

                workout = _mapper.Map<Workout>(request);

                workout.Id = id;

                await _workoutsRepository.AddAsync(workout);

                var workoutResponse = _mapper.Map<WorkoutResponseDTO>(workout);

                return TypedResults.Created("Smth", workoutResponse);

            }

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutUpdateDTO = _mapper.Map<WorkoutUpdateDTO>(request);

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id, workoutUpdateDTO);

            var workoutUpdatedResponse = _mapper.Map<WorkoutResponseDTO>(workoutUpdated);

            return TypedResults.Ok(workoutUpdatedResponse);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> Patch([FromRoute] string id,
            [FromBody] WorkoutPatchRequestDTO request)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutUpdateDTO = _mapper.Map<WorkoutUpdateDTO>(request);

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id,workoutUpdateDTO);

            var workoutUpdatedResponse = _mapper.Map<WorkoutResponseDTO>(workoutUpdated);

            return TypedResults.Ok(workoutUpdatedResponse);
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout.UserId != userId)
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout.UserId != userId)
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout.UserId != userId)
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
