using Application.Interfaces;
using MediatR;

namespace Application 
{
    public abstract class HandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ISecretsDbContext _dbContext;

        public HandlerBase(ISecretsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class HandlerBase<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        protected readonly ISecretsDbContext _dbContext;

        public HandlerBase(ISecretsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
    }
}
