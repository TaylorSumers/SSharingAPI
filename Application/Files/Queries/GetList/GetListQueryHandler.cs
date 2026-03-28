using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Files.Queries.GetList
{
    public class GetListQueryHandler : IRequestHandler<GetListQuery, List<FileLookupDto>>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetListQueryHandler(ISecretsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<FileLookupDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var files = await _dbContext.Files
                .Where(file => file.UserId == request.UserId)
                .ProjectTo<FileLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return files;
        }
    }
}
