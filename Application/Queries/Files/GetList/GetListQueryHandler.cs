using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Files.GetList
{
    public class GetListQueryHandler : HandlerBase<GetListQuery, List<FileLookupDto>>
    {
        private readonly IMapper _mapper;

        public GetListQueryHandler(ISecretsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public override async Task<List<FileLookupDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var files = await _dbContext.Files
                .Where(file => file.UserId == request.UserId)
                .ProjectTo<FileLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return files;
        }
    }
}
