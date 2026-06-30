using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.Interfaces.Repositories;
using System.Globalization;
namespace FitnessTracker.Application.UseCases.User.Commands
{
    public record RegisterUserCommand
    (
        string UserName,
        string Password
    )
        : IRequest<UserDTO>;

    public class RegisterUserCommandHandler
        : IRequestHandler<RegisterUserCommand, UserDTO>
    {
        IUsersRepository _usersRepository;
        IPasswordHasher _passwordHasher;
        IMapper _mapper;

        public RegisterUserCommandHandler(
            IUsersRepository usersRepository,
            IPasswordHasher passwordHasher,
            IMapper mapper)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var dbUser = await _usersRepository.GetByNameAsync(request.UserName);

            if (dbUser is not null)
            {
                throw new EntityAlreadyExistsException($"User With Name: {request.UserName} already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new Entities.User(request.UserName, passwordHash);

            await _usersRepository.AddAsync(user);

            return _mapper.Map<UserDTO>(user);
        }
    }
}
