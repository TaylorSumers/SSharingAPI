using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Queries.Strings.Get
{
    public class GetStringQueryHandler : HandlerBase<GetStringQuery, StringVm>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetStringQueryHandler(ISecretsDbContext dbContext, IMapper mapper, IConfiguration configuration) : base(dbContext)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public async override Task<StringVm> Handle(GetStringQuery request, CancellationToken cancellationToken) 
        {
            var dbStr = await _dbContext.Strings.FirstOrDefaultAsync(str => str.Code == request.Code, cancellationToken);
            if (dbStr is null)
            {
                throw new NotFoundException(nameof(Domain.String), request.Code);
            }

            var strVm = _mapper.Map<StringVm>(dbStr);
            strVm.GetUrl = $"{_configuration["PublicApiBaseUrl"]}/api/Strings/Get/{dbStr.Code}";
            return strVm;
        }
    }
}
