using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users.Create
{
    public class CreateUserCommandHandler : HandlerBase<CreateUserCommand>
    {
        private readonly IPasswordHasher<User> _hasher;

        public CreateUserCommandHandler(ISecretsDbContext dbContext, IPasswordHasher<User> hasher) : base(dbContext) 
        {
            _hasher = hasher;
        }

        public async override Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Users.Any(user => user.Login == request.Login))
            {
                throw new CreateUserException(request.Login);
            }

            var dbUser = new User
            {
                Login = request.Login
            };
            dbUser.PasswordHash = _hasher.HashPassword(dbUser, request.Password);

            _dbContext.Users.Add(dbUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
