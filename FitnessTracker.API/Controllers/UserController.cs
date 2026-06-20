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
        : ControllerBase
    {
        IUsersRepository _usersRepository = usersRepository;
        IPasswordHasher _passwordHasher = passwordHasher;
        IJwtTokenFactory _jwtTokenFactory = jwtTokenFactory;

        [HttpPost("register")]
        public async Task<Created<UserResponseDTO>> Register([FromBody] RegisterRequestDTO request)
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
                        new UserResponseDTO(user.Id ?? String.Empty, request.UserName));
        }

        [HttpPost("login")]
        public async Task<Ok<UserTokenResponseDTO>> Login([FromBody] LoginRequestDTO request)
        {
                var user = await _usersRepository.GetByNameAsync(request.UserName);

                if (user is null)
                {
                    throw new LoginException($"No User With Name: {request.UserName}");
                }

                var passHash = _passwordHasher.HashPassword(request.Password);
                if (!_passwordHasher.VerifyPassword(request.Password,
                     user.PasswordHash ?? String.Empty
                     ))
                {
                    throw new LoginException("Wrong Password");
                }

                var jwtToken = _jwtTokenFactory.Create(user);

                return TypedResults.Ok(new UserTokenResponseDTO(jwtToken, new UserResponseDTO(user.Id ?? String.Empty, request.UserName)));
        }
    }
}
