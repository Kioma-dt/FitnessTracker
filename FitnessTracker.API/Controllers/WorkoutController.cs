using FitnessTracker.Application.Repositories;
using FitnessTracker.Shared.DTO.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("workouts")]
    public class WorkoutController(IWorkoutsRepository workoutsRepository)
    {
        IWorkoutsRepository _workoutsRepository = workoutsRepository;

        [Authorize]
        [HttpGet]
        public async Task<Ok<IEnumerable<WorkoutResponseDTO>>> GetAll([FromServices] ClaimsPrincipal userInfo)
        {
            var userId = userInfo.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? String.Empty;
            var workouts = await _workoutsRepository.GetAllByUserIdAsync(userId);

            return TypedResults.Ok(workouts.Select(x => new WorkoutResponseDTO(
                x?.Id ?? String.Empty,
                x?.Title ?? String.Empty,
                x.Type,
                x.Duration.Minutes,
                x.CaloriesBurned,
                x.WorkoutDate,
                x.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList()
                )));
        }

        [Authorize]
        [HttpPost]
        public async Task<Created<WorkoutResponseDTO>> CreateWorkout([FromServices] ClaimsPrincipal userInfo,
            [FromBody] WorkoutCreateRequestDTO request)
        {
            var userId = userInfo.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? String.Empty;

            var workout = new Workout(userId,
                request.Title,
                request.Type,
                TimeSpan.FromMinutes(request.DurationInMinutes),
                request.CaloriesBurned,
                request.WorkoutDate,
                request.Exercises.Select(x => new Exercise(x.Name,
                x.Sets.Select(s => new Set(s.Reps, s.Weight)).ToList())).ToList());

            await _workoutsRepository.AddAsync(workout);

            return TypedResults.Created("Smth", new WorkoutResponseDTO(
                workout?.Id ?? String.Empty,
                workout?.Title ?? String.Empty,
                workout.Type,
                workout.Duration.Minutes,
                workout.CaloriesBurned,
                workout.WorkoutDate,
                workout.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList()
                ));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> GetById([FromServices] ClaimsPrincipal userInfo,
            string id)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var userId = userInfo.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? String.Empty;

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            return TypedResults.Ok(new WorkoutResponseDTO(
                workout?.Id ?? String.Empty,
                workout?.Title ?? String.Empty,
                workout.Type,
                workout.Duration.Minutes,
                workout.CaloriesBurned,
                workout.WorkoutDate,
                workout.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList()
                ));
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> UpdateInfo([FromServices] ClaimsPrincipal userInfo, 
            string id,
            [FromBody] WorkoutUpdateRequestDTO request)
        {
            var workout = await _workoutsRepository.GetByIdAsync(id);

            if (workout is null)
            {
                throw new EntityNotFoundException($"No workout with id: {id}");
            }

            var userId = userInfo.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? String.Empty;

            if (workout.UserId != userId)
            {
                throw new AccessDeniedException($"You don't have rights for workout with id: {id}");
            }

            await _workoutsRepository.UpdateAsync(id, 
                new WorkoutUpdateDTO(request.Title, 
                    request.Type, 
                    TimeSpan.FromMinutes(request.DurationInMinutes),
                    request.CaloriesBurned,
                    request.WorkoutDate
                 ));

            return TypedResults.Ok(new WorkoutResponseDTO(
                workout?.Id ?? String.Empty,
                workout?.Title ?? String.Empty,
                workout.Type,
                workout.Duration.Minutes,
                workout.CaloriesBurned,
                workout.WorkoutDate,
                workout.Exercises.Select(e => new ExerciseResponseDTO
                (
                    e?.Name ?? String.Empty,
                    e.Sets.Select(s => new SetResponseDTO(
                        s.Weight,
                        s.Reps)).ToList()
                )).ToList()
                ));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<NoContent> Delete(string id)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPatch("{id}/exercises")]
        public async Task<NoContent> AddExercise(string id, [FromBody] ExerciseCreateRequestDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
