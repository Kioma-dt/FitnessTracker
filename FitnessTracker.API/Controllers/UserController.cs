using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using FitnessTracker.Application.PasswordHasher;
using FitnessTracker.Application.Repositories;
using FitnessTracker.Application.JwtTokenFactory;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(IUsersRepository usersRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenFactory jwtTokenFactory)
    {
        IUsersRepository _usersRepository = usersRepository;
        IPasswordHasher _passwordHasher = passwordHasher;
        IJwtTokenFactory _jwtTokenFactory = jwtTokenFactory;

        [HttpPost("register")]
        public async Task<Created<RegisterResponseDTO>>
            Register([FromBody] RegisterRequestDTO request)
        {

                var dbUser = await _usersRepository.GetByNameAsync(request.UserName);

                if (dbUser is not null)
                {
                    throw new EntityAlreadyExistsException($"User With Name: {request.UserName} already exists");
                }

                var passwordHash = _passwordHasher.HashPassword(request.Password);

                var user = new User(request.UserName, passwordHash);

                await _usersRepository.AddAsync(user);

                return TypedResults.Created("None",
                        new RegisterResponseDTO(user.Id ?? String.Empty, request.UserName));
        }

        [HttpPost("login")]
        public async Task<
            Results<Ok<SuccessResponse<LoginResponseDTO>>,
                BadRequest<StatusResponse>,
                UnauthorizedHttpResult,
                InternalServerError<StatusResponse>>
            > Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var user = await _usersRepository.GetByNameAsync(request.UserName);

                if (user is null)
                {
                    throw new LoginException($"No User With Name {request.UserName}");
                }

                var passHash = _passwordHasher.HashPassword(request.Password);
                if (!_passwordHasher.VerifyPassword(request.Password,
                     user.PasswordHash ?? String.Empty
                     ))
                {
                    throw new LoginException("Wrong Password");
                }

                var jwtToken = _jwtTokenFactory.Create(user);

                return TypedResults.Ok(new SuccessResponse<LoginResponseDTO>(200,
                    "Loged In Successfully",
                    new LoginResponseDTO(jwtToken, user.Id ?? String.Empty, request.UserName)));
            }
            catch(LoginException ex)
            {
                return TypedResults.Unauthorized();
            }
            catch(Exception ex)
            {
                return TypedResults.InternalServerError(new StatusResponse(500, ex.Message));
            }
        }
    }
}
