using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.UseCases.User.Commands;
using FitnessTracker.Application.UseCases.User.Queries;

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
            var token = await _mediator.Send(new LogInUserQuery(
                request.UserName,
                request.Password));

            var tokenResonse = _mapper.Map<UserTokenResponseDTO>(token);

            return Ok(tokenResonse);
        }
    }
}
