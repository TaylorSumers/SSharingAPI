using Application.Commands;
using Application.Files.Queries.GetList;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Strings.GetList
{
    public class GetListQueryHandler : HandlerBase<GetListQuery, List<StringLookupDto>>
    {
        private readonly IMapper _mapper;

        public GetListQueryHandler(ISecretsDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async override Task<List<StringLookupDto>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var strings = await _dbContext.Strings
                .Where(str => str.UserId == request.UserId)
                .ProjectTo<StringLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return strings;
        }
    }
}
