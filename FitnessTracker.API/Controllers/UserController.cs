using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController
    {
        [HttpPost("register")]
        public async Task<
            Results<Created<SuccessResponse<RegisterResponseDTO>>,
                BadRequest<StatusResponse>,
                Conflict<StatusResponse>,
                InternalServerError<StatusResponse>>
            > Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public async Task<
            Results<Ok<SuccessResponse<LoginResponseDTO>>,
                BadRequest<StatusResponse>,
                UnauthorizedHttpResult,
                InternalServerError<StatusResponse>>
            > Login([FromBody] LoginRequestDTO registerRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
