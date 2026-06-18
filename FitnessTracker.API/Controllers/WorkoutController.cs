using FitnessTracker.Application.PhotosRemoteStorage;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.StreamImageChecker;
using FitnessTracker.Application.WorkoutFilters;
using FitnessTracker.Shared.DTO.Queries;
using FitnessTracker.Shared.DTO.Repositories;
using Imagekit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("workouts")]
    public class WorkoutController(IWorkoutsRepository workoutsRepository)
        : ControllerBase
    {
        IWorkoutsRepository _workoutsRepository = workoutsRepository;

        [Authorize]
        [HttpGet]
        public async Task<Ok<IEnumerable<WorkoutResponseDTO>>> GetAll([FromServices] IWorkoutFilterExpressionBuilder filterExpressionBuilder,
            [FromQuery] WorkoutFiltersQueryDTO filtersQuery)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            var filter = filterExpressionBuilder.BuildFilterExpression(filtersQuery.ToList());

            var workouts = await _workoutsRepository.GetAllByUserIdAsync(userId, filter);

            return TypedResults.Ok(workouts.Select(x => new WorkoutResponseDTO(
                x?.Id ?? String.Empty,
                x?.Title ?? String.Empty,
                x.Type,
                (int)x.Duration.TotalMinutes,
                x.CaloriesBurned,
                x.WorkoutDate,
                x.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList(),
                x.ProgressPhotos
                )));
        }

        [Authorize]
        [HttpPost]
        public async Task<Created<WorkoutResponseDTO>> CreateWorkout([FromBody] WorkoutCreateRequestDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            var workout = new Workout(userId,
                request.Title,
                request.Type,
                TimeSpan.FromMinutes(request.DurationInMinutes),
                request.CaloriesBurned,
                request.WorkoutDate,
                request.Exercises.Select(x => new Exercise(x.Name,
                x.Sets.Select(s => new Set(s.Reps, s.Weight)).ToList())).ToList(),
                request.ProgressPhotos);

            await _workoutsRepository.AddAsync(workout);

            return TypedResults.Created("Smth", new WorkoutResponseDTO(
                workout?.Id ?? String.Empty,
                workout?.Title ?? String.Empty,
                workout.Type,
                (int)workout.Duration.TotalMinutes,
                workout.CaloriesBurned,
                workout.WorkoutDate,
                workout.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList(),
                workout.ProgressPhotos
                ));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> GetById(string id)
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

            return TypedResults.Ok(new WorkoutResponseDTO(
                workout?.Id ?? String.Empty,
                workout?.Title ?? String.Empty,
                workout.Type,
                (int)workout.Duration.TotalMinutes,
                workout.CaloriesBurned,
                workout.WorkoutDate,
                workout.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList(),
                workout.ProgressPhotos
                ));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<Results<Ok<WorkoutResponseDTO>, Created<WorkoutResponseDTO>>> Put(string id,
            [FromBody] WorkoutUpdateRequestDTO request)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

            if (workout is null)
            {
                //throw new EntityNotFoundException($"No workout with id: {id}");

                workout = new Workout(userId,
                    request.Title,
                    request.Type,
                    TimeSpan.FromMinutes(request.DurationInMinutes),
                    request.CaloriesBurned,
                    request.WorkoutDate,
                    request.Exercises.Select(x => new Exercise(x.Name,
                    x.Sets.Select(s => new Set(s.Reps, s.Weight)).ToList())).ToList(),
                    request.ProgressPhotos);

                workout.Id = id;

                await _workoutsRepository.AddAsync(workout);

                return TypedResults.Created("Smth", new WorkoutResponseDTO(
                    workout?.Id ?? String.Empty,
                    workout?.Title ?? String.Empty,
                    workout.Type,
                    (int)workout.Duration.TotalMinutes,
                    workout.CaloriesBurned,
                    workout.WorkoutDate,
                    workout.Exercises.Select(e => new ExerciseResponseDTO
                    (
                        e?.Name ?? String.Empty,
                        e.Sets.Select(s => new SetResponseDTO(
                            s.Weight,
                            s.Reps)).ToList()
                    )).ToList(),
                    workout.ProgressPhotos
                    ));

            }

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            var workoutTimeSpan = TimeSpan.FromMinutes(request.DurationInMinutes);

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id,
                new WorkoutUpdateDTO(request.Title,
                    request.Type,
                    workoutTimeSpan,
                    request.CaloriesBurned,
                    request.WorkoutDate,
                    request.Exercises.Select(e => new ExerciseUpdateDTO
                            (
                                e?.Name ?? String.Empty,
                                e.Sets.Select(s => new SetUpdateDTO(
                                    s.Weight,
                                    s.Reps)).ToList()
                            )).ToList(),
                    request.ProgressPhotos
                 ));

            return TypedResults.Ok(new WorkoutResponseDTO(
                workoutUpdated?.Id ?? String.Empty,
                workoutUpdated?.Title ?? String.Empty,
                workoutUpdated.Type,
                (int)workoutUpdated.Duration.TotalMinutes,
                workoutUpdated.CaloriesBurned,
                workoutUpdated.WorkoutDate,
                workoutUpdated.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList(),
                workout.ProgressPhotos
                ));
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> Patch(string id,
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

            TimeSpan? workoutTimeSpan = request.DurationInMinutes is not null
                ? TimeSpan.FromMinutes(request.DurationInMinutes.Value) 
                : null;

            var workoutUpdated = await _workoutsRepository.UpdateAsync(id, 
                new WorkoutUpdateDTO(request.Title, 
                    request.Type,
                    workoutTimeSpan,
                    request.CaloriesBurned,
                    request.WorkoutDate,
                    request?.Exercises?.Select(e => new ExerciseUpdateDTO
                            (
                                e?.Name ?? String.Empty,
                                e?.Sets.Select(s => new SetUpdateDTO(
                                    s.Weight,
                                    s.Reps)).ToList()
                            )).ToList(),
                    request?.ProgressPhotos
                 ));

            return TypedResults.Ok(new WorkoutResponseDTO(
                workoutUpdated?.Id ?? String.Empty,
                workoutUpdated?.Title ?? String.Empty,
                workoutUpdated.Type,
               (int)workoutUpdated.Duration.TotalMinutes,
                workoutUpdated.CaloriesBurned,
                workoutUpdated.WorkoutDate,
                workoutUpdated.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList(),
                workoutUpdated.ProgressPhotos
                ));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<NoContent> Delete(string id)
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
        public async Task<NoContent> AddExercise(string id, 
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


            await _workoutsRepository.AddExerciseAsync(id, new Exercise(request.Name ?? String.Empty,
                    request.Sets.Select(s => new Set(
                        s.Reps,
                        s.Weight)).ToList()));

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

        //[Authorize]
        //[HttpGet("{id}/photos")]
        //public async Task<IEnumerable<string>> GetAllPhotos(string id)
        //{
        //    var workout = await _workoutsRepository.GetByIdAsync(id);

        //    if (workout is null)
        //    {
        //        throw new EntityNotFoundException($"No workout with id: {id}");
        //    }

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;

        //    if (workout.UserId != userId)
        //    {
        //        throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
        //    }

        //    return workout.ProgressPhotos;
        //}
    }
}
