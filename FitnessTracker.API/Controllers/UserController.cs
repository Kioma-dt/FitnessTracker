using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.UseCases.User.Commands;

namespace FitnessTracker.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController
        : ControllerBase
    {
        IUsersRepository _usersRepository;
        IPasswordHasher _passwordHasher;
        IJwtTokenFactory _jwtTokenFactory;
        IMapper _mapper;
        IMediator _mediator;

        public UserController(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtTokenFactory jwtTokenFactory, IMapper mapper, IMediator mediator)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenFactory = jwtTokenFactory;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost("register", Name = "RegisterUser")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var user = await _mediator.Send(new RegisterUserCommand(
                request.UserName,
                request.Password));

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
