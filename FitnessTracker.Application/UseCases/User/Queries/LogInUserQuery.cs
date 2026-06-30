using FitnessTracker.Application.Interfaces.Authentication;
using FitnessTracker.Application.Interfaces.Repositories;
using FitnessTracker.Shared.DTO.Application.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessTracker.Application.UseCases.User.Queries
{
    public record LogInUserQuery
    (
        string UserName,
        string Password
    )
        : IRequest<UserTokenDTO>;

    public class LogInUserQueryHandler
        : IRequestHandler<LogInUserQuery, UserTokenDTO>
    {
        IUsersRepository _usersRepository;
        IPasswordHasher _passwordHasher;
        IJwtTokenFactory _jwtTokenFactory;
        IMapper _mapper;

        public LogInUserQueryHandler(
            IUsersRepository usersRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenFactory jwtTokenFactory,
            IMapper mapper)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenFactory = jwtTokenFactory;
            _mapper = mapper;
        }

        public async Task<UserTokenDTO> Handle(
            LogInUserQuery request,
            CancellationToken cancellationToken)
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
            return new UserTokenDTO(
                jwtToken,
                _mapper.Map<UserDTO>(user));
        }
    }
}
