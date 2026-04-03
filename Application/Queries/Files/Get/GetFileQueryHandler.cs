using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Queries.Files.Get
{
    public class GetFileQueryHandler : HandlerBase<GetFileQuery, FileVm>
    {
        private readonly IMapper _mapper;
        private readonly IYandexStorageService _storageService;
        private readonly IConfiguration _configuration;

        public GetFileQueryHandler(ISecretsDbContext dbContext, IMapper mapper, IYandexStorageService storageService, IConfiguration configuration) : base(dbContext)
        {
            _mapper = mapper;
            _storageService = storageService;
            _configuration = configuration;
        }

        public async override Task<FileVm> Handle(GetFileQuery request, CancellationToken cancellationToken) 
        {
            // Get from db
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == request.Code, cancellationToken);
            if (dbFile is null)
            {
                throw new NotFoundException(nameof(Domain.File), request.Code);
            }

            // Get from cloud
            var cloudFileName = $"{dbFile.Code}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            var result = await _storageService.ObjectService.GetAsync(cloudFileName);
            var content = await result.ReadAsByteArrayAsync();
            if (!result.IsSuccessStatusCode)
            {
                throw new S3RequestException(content.Errors);
            }

            var fileVm = _mapper.Map<FileVm>(dbFile);
            fileVm.Content = content.Value;
            fileVm.GetUrl = $"{_configuration["PublicApiBaseUrl"]}/api/Files/Get/{dbFile.Code}";
            return fileVm;
        }
    }
}
