using FitnessTracker.Application.Repositories;

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
        public async Task<Created<WorkoutResponseDTO>> CreatWorkout([FromBody] WorkoutCreateRequestDTO request)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<Ok<WorkoutResponseDTO>> Update(string id, [FromBody] WorkoutUpdateRequestDTO request)
        {
            throw new NotImplementedException();
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
