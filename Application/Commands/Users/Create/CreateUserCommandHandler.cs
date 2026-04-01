using Application.Interfaces;
using Domain;

namespace Application.Commands.Users.Create
{
    public class CreateUserCommandHandler : HandlerBase<CreateUserCommand>
    {
        public CreateUserCommandHandler(ISecretsDbContext dbContext) : base(dbContext) { }

        public async override Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Users.Any(user => user.Login == request.Login))
            {
                throw new CreateUserException(request.Login);
            }

            var dbUser = new User
            {
                Login = request.Login,
                PasswordHash = request.Password.GetHashCode()
            };
            _dbContext.Users.Add(dbUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
