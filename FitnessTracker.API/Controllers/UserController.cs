using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using MapsterMapper;
using FitnessTracker.Application.Interfaces;
using FitnessTracker.Application.Interfaces.Repositories;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(IUsersRepository usersRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenFactory jwtTokenFactory,
        IMapper mapper)
        : ControllerBase
    {
        IUsersRepository _usersRepository = usersRepository;
        IPasswordHasher _passwordHasher = passwordHasher;
        IJwtTokenFactory _jwtTokenFactory = jwtTokenFactory;
        IMapper _mapper = mapper;

        [HttpPost("register", Name = "RegisterUser")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {

            var dbUser = await _usersRepository.GetByNameAsync(request.UserName);

            if (dbUser is not null)
            {
                throw new EntityAlreadyExistsException($"User With Name: {request.UserName} already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User(request.UserName, passwordHash);

            await _usersRepository.AddAsync(user);

            var userResonse = _mapper.Map<UserResponseDTO>(user);

            return CreatedAtRoute("None", userResonse);
        }

        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(typeof(UserTokenResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var user = await _usersRepository.GetByNameAsync(request.UserName);

            if (user is null)
            {
                throw new LoginException($"No User With Name: {request.UserName}");
            }

            if (user.PasswordHash is null)
            {
                throw new LoginException($"User :{request.UserName} has no password");
            }

            if (!_passwordHasher.VerifyPassword(request.Password,
                 user.PasswordHash
            ))
            {
                throw new LoginException("Wrong Password");
            }

            var jwtToken = _jwtTokenFactory.Create(user);

            var userResonse = _mapper.Map<UserResponseDTO>(user);

            return Ok(new UserTokenResponseDTO(jwtToken, userResonse));
        }
    }
}
