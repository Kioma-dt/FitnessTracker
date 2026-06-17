using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.Repositories;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(IUsersRepository usersRepository,
        IPasswordHasher passwordHasher)
    {
        IUsersRepository _usersRepository = usersRepository;
        IPasswordHasher _passwordHasher = passwordHasher;

        [HttpPost("register")]
        public async Task<
            Results<Created<SuccessResponse<RegisterResponseDTO>>,
                BadRequest<StatusResponse>,
                Conflict<StatusResponse>,
                InternalServerError<StatusResponse>>
            > Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            try
            { 
                var dbUser = await _usersRepository.GetByNameAsync(registerRequestDTO.UserName);

                if (dbUser is not null)
                {
                    throw new EntityAlreadyExistsException($"User With Name: {registerRequestDTO.UserName} already exists");
                }

                var passwordHash = _passwordHasher.HashPassword(registerRequestDTO.Password);


                await _usersRepository.AddAsync(new User(registerRequestDTO.UserName, passwordHash));

                return TypedResults.Created("Register",
                    new SuccessResponse<RegisterResponseDTO>(201,
                    "User Registered",
                    new RegisterResponseDTO(registerRequestDTO.UserName)));
            }
            catch (EntityAlreadyExistsException ex)
            {
                return TypedResults.Conflict(new StatusResponse(412, ex.Message));
            }
            catch (Exception ex)
            {
                return TypedResults.InternalServerError(new StatusResponse(500, ex.Message));
            }
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
