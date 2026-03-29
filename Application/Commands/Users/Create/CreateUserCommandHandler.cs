using Application.Interfaces;
using Domain;

namespace Application.Commands.Users.Create
{
    public class CreateUserCommandHandler : HandlerBase<CreateUserCommand>
    {
        public CreateUserCommandHandler(ISecretsDbContext dbContext) : base(dbContext) { }

        public async override Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
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
