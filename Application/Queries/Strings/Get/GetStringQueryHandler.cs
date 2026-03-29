using Application.Commands;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Strings.Get
{
    public class GetFileQueryHandler : HandlerBase<GetStringQuery, StringVm>
    {
        private readonly IMapper _mapper;

        public GetFileQueryHandler(ISecretsDbContext dbContext, IMapper mapper, YandexStorageService storageService) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async override Task<StringVm> Handle(GetStringQuery request, CancellationToken cancellationToken) 
        {
            var dbStr = await _dbContext.Strings.FirstOrDefaultAsync(str => str.Code == request.Code, cancellationToken);
            return _mapper.Map<StringVm>(dbStr);
        }
    }
}
