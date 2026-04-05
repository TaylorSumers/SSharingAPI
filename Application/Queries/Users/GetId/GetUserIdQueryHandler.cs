using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users.GetId
{
    public class GetUserIdQueryHandler : HandlerBase<GetUserIdQuery, int>
    {
        private readonly IPasswordHasher<User> _hasher;

        public GetUserIdQueryHandler(ISecretsDbContext dbContext, IPasswordHasher<User> hasher) : base(dbContext) 
        {
            _hasher = hasher;
        }

        public async override Task<int> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken);
            if (dbUser is null || _hasher.VerifyHashedPassword(dbUser, dbUser.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }
            return dbUser.Id;
        }
    }
}
