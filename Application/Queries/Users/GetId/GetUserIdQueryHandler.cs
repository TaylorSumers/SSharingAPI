using Application.Commands;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.GetId
{
    public class GetUserIdQueryHandler : HandlerBase<GetUserIdQuery, int>
    {
        public GetUserIdQueryHandler(ISecretsDbContext dbContext) : base(dbContext) { }

        public async override Task<int> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken);
            if (dbUser is null || dbUser.PasswordHash != request.Password.GetHashCode())
            {
                throw new InvalidCredentialsException();
            }
            return dbUser.Id;
        }
    }
}
